using FluentAssertions;
using System;
using System.Linq.Expressions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	[Fact]
	public void ShouldSatisfy_Expression_ShouldContainExpressionString()
	{
		Type type = typeof(RequirementOnTypeExtensionsTests);
		Expression<Func<Type, bool>> expression = _ => false;
		IRule rule = Expect.That.Types
			.WhichAre(type)
			.ShouldSatisfy(expression);

		ITestResult result = rule.Check
			.InAllLoadedAssemblies();

		result.ShouldBeViolated();
		result.Errors[0].Should().BeOfType<TypeTestError>()
			.Which.ToString().Should().Contain(expression.ToString());
	}
}

internal class UnnestedPrivateType
{
}

public class UnnestedPublicType
{
}
