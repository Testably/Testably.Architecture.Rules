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
	[Theory]
	[AutoData]
	public void Apply_ShouldCheckEachFilterIndividually()
	{
		ParameterInfo[] parameters = typeof(DummyFooClass).GetConstructors()[0].GetParameters();
		IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.InOrder;

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
		ParameterInfo[] parameters = typeof(DummyFooClass).GetConstructors()[0].GetParameters();
		IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.InOrder;

		IOrderedParameterFilterResult filter = sut
			.Which(_ => filter1Result).And
			.Which(_ => filter2Result);

		bool result = filter.Apply(parameters);

		result.Should().Be(expectedResult);
	}

	[Theory]
	[InlineData(0, "1st")]
	[InlineData(1, "2nd")]
	[InlineData(2, "3rd")]
	[InlineData(3, "4th")]
	[InlineData(4, "5th")]
	public void FriendlyName_ShouldIncludeExpectedValue(int numberOfThen, string expectedValue)
	{
		IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.InOrder;

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
	[AutoData]
	public void FriendlyName_ShouldJoinAllFilters(string filter1, string filter2)
	{
		IParameterFilter<IOrderedParameterFilterResult> sut = Parameters.InOrder;

		IOrderedParameterFilterResult result = sut
			.Which(_ => false, filter1).And
			.Which(_ => true, filter2);

		result.FriendlyName().Should()
			.Contain($"{filter1} and {filter2}");
		result.ToString().Should()
			.Contain($"{filter1} and {filter2}");
	}

	#region Helpers

	private static void DummyMethod(int v1, int v2, int v3, int v4, int v5)
	{
		// Do nothing
	}

	#endregion
}
