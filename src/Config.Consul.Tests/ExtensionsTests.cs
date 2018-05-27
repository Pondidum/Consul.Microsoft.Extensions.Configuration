using System;
using Consul;
using NSubstitute;
using Xunit;

namespace Microsoft.Extensions.Configuration.Consul.Tests
{
	public class ExtensionsTests
	{
		private readonly IConfigurationBuilder _builder;

		public ExtensionsTests()
		{
			_builder = Substitute.For<IConfigurationBuilder>();
		}

		[Fact]
		public void When_no_prefix_is_specified()
		{
			_builder.AddConsul();

			_builder.Received().Add(Arg.Is<ConsulConfigurationSource>(source => source.Prefix == string.Empty));
		}

		[Fact]
		public void When_a_prefix_is_specified()
		{
			_builder.AddConsul("wat");

			_builder.Received().Add(Arg.Is<ConsulConfigurationSource>(source => source.Prefix == "wat"));
		}

		[Fact]
		public void When_query_options_are_modified()
		{
			var datacentre = Guid.NewGuid().ToString();
			_builder.AddConsul(consul => consul.Options.Datacenter = datacentre);

			_builder.Received().Add(Arg.Is<ConsulConfigurationSource>(source => source.Options.Datacenter == datacentre));
		}

		[Fact]
		public void Query_options_can_be_replaced_entirely()
		{
			var options = new QueryOptions
			{
				Datacenter = Guid.NewGuid().ToString(),
				Consistency = ConsistencyMode.Consistent
			};

			_builder.AddConsul(consul => consul.Options = options);

			_builder.Received().Add(Arg.Is<ConsulConfigurationSource>(source => source.Options == options));
		}
	}
}
