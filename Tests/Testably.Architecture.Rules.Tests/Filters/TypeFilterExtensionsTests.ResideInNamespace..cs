using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class TypeFilterExtensionsTests
{
	public sealed class ResideInNamespaceTests
	{
		[Theory]
		[InlineData("TESTABLY.architecture.RULES.tests.FILTERS", false)]
		[InlineData("Testably.Architecture.Rules.Tests.Filters", true)]
		[InlineData("T???????.Architecture.Rules.Tests.Filters", true)]
		[InlineData("*Filters", true)]
		[InlineData("Test*", true)]
		[InlineData("test*", false)]
		[InlineData("T*s", true)]
		[InlineData("*FILTERS", false)]
		public void WhichDoNotResideInNamespace_CaseSensitive_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(TypeFilterExtensionsTests)).And;

			ITypeFilterResult sut = source.WhichDoNotResideInNamespace(pattern);

			ITestResult result = sut
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain(
				$"does not reside in namespace '{pattern}'");
			result.ShouldBeViolatedIf(!expectMatch);
		}

		[Theory]
		[InlineData("TESTABLY.architecture.RULES.tests.FILTERS", true)]
		[InlineData("Testably.Architecture.Rules.Tests.Filters", true)]
		[InlineData("T???????.Architecture.Rules.Tests.Filters", true)]
		[InlineData("*Filters", true)]
		[InlineData("Test*", true)]
		[InlineData("test???", false)]
		[InlineData("T*s", true)]
		public void WhichDoNotResideInNamespace_WithIgnoreCase_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(TypeFilterExtensionsTests)).And;

			ITypeFilterResult sut = source.WhichDoNotResideInNamespace(pattern, true);

			ITestResult result = sut
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain(
				$"does not reside in namespace '{pattern}'");
			result.ShouldBeViolatedIf(!expectMatch);
		}

		[Theory]
		[InlineData("TESTABLY.architecture.RULES.tests.FILTERS", false)]
		[InlineData("Testably.Architecture.Rules.Tests.Filters", true)]
		[InlineData("T???????.Architecture.Rules.Tests.Filters", true)]
		[InlineData("*Filters", true)]
		[InlineData("Test*", true)]
		[InlineData("test*", false)]
		[InlineData("T*s", true)]
		[InlineData("*FILTERS", false)]
		public void WhichResideInNamespace_CaseSensitive_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(TypeFilterExtensionsTests)).And;

			ITypeFilterResult sut = source.WhichResideInNamespace(pattern);

			ITestResult result = sut
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain(
				$"resides in namespace '{pattern}'");
			result.ShouldBeViolatedIf(expectMatch);
		}

		[Theory]
		[InlineData("TESTABLY.architecture.RULES.tests.FILTERS", true)]
		[InlineData("Testably.Architecture.Rules.Tests.Filters", true)]
		[InlineData("T???????.Architecture.Rules.Tests.Filters", true)]
		[InlineData("*Filters", true)]
		[InlineData("Test*", true)]
		[InlineData("test???", false)]
		[InlineData("T*s", true)]
		public void WhichResideInNamespace_WithIgnoreCase_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(TypeFilterExtensionsTests)).And;

			ITypeFilterResult sut = source.WhichResideInNamespace(pattern, true);

			ITestResult result = sut
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain(
				$"resides in namespace '{pattern}'");
			result.ShouldBeViolatedIf(expectMatch);
		}
	}
}
