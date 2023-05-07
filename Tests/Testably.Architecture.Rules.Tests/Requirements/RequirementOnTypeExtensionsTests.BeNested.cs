using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	public sealed class BeNestedTests
	{
		[Fact]
		public void ShouldBeNested_NestedType_ShouldBeSatisfied()
		{
			Type type = typeof(NestedType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBeNested();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldBeNested_UnnestedType_ShouldNotBeSatisfied()
		{
			Type type = typeof(RequirementOnTypeExtensionsTests);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBeNested();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be nested");
		}

		[Fact]
		public void ShouldNotBeNested_NestedType_ShouldNotBeSatisfied()
		{
			Type type = typeof(NestedType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBeNested();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be nested");
		}

		[Fact]
		public void ShouldNotBeNested_UnnestedType_ShouldBeSatisfied()
		{
			Type type = typeof(RequirementOnTypeExtensionsTests);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBeNested();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		public class NestedType
		{
		}
	}
}
