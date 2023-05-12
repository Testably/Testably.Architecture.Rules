using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class FilterOnTypeExtensionsTests
{
	public sealed class AreAnInterfaceTests
	{
		[Fact]
		public void WhichAreAnInterface_ShouldFilterForAnInterfaceTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(InterfaceType), typeof(ClassType)).And
				.WhichAreAnInterface()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(InterfaceType).FullName);
		}

		[Fact]
		public void WhichAreNotAnInterface_ShouldFilterForNotAnInterfaceTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(InterfaceType), typeof(ClassType)).And
				.WhichAreNotAnInterface()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(ClassType).FullName);
		}

		private class ClassType
		{
		}

		private interface InterfaceType
		{
		}
	}
}
