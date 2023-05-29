using FluentAssertions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnFieldExtensionsTests
{
	[Fact]
	public void ShouldSatisfy_Expression_ShouldContainExpressionString()
	{
		FieldInfo field = typeof(DummyFooClass).GetFields().First();
		Expression<Func<FieldInfo, bool>> expression = _ => false;
		IRule rule = Expect.That.Fields
			.WhichAre(field)
			.ShouldSatisfy(expression);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.ShouldBeViolated();
		result.Errors[0].Should().BeOfType<FieldTestError>()
			.Which.ToString().Should().Contain(expression.ToString());
	}
}
