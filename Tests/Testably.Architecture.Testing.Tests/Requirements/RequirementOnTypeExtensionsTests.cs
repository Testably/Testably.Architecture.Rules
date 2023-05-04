using FluentAssertions;
using System;
using System.Linq.Expressions;
using Xunit;

namespace Testably.Architecture.Testing.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	[Fact]
	public void ShouldSatisfy_Expression_ShouldContainExpressionString()
	{
		Type type = typeof(RequirementOnTypeExtensionsTests);
		ITypeExpectation sut = Expect.That.Type(type);
		Expression<Func<Type, bool>> expression = _ => false;

		IRequirementResult<Type> result = sut.ShouldSatisfy(expression);

		result.IsSatisfied.Should().BeFalse();
		result.Errors[0].Should().BeOfType<TypeTestError>()
			.Which.ToString().Should().Contain(expression.ToString());
	}
}
