using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Config.Consul
{
	public class ConsulConfigurationProvider : IConfigurationProvider
	{
		public bool TryGet(string key, out string value)
		{
			throw new System.NotImplementedException();
		}

		public void Set(string key, string value)
		{
			throw new System.NotImplementedException();
		}

		public IChangeToken GetReloadToken()
		{
			throw new System.NotImplementedException();
		}

		public void Load()
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
		{
			throw new System.NotImplementedException();
		}
	}
}
