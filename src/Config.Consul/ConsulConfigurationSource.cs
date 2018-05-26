using Microsoft.Extensions.Configuration;

namespace Config.Consul
{
	public class ConsulConfigurationSource : IConfigurationSource
	{
		public IConfigurationProvider Build(IConfigurationBuilder builder)
		{
			return new ConsulConfigurationProvider();
		}
	}
}
