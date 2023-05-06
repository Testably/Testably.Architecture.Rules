using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace Testably.Architecture.Rules.Tests;

public class ExemptionTests
{
	[Theory]
	[InlineAutoData(0, true)]
	[InlineAutoData(1, false)]
	public void For_MatchingType_ShouldConsiderTypeValue(
		int valueOffset,
		bool predicateResult,
		string message,
		int value)
	{
		DummyTestError error = new(message, value + valueOffset);
		Exemption sut = Exemption.For<DummyTestError>(d => d.Value == value);

		bool result = sut.Exempt(error);
		result.Should().Be(predicateResult);
	}

	[Theory]
	[AutoData]
	public void For_NotMatchingType_ShouldReturnFalse(string message)
	{
		TestError error = new(message);
		Exemption sut = Exemption.For<DummyTestError>(_ => true);

		bool result = sut.Exempt(error);
		result.Should().Be(false);
	}

	[Theory]
	[InlineAutoData(true)]
	[InlineAutoData(false)]
	public void FromPredicate_WithExpression_NameShouldContainExpression(
		bool predicateResult,
		TestError error)
	{
		Exemption sut = Exemption.FromPredicate(_ => predicateResult);

		bool result = sut.Exempt(error);
		result.Should().Be(predicateResult);
		sut.ToString().Should().Contain(nameof(predicateResult));
	}

	[Theory]
	[InlineAutoData(true)]
	[InlineAutoData(false)]
	public void FromPredicate_WithName_Exempt_ShouldReturnPredicateResult(
		bool predicateResult,
		string name,
		TestError error)
	{
		Exemption sut = Exemption.FromPredicate(_ => predicateResult, name);

		bool result = sut.Exempt(error);
		result.Should().Be(predicateResult);
		sut.ToString().Should().Be(name);
	}

	private class DummyTestError : TestError
	{
		public int Value { get; }

		/// <inheritdoc />
		public DummyTestError(string errorMessage, int value) : base(errorMessage)
		{
			Value = value;
		}
	}
}
