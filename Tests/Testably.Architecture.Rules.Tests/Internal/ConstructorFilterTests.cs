﻿using FluentAssertions;
using System;
using System.Reflection;
using Testably.Architecture.Rules.Internal;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class ConstructorFilterTests
{
	[Fact]
	public void ToTypeFilter_ShouldRequireOnlyOneConstructor()
	{
		ConstructorFilter sut = new();

		sut.Which(Filter.FromPredicate<ConstructorInfo>(c => c.GetParameters().Length == 1));

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
		ConstructorFilter sut = new();

		sut.Which(Filter.FromPredicate<ConstructorInfo>(_ => filter1)).And
			.Which(Filter.FromPredicate<ConstructorInfo>(_ => filter2));

		Filter<Type> typeFilter = sut.ToTypeFilter();
		bool result = typeFilter.Applies(typeof(TestDummy));

		result.Should().Be(expectedResult);
	} // ReSharper disable UnusedParameter.Local
	// ReSharper disable UnusedMember.Local
	private class TestDummy
	{
		public TestDummy(int value1, int value2)
		{
		}

		public TestDummy(int value)
		{
		}
	}
}
