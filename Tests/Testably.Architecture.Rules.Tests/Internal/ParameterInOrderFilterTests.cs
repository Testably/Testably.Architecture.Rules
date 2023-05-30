using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
public sealed class ParameterInOrderFilterTests
{
	[Fact]
	public void Apply_First_Then_ShouldIncrementIndex()
	{
		ParameterInfo[] parameters = typeof(ParameterInOrderFilterTests)
			.GetMethod(nameof(DummyMethod))!
			.GetParameters();
		IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.First;

		IOrderedParameterFilterResult filter = sut
			.Which(p => p == parameters[0]).Then()
			.Which(p => p == parameters[1]).Then()
			.Which(p => p == parameters[2]).Then()
			.Which(p => p == parameters[3]).Then()
			.Which(p => p == parameters[4]);

		bool result = filter.Apply(parameters);

		result.Should().BeTrue();
	}

	[Fact]
	public void Apply_Last_Then_ShouldDecrementIndex()
	{
		ParameterInfo[] parameters = typeof(ParameterInOrderFilterTests)
			.GetMethod(nameof(DummyMethod))!
			.GetParameters();
		IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.Last;

		IOrderedParameterFilterResult filter = sut
			.Which(p => p == parameters[4]).Then()
			.Which(p => p == parameters[3]).Then()
			.Which(p => p == parameters[2]).Then()
			.Which(p => p == parameters[1]).Then()
			.Which(p => p == parameters[0]);

		bool result = filter.Apply(parameters);

		result.Should().BeTrue();
	}

	[Fact]
	public void Apply_ShouldCheckEachFilterIndividually()
	{
		ParameterInfo[] parameters = typeof(DummyFooClass).GetDeclaredConstructors()[0].GetParameters();
		IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.First;

		IOrderedParameterFilterResult filter = sut
			.Which(p => p == parameters.First()).And
			.Which(p => p == parameters.Last());

		bool result = filter.Apply(parameters);

		result.Should().BeTrue();
	}

	[Theory]
	[InlineData(true, true, true)]
	[InlineData(true, false, false)]
	[InlineData(false, true, false)]
	[InlineData(false, false, false)]
	public void Apply_ShouldRequireAllFilters(
		bool filter1Result, bool filter2Result, bool expectedResult)
	{
		ParameterInfo[] parameters = typeof(DummyFooClass).GetDeclaredConstructors()[0].GetParameters();
		IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.First;

		IOrderedParameterFilterResult filter = sut
			.Which(_ => filter1Result).And
			.Which(_ => filter2Result);

		bool result = filter.Apply(parameters);

		result.Should().Be(expectedResult);
	}

	[Theory]
	[InlineData(0, "1st parameter")]
	[InlineData(1, "2nd parameter")]
	[InlineData(2, "3rd parameter")]
	[InlineData(3, "4th parameter")]
	[InlineData(4, "5th parameter")]
	public void FriendlyName_First_ShouldIncludeExpectedValue(int numberOfThen,
		string expectedValue)
	{
		IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.First;

		for (int i = 0; i < numberOfThen; i++)
		{
			sut = sut.Which(_ => true).Then();
		}

		IOrderedParameterFilterResult result = sut
			.Which(_ => true);

		result.FriendlyName().Should()
			.Contain(expectedValue);
	}

	[Theory]
	[InlineData(1, "2nd to last parameter")]
	[InlineData(2, "3rd to last parameter")]
	[InlineData(3, "4th to last parameter")]
	[InlineData(4, "5th to last parameter")]
	public void FriendlyName_Last_ShouldIncludeExpectedValue(int numberOfThen, string expectedValue)
	{
		IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.Last;

		for (int i = 0; i < numberOfThen; i++)
		{
			sut = sut.Which(_ => true).Then();
		}

		IOrderedParameterFilterResult result = sut
			.Which(_ => true);

		result.FriendlyName().Should()
			.Contain(expectedValue);
	}

	[Fact]
	public void FriendlyName_OnlyLast_ShouldStartWithLastParameter()
	{
		IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.Last;

		IOrderedParameterFilterResult result = sut
			.Which(_ => true);

		result.FriendlyName().Should()
			.StartWith("last parameter");
	}

	[Theory]
	[AutoData]
	public void FriendlyName_ShouldJoinAllFilters(string filter1, string filter2)
	{
		IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.First;

		IOrderedParameterFilterResult result = sut
			.Which(_ => false, filter1).And
			.Which(_ => true, filter2);

		result.FriendlyName().Should()
			.Contain($"{filter1} and {filter2}");
		result.ToString().Should()
			.Contain($"{filter1} and {filter2}");
	}

	#region Helpers

	public void DummyMethod(int v1, int v2, int v3, int v4, int v5)
	{
		// Do nothing
	}

	#endregion
}
