using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
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
		FieldInfo fieldInfo = typeof(DummyClass).GetFields().First();
		IFieldFilterResult sut = Have.Field
			.Which(_ => predicateResult);

		bool result = sut.Applies(fieldInfo);

		result.Should().Be(predicateResult);
	}

	[Theory]
	[InlineAutoData(false)]
	[InlineAutoData(true)]
	public void Which_WithName_ShouldConsiderPredicateResult(bool predicateResult, string name)
	{
		var fieldInfo = typeof(DummyClass).GetFields().First();
		IFieldFilterResult sut = Have.Field
			.Which(_ => predicateResult, name);

		bool result = sut.Applies(fieldInfo);

		result.Should().Be(predicateResult);
	}
}
