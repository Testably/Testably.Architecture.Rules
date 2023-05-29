using FluentAssertions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnPropertyExtensionsTests
{
	[Fact]
	public void ShouldSatisfy_Expression_ShouldContainExpressionString()
	{
		PropertyInfo property = typeof(DummyFooClass).GetProperties().First();
		Expression<Func<PropertyInfo, bool>> expression = _ => false;
		IRule rule = Expect.That.Properties
			.WhichAre(property)
			.ShouldSatisfy(expression);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.ShouldBeViolated();
		result.Errors[0].Should().BeOfType<PropertyTestError>()
			.Which.ToString().Should().Contain(expression.ToString());
	}
}
