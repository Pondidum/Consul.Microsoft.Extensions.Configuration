using Microsoft.Extensions.Configuration;
using NSubstitute;
using Xunit;

namespace Config.Consul.Tests
{
	public class ExtensionsTests
	{
		[Fact]
		public void When_no_prefix_is_specified()
		{
			var builder = Substitute.For<IConfigurationBuilder>();
			builder.AddConsul();

			builder.Received().Add(Arg.Is<ConsulConfigurationSource>(source => source.Prefix == string.Empty));
		}

		[Fact]
		public void When_a_prefix_is_specified()
		{
			var builder = Substitute.For<IConfigurationBuilder>();
			builder.AddConsul("wat");

			builder.Received().Add(Arg.Is<ConsulConfigurationSource>(source => source.Prefix == "wat"));
		}
	}
}
