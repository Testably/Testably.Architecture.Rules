using AutoFixture.Xunit2;
using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class FieldFilterExtensionsTests
{
	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void Which_WithExpression_ShouldConsiderPredicateResult(bool predicateResult)
	{
		IFieldFilterResult sut = Have.Field
			.Which(_ => predicateResult);

		bool result = sut.ToTypeFilter()
			.Applies(typeof(DummyClass));

		result.Should().Be(predicateResult);
	}

	[Theory]
	[InlineAutoData(false)]
	[InlineAutoData(true)]
	public void Which_WithName_ShouldConsiderPredicateResult(bool predicateResult, string name)
	{
		IFieldFilterResult sut = Have.Field
			.Which(_ => predicateResult, name);

		bool result = sut.ToTypeFilter()
			.Applies(typeof(DummyClass));

		result.Should().Be(predicateResult);
	}
}
