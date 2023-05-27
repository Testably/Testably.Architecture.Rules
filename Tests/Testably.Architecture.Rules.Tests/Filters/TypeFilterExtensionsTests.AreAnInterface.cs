using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class TypeFilterExtensionsTests
{
	public sealed class AreAnInterfaceTests
	{
		[Fact]
		public void WhichAreAnInterface_ShouldFilterForAnInterfaceTypes()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(InterfaceType), typeof(ClassType)).And;

			ITypeFilterResult sut = source.WhichAreAnInterface();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is an interface");
			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(InterfaceType).FullName);
		}

		[Fact]
		public void WhichAreNotAnInterface_ShouldFilterForNotAnInterfaceTypes()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(InterfaceType), typeof(ClassType)).And;

			ITypeFilterResult sut = source.WhichAreNotAnInterface();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is no interface");
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
