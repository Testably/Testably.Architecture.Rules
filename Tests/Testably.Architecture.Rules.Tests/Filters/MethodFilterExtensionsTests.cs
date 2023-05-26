using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class MethodFilterExtensionsTests
{
	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void Which_WithExpression_ShouldConsiderPredicateResult(bool predicateResult)
	{
		MethodInfo methodInfo = typeof(DummyClass).GetDeclaredMethods().First();
		IMethodFilterResult sut = Have.Method
			.Which(_ => predicateResult);

		bool result = sut.Applies(methodInfo);

		result.Should().Be(predicateResult);
	}

	[Theory]
	[InlineAutoData(false)]
	[InlineAutoData(true)]
	public void Which_WithName_ShouldConsiderPredicateResult(bool predicateResult, string name)
	{
		MethodInfo methodInfo = typeof(DummyClass).GetDeclaredMethods().First();
		IMethodFilterResult sut = Have.Method
			.Which(_ => predicateResult, name);

		bool result = sut.Applies(methodInfo);

		result.Should().Be(predicateResult);
	}
}
