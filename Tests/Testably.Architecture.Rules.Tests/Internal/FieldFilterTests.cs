using FluentAssertions;
using System;
using System.Reflection;
using Testably.Architecture.Rules.Internal;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class FieldFilterTests
{
	[Fact]
	public void ToTypeFilter_ShouldRequireOnlyOneField()
	{
		FieldFilter sut = new();

		sut.Which(Filter.FromPredicate<FieldInfo>(c => c.Name == nameof(TestDummy.Dummy1)));

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
		FieldFilter sut = new();

		sut.Which(Filter.FromPredicate<FieldInfo>(_ => filter1)).And
			.Which(Filter.FromPredicate<FieldInfo>(_ => filter2));

		Filter<Type> typeFilter = sut.ToTypeFilter();
		bool result = typeFilter.Applies(typeof(TestDummy));

		result.Should().Be(expectedResult);
	}

	private class TestDummy
	{
		#pragma warning disable CS0649
		#pragma warning disable CS0414
		public int Dummy1 = 1;
		public int Dummy2 = 1;
		#pragma warning restore CS0414
		#pragma warning restore CS0649
	}
}
