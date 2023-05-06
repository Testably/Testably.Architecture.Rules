using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	public sealed class BeStatic
	{
		[Fact]
		public void ShouldBeStatic_InstanceType_ShouldNotBeSatisfied()
		{
			Type type = typeof(InstanceType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBeStatic();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be static");
		}

		[Fact]
		public void ShouldBeStatic_InterfaceType_ShouldNotBeSatisfied()
		{
			Type type = typeof(IInterfaceType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBeStatic();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be static");
		}

		[Fact]
		public void ShouldBeStatic_StaticType_ShouldBeSatisfied()
		{
			Type type = typeof(StaticType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBeStatic();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldNotBeStatic_InstanceType_ShouldBeSatisfied()
		{
			Type type = typeof(InstanceType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBeStatic();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldNotBeStatic_InterfaceType_ShouldBeSatisfied()
		{
			Type type = typeof(IInterfaceType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBeStatic();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldNotBeStatic_StaticType_ShouldNotBeSatisfied()
		{
			Type type = typeof(StaticType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBeStatic();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be static");
		}

		private interface IInterfaceType
		{
		}

		private abstract class InstanceType
		{
		}

		private static class StaticType
		{
		}
	}
}
