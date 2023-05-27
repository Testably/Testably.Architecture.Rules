using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class ParameterAnyFilterTests
{
	[Theory]
	[AutoData]
	public void Apply_ShouldCheckEachFilterIndividually()
	{
		ParameterInfo[] parameters = typeof(DummyFooClass).GetConstructors()[0].GetParameters();
		IParameterFilter<IUnorderedParameterFilterResult> sut = Parameters.Any;

		IUnorderedParameterFilterResult filter = sut
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
		IParameterFilter<IUnorderedParameterFilterResult> sut = Parameters.Any;

		IUnorderedParameterFilterResult filter = sut
			.Which(_ => filter1Result).And
			.Which(_ => filter2Result);

		bool result = filter.Apply(parameters);

		result.Should().Be(expectedResult);
	}

	[Theory]
	[AutoData]
	public void FriendlyName_ShouldJoinAllFilters(string filter1, string filter2)
	{
		IParameterFilter<IUnorderedParameterFilterResult> sut = Parameters.Any;

		IUnorderedParameterFilterResult result = sut
			.Which(_ => false, filter1).And
			.Which(_ => true, filter2);

		result.FriendlyName().Should()
			.Be($"{filter1} and {filter2}");
		result.ToString().Should()
			.Be($"{filter1} and {filter2}");
	}
}
