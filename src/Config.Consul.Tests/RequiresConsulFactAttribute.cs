using System;
using Consul;
using Xunit;

namespace Config.Consul.Tests
{
	public class RequiresConsulFactAttribute : FactAttribute
	{
		private static readonly Lazy<bool> IsAvailable = new Lazy<bool>(() =>
		{
			using (var client = new ConsulClient())
			{
				try
				{
					client.KV.List("").Wait();
					return true;
				}
				catch (Exception)
				{
					return false;
				}
			}
		});

		public override string Skip
		{
			get => IsAvailable.Value ? string.Empty : "Consul not detected";
			set { }
		}
	}
}
