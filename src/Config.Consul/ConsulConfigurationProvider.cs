using System;
using System.Collections.Generic;
using System.Text;
using Consul;
using Microsoft.Extensions.Configuration;

namespace Config.Consul
{
	public class ConsulConfigurationProvider : ConfigurationProvider
	{
		private readonly Func<IConsulClient> _clientFactory;
		private readonly string _prefix;

		public ConsulConfigurationProvider(Func<IConsulClient> clientFactory, string prefix)
		{
			_clientFactory = clientFactory;
			_prefix = prefix ?? string.Empty;
		}

		public override void Load()
		{
			Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			using (var client = _clientFactory())
			{
				var results = client.KV.List(_prefix).Result.Response;

				foreach (var pair in results)
				{
					var key = ReplacePathDelimiters(RemovePrefix(pair.Key));
					var value = AsString(pair.Value);

					Data[key] = value;
				}
			}
		}

		private string RemovePrefix(string input) => input.Substring(_prefix.Length);
		private static string ReplacePathDelimiters(string input) => input.Replace("/", ConfigurationPath.KeyDelimiter);
		private static string AsString(byte[] bytes) => Encoding.UTF8.GetString(bytes, 0, bytes.Length);
	}
}
