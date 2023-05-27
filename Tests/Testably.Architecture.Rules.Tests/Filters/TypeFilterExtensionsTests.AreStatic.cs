using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class TypeFilterExtensionsTests
{
	public sealed class AreStaticTests
	{
		[Fact]
		public void WhichAreNotStatic_ShouldFilterForNotStaticTypes()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(StaticClass), typeof(NotStaticClass)).And;

			ITypeFilterResult sut = source.WhichAreNotStatic();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is not static");
			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(NotStaticClass).FullName);
		}

		[Fact]
		public void WhichAreStatic_ShouldFilterForStaticTypes()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(StaticClass), typeof(NotStaticClass)).And;

			ITypeFilterResult sut = source.WhichAreStatic();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is static");
			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(StaticClass).FullName);
		}

		private class NotStaticClass
		{
		}

		private static class StaticClass
		{
		}
	}
}
