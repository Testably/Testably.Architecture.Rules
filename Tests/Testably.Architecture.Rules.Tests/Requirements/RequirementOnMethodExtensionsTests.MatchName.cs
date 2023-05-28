using FluentAssertions;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnMethodExtensionsTests
{
	public sealed class MatchNameTests
	{
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldMatchName_IgnoreCase_ShouldConsiderParameter(bool ignoreCase)
		{
			MethodInfo method =
				typeof(DummyFooClass).GetMethod(nameof(DummyFooClass.DummyFooMethod1))!;
			IRule rule = Expect.That.Methods
				.WhichAre(method)
				.ShouldMatchName("DUMMYfooMETHOD1", ignoreCase);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(!ignoreCase);
		}

		[Theory]
		[InlineData("*")]
		[InlineData("?????FooMethod1")]
		[InlineData("DummyFooMethod1")]
		public void ShouldMatchName_MatchingPattern_ShouldNotBeViolated(string matchingPattern)
		{
			MethodInfo method =
				typeof(DummyFooClass).GetMethod(nameof(DummyFooClass.DummyFooMethod1))!;
			IRule rule = Expect.That.Methods
				.WhichAre(method)
				.ShouldMatchName(matchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Theory]
		[InlineData("*Foo")]
		[InlineData("??FooMethod1")]
		[InlineData("dummyfoomethod1")]
		public void ShouldMatchName_NotMatchingPattern_ShouldNotBeSatisfied(
			string notMatchingPattern)
		{
			MethodInfo method =
				typeof(DummyFooClass).GetMethod(nameof(DummyFooClass.DummyFooMethod1))!;
			IRule rule = Expect.That.Methods
				.WhichAre(method)
				.ShouldMatchName(notMatchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<MethodTestError>()
				.Which.Method.Should().BeSameAs(method);
			result.Errors[0].Should().BeOfType<MethodTestError>()
				.Which.ToString().Should()
				.Contain(method.Name).And.Contain($"'{notMatchingPattern}'");
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldNotMatchName_IgnoreCase_ShouldConsiderParameter(bool ignoreCase)
		{
			MethodInfo method =
				typeof(DummyFooClass).GetMethod(nameof(DummyFooClass.DummyFooMethod1))!;
			IRule rule = Expect.That.Methods
				.WhichAre(method)
				.ShouldNotMatchName("DUMMYfooMETHOD1", ignoreCase);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(ignoreCase);
		}

		[Theory]
		[InlineData("*")]
		[InlineData("?????FooMethod1")]
		[InlineData("DummyFooMethod1")]
		public void ShouldNotMatchName_MatchingPattern_ShouldNotBeSatisfied(string matchingPattern)
		{
			MethodInfo method =
				typeof(DummyFooClass).GetMethod(nameof(DummyFooClass.DummyFooMethod1))!;
			IRule rule = Expect.That.Methods
				.WhichAre(method)
				.ShouldNotMatchName(matchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<MethodTestError>()
				.Which.Method.Should().BeSameAs(method);
			result.Errors[0].Should().BeOfType<MethodTestError>()
				.Which.ToString().Should()
				.Contain(method.Name).And.Contain($"'{matchingPattern}'");
		}

		[Theory]
		[InlineData("*Foo")]
		[InlineData("??FooMethod1")]
		[InlineData("dummyfoomethod1")]
		public void ShouldNotMatchName_NotMatchingPattern_ShouldNotBeViolated(
			string notMatchingPattern)
		{
			MethodInfo method =
				typeof(DummyFooClass).GetMethod(nameof(DummyFooClass.DummyFooMethod1))!;
			IRule rule = Expect.That.Methods
				.WhichAre(method)
				.ShouldNotMatchName(notMatchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}
	}
}
