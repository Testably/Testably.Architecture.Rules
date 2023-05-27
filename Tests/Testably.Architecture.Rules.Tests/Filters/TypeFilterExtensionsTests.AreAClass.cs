using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class TypeFilterExtensionsTests
{
	public sealed class AreAClassTests
	{
		[Fact]
		public void WhichAreAClass_ShouldFilterForAClassTypes()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(ClassType), typeof(InterfaceType)).And;

			ITypeFilterResult sut = source.WhichAreAClass();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is a class");
			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(ClassType).FullName);
		}

		[Fact]
		public void WhichAreNotAClass_ShouldFilterForNotAClassTypes()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(ClassType), typeof(InterfaceType)).And;

			ITypeFilterResult sut = source.WhichAreNotAClass();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is no class");
			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(InterfaceType).FullName);
		}

		private class ClassType
		{
		}

		private interface InterfaceType
		{
		}
	}
}
