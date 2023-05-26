using FluentAssertions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed class RequirementOnMethodExtensionsTests
{
	[Fact]
	public void ShouldSatisfy_Expression_ShouldContainExpressionString()
	{
		MethodInfo method = typeof(DummyClass).GetDeclaredMethods().First();
		Expression<Func<MethodInfo, bool>> expression = _ => false;
		IRule rule = Expect.That.Methods
			.WhichAre(method)
			.ShouldSatisfy(expression);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.ShouldBeViolated();
		result.Errors[0].Should().BeOfType<MethodTestError>()
			.Which.ToString().Should().Contain(expression.ToString());
	}
}
