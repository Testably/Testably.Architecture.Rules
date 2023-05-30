using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class ParameterFilterExtensionsTests
{
	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void Which_WithExpression_ShouldConsiderPredicateResult(bool predicateResult)
	{
		IUnorderedParameterFilterResult sut = Parameters.Any
			.Which(_ => predicateResult);

		bool result = sut.Apply(
			typeof(DummyFooClass).GetDeclaredConstructors().First().GetParameters());

		result.Should().Be(predicateResult);
	}

	[Theory]
	[InlineAutoData(false)]
	[InlineAutoData(true)]
	public void Which_WithName_ShouldConsiderPredicateResult(bool predicateResult, string name)
	{
		IUnorderedParameterFilterResult sut = Parameters.Any
			.Which(_ => predicateResult, name);

		bool result = sut.Apply(
			typeof(DummyFooClass).GetDeclaredConstructors().First().GetParameters());

		result.Should().Be(predicateResult);
	}
}
