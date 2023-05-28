using FluentAssertions;
using System;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

// ReSharper disable UnusedParameter.Local
public sealed partial class RequirementOnMethodExtensionsTests
{
	public sealed class HaveParameterTests
	{
		[Theory]
		[InlineData(nameof(TestClass.TestMethodWithoutParameters), false)]
		[InlineData(nameof(TestClass.TestMethodWithStringParameter), true)]
		[InlineData(nameof(TestClass.TestMethodWithStringAndIntParameter), true)]
		public void ShouldHaveNoParameters_WithMethodName_ShouldResultInExpectViolation(
			string methodName, bool expectViolation)
		{
			MethodInfo method = typeof(TestClass)
				.GetMethod(methodName)!;
			IRule rule = Expect.That.Methods
				.WhichAre(method)
				.ShouldHaveNoParameters();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectViolation);
			if (expectViolation)
			{
				result.Errors[0].ToString().Should()
					.Contain($"'{method.Name}'").And
					.Contain("should have no parameters");
			}
		}

		[Theory]
		[InlineData(nameof(TestClass.TestMethodWithoutParameters), true)]
		[InlineData(nameof(TestClass.TestMethodWithStringParameter), false)]
		[InlineData(nameof(TestClass.TestMethodWithStringAndIntParameter), false)]
		public void ShouldHaveParameters_WithMethodName_ShouldResultInExpectViolation(
			string methodName, bool expectViolation)
		{
			MethodInfo method = typeof(TestClass)
				.GetMethod(methodName)!;
			IRule rule = Expect.That.Methods
				.WhichAre(method)
				.ShouldHaveParameters();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectViolation);
			if (expectViolation)
			{
				result.Errors[0].ToString().Should()
					.Contain($"'{method.Name}'").And
					.Contain("should have at least 1 parameter").And
					.NotContain("parameters");
			}
		}

		[Theory]
		[InlineData(nameof(TestClass.TestMethodWithoutParameters), 2, true)]
		[InlineData(nameof(TestClass.TestMethodWithStringParameter), 2, true)]
		[InlineData(nameof(TestClass.TestMethodWithStringAndIntParameter), 2, false)]
		[InlineData(nameof(TestClass.TestMethodWithStringAndIntParameter), 3, true)]
		public void ShouldHaveParameters_WithMinimumCount_ShouldResultInExpectViolation(
			string methodName, int minimumCount, bool expectViolation)
		{
			MethodInfo method = typeof(TestClass)
				.GetMethod(methodName)!;
			IRule rule = Expect.That.Methods
				.WhichAre(method)
				.ShouldHaveParameters(minimumCount);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectViolation);
			if (expectViolation)
			{
				result.Errors[0].ToString().Should()
					.Contain($"'{method.Name}'").And
					.Contain($"should have at least {minimumCount} parameters");
			}
		}

		[Theory]
		[InlineData(nameof(TestClass.TestMethodWithoutParameters), true)]
		[InlineData(nameof(TestClass.TestMethodWithStringParameter), true)]
		[InlineData(nameof(TestClass.TestMethodWithStringAndIntParameter), false)]
		public void ShouldHave_WithAnyIntParameter_ShouldResultInExpectViolation(
			string methodName, bool expectViolation)
		{
			MethodInfo method = typeof(TestClass)
				.GetMethod(methodName)!;
			IRule rule = Expect.That.Methods
				.WhichAre(method)
				.ShouldHave(Parameters.Any.OfType<int>());

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectViolation);
			if (expectViolation)
			{
				result.Errors[0].ToString().Should()
					.Contain($"'{method.Name}'").And
					.Contain($"should have a parameter which is of type {nameof(Int32)}");
			}
		}

		[Theory]
		[InlineData(nameof(TestClass.TestMethodWithoutParameters), true)]
		[InlineData(nameof(TestClass.TestMethodWithStringParameter), true)]
		[InlineData(nameof(TestClass.TestMethodWithStringAndIntParameter), false)]
		public void ShouldHave_WithOrderedStringAndInt_ShouldResultInExpectViolation(
			string methodName, bool expectViolation)
		{
			MethodInfo method = typeof(TestClass)
				.GetMethod(methodName)!;
			IRule rule = Expect.That.Methods
				.WhichAre(method)
				.ShouldHave(Parameters.InOrder
					.OfType<string>().Then()
					.OfType<int>());

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectViolation);
			if (expectViolation)
			{
				result.Errors[0].ToString().Should()
					.Contain($"'{method.Name}'").And
					.Contain($"should have parameters whose 1st parameter is of type {nameof(String)} and 2nd parameter is of type {nameof(Int32)}");
			}
		}

#pragma warning disable CA1822
		private class TestClass
		{
			public void TestMethodWithoutParameters()
			{
				// Do nothing
			}

			public void TestMethodWithStringAndIntParameter(string value1, int value2)
			{
				// Do nothing
			}

			public void TestMethodWithStringParameter(string value1)
			{
				// Do nothing
			}
		}
		#pragma warning restore CA1822
	}
}
