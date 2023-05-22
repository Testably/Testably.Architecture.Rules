using FluentAssertions;
using Testably.Architecture.Rules.Tests.Internal;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class TypeFilterExtensionsTests
{
	public sealed class AreNestedTests
	{
		[Fact]
		public void WhichAreNested_ShouldFilterForNestedTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(NestedClass), typeof(RuleTests)).And
				.WhichAreNested()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(NestedClass).FullName);
		}

		[Fact]
		public void WhichAreNotNested_ShouldFilterForNotNestedTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(NestedClass), typeof(RuleTests)).And
				.WhichAreNotNested()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(RuleTests).FullName);
		}

		private abstract class NestedClass
		{
		}
	}
}
