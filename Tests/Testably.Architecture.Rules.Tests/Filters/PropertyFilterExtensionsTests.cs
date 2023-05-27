using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class PropertyFilterExtensionsTests
{
	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void Which_WithExpression_ShouldConsiderPredicateResult(bool predicateResult)
	{
		PropertyInfo propertyInfo = typeof(DummyFooClass).GetProperties().First();
		IPropertyFilterResult sut = Have.Property
			.Which(_ => predicateResult);

		bool result = sut.Applies(propertyInfo);

		result.Should().Be(predicateResult);
	}

	[Theory]
	[InlineAutoData(false)]
	[InlineAutoData(true)]
	public void Which_WithName_ShouldConsiderPredicateResult(bool predicateResult, string name)
	{
		PropertyInfo propertyInfo = typeof(DummyFooClass).GetProperties().First();
		IPropertyFilterResult sut = Have.Property
			.Which(_ => predicateResult, name);

		bool result = sut.Applies(propertyInfo);

		result.Should().Be(predicateResult);
	}
}
