using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	public sealed class BeSealed
	{
		[Fact]
		public void ShouldBeSealed_SealedType_ShouldBeSatisfied()
		{
			Type type = typeof(SealedType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBeSealed();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldBeSealed_UnsealedType_ShouldNotBeSatisfied()
		{
			Type type = typeof(UnsealedType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBeSealed();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be sealed");
		}

		[Fact]
		public void ShouldNotBeSealed_SealedType_ShouldNotBeSatisfied()
		{
			Type type = typeof(SealedType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBeSealed();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be sealed");
		}

		[Fact]
		public void ShouldNotBeSealed_UnsealedType_ShouldBeSatisfied()
		{
			Type type = typeof(UnsealedType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBeSealed();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		private sealed class SealedType
		{
		}

		private class UnsealedType
		{
		}
	}
}
