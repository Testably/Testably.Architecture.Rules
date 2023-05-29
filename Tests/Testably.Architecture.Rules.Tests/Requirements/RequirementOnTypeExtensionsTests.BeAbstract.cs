using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	public sealed class BeAbstractTests
	{
		[Fact]
		public void ShouldBeAbstract_AbstracTEntity_ShouldNotBeViolated()
		{
			Type type = typeof(AbstracTEntity);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBeAbstract();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldBeAbstract_ConcreteType_ShouldNotBeSatisfied()
		{
			Type type = typeof(ConcreteType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBeAbstract();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be abstract");
		}

		[Fact]
		public void ShouldNotBeAbstract_AbstracTEntity_ShouldNotBeSatisfied()
		{
			Type type = typeof(AbstracTEntity);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBeAbstract();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be abstract");
		}

		[Fact]
		public void ShouldNotBeAbstract_ConcreteType_ShouldNotBeViolated()
		{
			Type type = typeof(ConcreteType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBeAbstract();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		private abstract class AbstracTEntity
		{
		}

		private class ConcreteType
		{
		}
	}
}
