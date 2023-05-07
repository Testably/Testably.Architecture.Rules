using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	public sealed class BeAClassTests
	{
		[Fact]
		public void ShouldBeAClass_ClassType_ShouldNotBeViolated()
		{
			Type type = typeof(ClassType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBeAClass();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldBeAClass_EnumType_ShouldNotBeSatisfied()
		{
			Type type = typeof(EnumType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBeAClass();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be a class");
		}

		[Fact]
		public void ShouldNotBeAClass_ClassType_ShouldNotBeSatisfied()
		{
			Type type = typeof(ClassType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBeAClass();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be a class");
		}

		[Fact]
		public void ShouldNotBeAClass_EnumType_ShouldNotBeViolated()
		{
			Type type = typeof(EnumType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBeAClass();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		private class ClassType
		{
		}

		private enum EnumType
		{
		}
	}
}
