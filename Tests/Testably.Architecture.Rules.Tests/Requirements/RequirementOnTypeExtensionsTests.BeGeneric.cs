using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

// ReSharper disable UnusedTypeParameter
public sealed partial class RequirementOnTypeExtensionsTests
{
	public sealed class BeGenericTests
	{
		[Fact]
		public void ShouldBeGeneric_GenericType_ShouldNotBeViolated()
		{
			Type type = typeof(GenericType<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBeGeneric();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldBeGeneric_SpecificType_ShouldNotBeSatisfied()
		{
			Type type = typeof(SpecificType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBeGeneric();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be generic");
		}

		[Fact]
		public void ShouldNotBeGeneric_GenericType_ShouldNotBeSatisfied()
		{
			Type type = typeof(GenericType<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBeGeneric();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be generic");
		}

		[Fact]
		public void ShouldNotBeGeneric_SpecificType_ShouldNotBeViolated()
		{
			Type type = typeof(SpecificType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBeGeneric();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		private class GenericType<T>
		{
		}

		private class SpecificType
		{
		}
	}
}
