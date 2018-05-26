using Consul;
using Microsoft.Extensions.Configuration;

namespace Config.Consul
{
	public class ConsulConfigurationSource : IConfigurationSource
	{
		public string Prefix { get; set; }

		public ConsulConfigurationSource()
		{
			Prefix = string.Empty;
		}

		public IConfigurationProvider Build(IConfigurationBuilder builder)
		{
			return new ConsulConfigurationProvider(() => new ConsulClient(), Prefix);
		}
	}
}
