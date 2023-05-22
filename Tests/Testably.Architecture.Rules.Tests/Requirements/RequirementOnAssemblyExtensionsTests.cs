using FluentAssertions;
using System;
using System.Linq.Expressions;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnAssemblyExtensionsTests
{
	[Fact]
	public void ShouldSatisfy_Expression_ShouldContainExpressionString()
	{
		Expression<Func<Assembly, bool>> expression = _ => false;
		IRule rule = Expect.That.Assemblies
			.ShouldSatisfy(expression);

		ITestResult result = rule.Check
			.InExecutingAssembly();

		result.ShouldBeViolated();
		result.Errors[0].Should().BeOfType<AssemblyTestError>()
			.Which.ToString().Should().Contain(expression.ToString());
	}
}
