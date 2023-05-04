using FluentAssertions;
using System;
using System.Linq.Expressions;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed class ExtensionsForExpectationConditionOnTypeTests
{
	[Fact]
	public void ShouldSatisfy_Expression_ShouldContainExpressionString()
	{
		Type type = typeof(ExtensionsForExpectationConditionOnTypeTests);
		ITypeExpectation sut = Expect.That.Type(type);
		Expression<Func<Type, bool>> expression = _ => false;

		IExpectationConditionResult<Type> result = sut.ShouldSatisfy(expression);

		result.IsSatisfied.Should().BeFalse();
		result.Errors[0].Should().BeOfType<TypeTestError>()
			.Which.ToString().Should().Contain(expression.ToString());
	}
}
