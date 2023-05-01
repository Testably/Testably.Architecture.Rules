using FluentAssertions;
using System;
using System.Linq.Expressions;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed partial class ExtensionsForITypeExpectationTests
{
	[Fact]
	public void ShouldSatisfy_Expression_ShouldContainExpressionString()
	{
		Type type = typeof(ExtensionsForITypeExpectationTests);
		IFilterableTypeExpectation sut = Expect.That.Type(type);
		Expression<Func<Type, bool>> expression = _ => false;

		ITestResult<ITypeExpectation> result = sut.ShouldSatisfy(expression);

		result.IsSatisfied.Should().BeFalse();
		result.Errors[0].Should().BeOfType<TypeTestError>()
			.Which.ToString().Should().Contain(expression.ToString());
	}
}
