using FluentAssertions;
using System;
using System.Reflection;
using Testably.Architecture.Rules.Internal;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class PropertyFilterTests
{
	[Fact]
	public void ToTypeFilter_ShouldRequireOnlyOneProperty()
	{
		PropertyFilter sut = new();

		sut.Which(Filter.FromPredicate<PropertyInfo>(c => c.Name == nameof(TestDummy.Dummy1)));

		Filter<Type> typeFilter = sut.ToTypeFilter();
		bool result = typeFilter.Applies(typeof(TestDummy));

		result.Should().BeTrue();
	}

	[Theory]
	[InlineData(false, false, false)]
	[InlineData(false, true, false)]
	[InlineData(true, false, false)]
	[InlineData(true, true, true)]
	public void ToTypeFilter_ShouldUseAllPredicates(bool filter1, bool filter2, bool expectedResult)
	{
		PropertyFilter sut = new();

		sut.Which(Filter.FromPredicate<PropertyInfo>(_ => filter1)).And
			.Which(Filter.FromPredicate<PropertyInfo>(_ => filter2));

		Filter<Type> typeFilter = sut.ToTypeFilter();
		bool result = typeFilter.Applies(typeof(TestDummy));

		result.Should().Be(expectedResult);
	}

	// ReSharper disable once UnusedMember.Local
	private class TestDummy
	{
		public int Dummy1 { get; set; } = 1;
		public int Dummy2 { get; set; } = 1;
	}
}
