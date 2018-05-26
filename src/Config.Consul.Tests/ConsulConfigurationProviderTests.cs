using System;
using System.Text;
using System.Threading.Tasks;
using Consul;
using Shouldly;
using Xunit;

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

		private string Prefixed(string key) => _prefix + key;
		private Task Write(string key, string value) => _client.KV.Put(Pair(key, value));

		private KVPair Pair(string key, string value) => new KVPair(Prefixed(key))
		{
			Value = Encoding.UTF8.GetBytes(value)
		};

		public void Dispose()
		{
			_client.KV.DeleteTree(_prefix).Wait();
			_client.Dispose();
		}
	}
}
