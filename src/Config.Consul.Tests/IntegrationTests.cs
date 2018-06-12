using System;
using System.Text;
using System.Threading.Tasks;
using Consul;
using Shouldly;
using Xunit;

namespace Microsoft.Extensions.Configuration.Consul.Tests
{
	public class IntegrationTests : IDisposable
	{
		private readonly ConsulClient _client;
		private readonly string _prefix;

		public IntegrationTests()
		{
			_client = new ConsulClient();
			_prefix = Guid.NewGuid() + "/";
		}

		[RequiresConsulFact]
		public void When_reading_from_non_existing_store()
		{
			var config = new ConfigurationBuilder()
				.AddConsul(prefix: "appsettings/twelve/")
				.Build()
				.Get<Configuration>();

			config.ShouldBeNull();
		}

		[RequiresConsulFact]
		public async void When_reading_values_which_exist()
		{
			await Write(nameof(Configuration.Name), "the name");
			await Write(nameof(Configuration.Description), "the description");

			var config = new ConfigurationBuilder()
				.AddConsul(prefix: _prefix)
				.Build()
				.Get<Configuration>();

			config.ShouldSatisfyAllConditions(
				() => config.Name.ShouldBe("the name"),
				() => config.Description.ShouldBe("the description")
			);
		}

		private Task Write(string key, string value) => _client.KV.Put(Pair(key, value));
		private KVPair Pair(string key, string value) => new KVPair(_prefix + key) { Value = Encoding.UTF8.GetBytes(value) };

		public void Dispose()
		{
			_client.KV.DeleteTree(_prefix).Wait();
			_client.Dispose();
		}


		public class Configuration
		{
			public string Name { get; set; }
			public string Description { get; set; }
		}
	}
}
