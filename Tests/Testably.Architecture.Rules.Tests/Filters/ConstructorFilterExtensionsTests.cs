using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class ConstructorFilterExtensionsTests
{
	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void Which_WithExpression_ShouldConsiderPredicateResult(bool predicateResult)
	{
		ConstructorInfo constructorInfo = typeof(DummyFooClass).GetDeclaredConstructors().First();
		IConstructorFilterResult sut = Have.Constructor
			.Which(_ => predicateResult);

		bool result = sut.Applies(constructorInfo);

		result.Should().Be(predicateResult);
	}

	[Theory]
	[InlineAutoData(false)]
	[InlineAutoData(true)]
	public void Which_WithName_ShouldConsiderPredicateResult(bool predicateResult, string name)
	{
		ConstructorInfo constructorInfo = typeof(DummyFooClass).GetDeclaredConstructors().First();
		IConstructorFilterResult sut = Have.Constructor
			.Which(_ => predicateResult, name);

		bool result = sut.Applies(constructorInfo);

		result.Should().Be(predicateResult);
	}
}
