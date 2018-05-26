using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Config.Consul
{
	public class ConsulConfigurationProvider : ConfigurationProvider
	{
		private readonly Func<IConsulClient> _clientFactory;

		public ConsulConfigurationProvider(Func<IConsulClient> clientFactory)
		{
			_clientFactory = clientFactory;
		}

		public override void Load()
		{
			Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			using (var client = _clientFactory())
			{
				var results = client.KV.List("").Result.Response;

				foreach (var pair in results)
				{
					Data[pair.Key] = AsString(pair.Value);
				}
			}
		}

		private static string AsString(byte[] bytes) =>
			Encoding.UTF8.GetString(bytes, 0, bytes.Length);
	}
}
