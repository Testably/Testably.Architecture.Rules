using FluentAssertions;
using System;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
public sealed partial class RequirementOnConstructorExtensionsTests
{
	public sealed class HaveParameterTests
	{
		[Theory]
		[InlineData(0, true)]
		[InlineData(1, true)]
		[InlineData(2, false)]
		public void ShouldHave_WithAnyIntParameter_ShouldResultInExpectViolation(
			int parameterCount, bool expectViolation)
		{
			ConstructorInfo constructor = typeof(TestClass).GetConstructors()
				.First(p => p.GetParameters().Length == parameterCount);
			IRule rule = Expect.That.Constructors
				.WhichAre(constructor)
				.ShouldHave(Parameters.Any.OfType<int>());

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectViolation);
			if (expectViolation)
			{
				result.Errors[0].ToString().Should()
					.Contain($"'{constructor.Name}'").And
					.Contain($"should have a parameter which is of type {nameof(Int32)}");
			}
		}

		[Theory]
		[InlineData(0, true)]
		[InlineData(1, true)]
		[InlineData(2, false)]
		public void ShouldHave_WithOrderedStringAndInt_ShouldResultInExpectViolation(
			int parameterCount, bool expectViolation)
		{
			ConstructorInfo constructor = typeof(TestClass).GetConstructors()
				.First(p => p.GetParameters().Length == parameterCount);
			IRule rule = Expect.That.Constructors
				.WhichAre(constructor)
				.ShouldHave(Parameters.InOrder
					.OfType<string>().Then()
					.OfType<int>());

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectViolation);
			if (expectViolation)
			{
				result.Errors[0].ToString().Should()
					.Contain($"'{constructor.Name}'").And
					.Contain(
						$"should have parameters whose 1st parameter is of type {nameof(String)} and 2nd parameter is of type {nameof(Int32)}");
			}
		}

		[Theory]
		[InlineData(0, false)]
		[InlineData(1, true)]
		[InlineData(2, true)]
		public void ShouldHaveNoParameters_WithConstructorName_ShouldResultInExpectViolation(
			int parameterCount, bool expectViolation)
		{
			ConstructorInfo constructor = typeof(TestClass).GetConstructors()
				.First(p => p.GetParameters().Length == parameterCount);
			IRule rule = Expect.That.Constructors
				.WhichAre(constructor)
				.ShouldHaveNoParameters();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectViolation);
			if (expectViolation)
			{
				result.Errors[0].ToString().Should()
					.Contain($"'{constructor.Name}'").And
					.Contain("should have no parameters");
			}
		}

		[Theory]
		[InlineData(0, true)]
		[InlineData(1, false)]
		[InlineData(2, false)]
		public void ShouldHaveParameters_WithConstructorName_ShouldResultInExpectViolation(
			int parameterCount, bool expectViolation)
		{
			ConstructorInfo constructor = typeof(TestClass).GetConstructors()
				.First(p => p.GetParameters().Length == parameterCount);
			IRule rule = Expect.That.Constructors
				.WhichAre(constructor)
				.ShouldHaveParameters();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectViolation);
			if (expectViolation)
			{
				result.Errors[0].ToString().Should()
					.Contain($"'{constructor.Name}'").And
					.Contain("should have at least 1 parameter").And
					.NotContain("parameters");
			}
		}

		[Theory]
		[InlineData(0, 2, true)]
		[InlineData(1, 2, true)]
		[InlineData(2, 2, false)]
		[InlineData(2, 3, true)]
		public void ShouldHaveParameters_WithMinimumCount_ShouldResultInExpectViolation(
			int parameterCount, int minimumCount, bool expectViolation)
		{
			ConstructorInfo constructor = typeof(TestClass).GetConstructors()
				.First(p => p.GetParameters().Length == parameterCount);
			IRule rule = Expect.That.Constructors
				.WhichAre(constructor)
				.ShouldHaveParameters(minimumCount);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectViolation);
			if (expectViolation)
			{
				result.Errors[0].ToString().Should()
					.Contain($"'{constructor.Name}'").And
					.Contain($"should have at least {minimumCount} parameters");
			}
		}

		private class TestClass
		{
			[ParameterCount(0)]
			public TestClass()
			{
			}

			[ParameterCount(1)]
			public TestClass(string value1)
			{
			}

			[ParameterCount(2)]
			public TestClass(string value1, int value2)
			{
			}
		}
	}
}
