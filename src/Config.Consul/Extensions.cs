using Microsoft.Extensions.Configuration;

namespace Config.Consul
{
	public static class Extensions
	{
		public static IConfigurationBuilder AddConsul(this IConfigurationBuilder configurationBuilder)
		{
			return configurationBuilder.Add(new ConsulConfigurationSource());
		}
	}
}
