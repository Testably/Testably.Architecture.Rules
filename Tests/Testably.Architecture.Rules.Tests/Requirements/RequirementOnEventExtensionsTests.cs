using FluentAssertions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnEventExtensionsTests
{
	[Fact]
	public void ShouldSatisfy_Expression_ShouldContainExpressionString()
	{
		EventInfo @event = typeof(DummyFooClass).GetEvents().First();
		Expression<Func<EventInfo, bool>> expression = _ => false;
		IRule rule = Expect.That.Events
			.WhichAre(@event)
			.ShouldSatisfy(expression);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.ShouldBeViolated();
		result.Errors[0].Should().BeOfType<EventTestError>()
			.Which.ToString().Should().Contain(expression.ToString());
	}
}
