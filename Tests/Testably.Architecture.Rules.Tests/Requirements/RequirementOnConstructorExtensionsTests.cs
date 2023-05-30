using FluentAssertions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnConstructorExtensionsTests
{
	[Fact]
	public void ShouldSatisfy_Expression_ShouldContainExpressionString()
	{
		ConstructorInfo constructor = typeof(DummyFooClass).GetDeclaredConstructors().First();
		Expression<Func<ConstructorInfo, bool>> expression = _ => false;
		IRule rule = Expect.That.Constructors
			.WhichAre(constructor)
			.ShouldSatisfy(expression);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.ShouldBeViolated();
		result.Errors[0].Should().BeOfType<ConstructorTestError>()
			.Which.ToString().Should().Contain(expression.ToString());
	}
}
