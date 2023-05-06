using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	public sealed class BeAnInterface
	{
		[Fact]
		public void ShouldBeAnInterface_EnumType_ShouldNotBeSatisfied()
		{
			Type type = typeof(EnumType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBeAnInterface();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be an interface");
		}

		[Fact]
		public void ShouldBeAnInterface_InterfaceType_ShouldBeSatisfied()
		{
			Type type = typeof(InterfaceType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBeAnInterface();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldNotBeAnInterface_EnumType_ShouldBeSatisfied()
		{
			Type type = typeof(EnumType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBeAnInterface();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldNotBeAnInterface_InterfaceType_ShouldNotBeSatisfied()
		{
			Type type = typeof(InterfaceType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBeAnInterface();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be an interface");
		}

		private enum EnumType
		{
		}

		private interface InterfaceType
		{
		}
	}
}
