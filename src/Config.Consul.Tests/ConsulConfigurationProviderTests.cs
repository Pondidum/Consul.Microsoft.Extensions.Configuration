using System;
using System.Text;
using System.Threading.Tasks;
using Consul;
using Microsoft.Extensions.Configuration;
using Shouldly;

namespace Config.Consul.Tests
{
	public class ConsulConfigurationProviderTests : IDisposable
	{
		private readonly ConsulConfigurationProvider _provider;
		private readonly ConsulClient _client;
		private readonly string _prefix;

		public ConsulConfigurationProviderTests()
		{
			_client = new ConsulClient();
			_prefix = Guid.NewGuid() + "/";
			_provider = new ConsulConfigurationProvider(() => new ConsulClient(), _prefix);
		}

		[RequiresConsulFact]
		public void When_reading_a_non_existing_key()
		{
			_provider.TryGet("test", out var no).ShouldBeFalse();
		}

		[RequiresConsulFact]
		public async Task When_reading_an_existing_key()
		{
			await Write("a", "one");

			_provider.Load();
			_provider
				.TryGet("a", out var value)
				.ShouldBeTrue();

			value.ShouldBe("one");
		}

		[RequiresConsulFact]
		public async Task When_building_a_configuration_object()
		{
			var name = "Testing configuration";
			var time = TimeSpan.FromSeconds(146);

			await Write(nameof(TestConfig.Name), name);
			await Write(nameof(TestConfig.Interval), time.ToString());

			var config = new ConfigurationBuilder()
				.AddConsul(_prefix)
				.Build()
				.Get<TestConfig>();

			config.ShouldSatisfyAllConditions(
				() => config.Name.ShouldBe(name),
				() => config.Interval.ShouldBe(time)
			);
		}

		[RequiresConsulFact]
		public async Task When_building_a_composite_configuration_object()
		{
			var type = "nested";
			var name = "Testing configuration";
			var time = TimeSpan.FromSeconds(146);

			await Write("type", type);
			await Write("inner/name", name);
			await Write("inner/interval", time.ToString());

			var config = new ConfigurationBuilder()
				.AddConsul(_prefix)
				.Build()
				.Get<OuterConfig>();

			config.ShouldSatisfyAllConditions(
				() => config.Type.ShouldBe(type),
				() => config.Inner.Name.ShouldBe(name),
				() => config.Inner.Interval.ShouldBe(time)
			);
		}

		private Task Write(string key, string value) => _client.KV.Put(Pair(key, value));
		private KVPair Pair(string key, string value) => new KVPair(_prefix + key) { Value = Encoding.UTF8.GetBytes(value) };

		public void Dispose()
		{
			_client.KV.DeleteTree(_prefix).Wait();
			_client.Dispose();
		}

		private class OuterConfig
		{
			public string Type { get; set; }
			public TestConfig Inner { get; set; }
		}

		private class TestConfig
		{
			public string Name { get; set; }
			public TimeSpan Interval { get; set; }
		}
	}
}
