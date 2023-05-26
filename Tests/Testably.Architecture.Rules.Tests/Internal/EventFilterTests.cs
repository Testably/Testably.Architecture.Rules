using FluentAssertions;
using System;
using System.Reflection;
using Testably.Architecture.Rules.Internal;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class EventFilterTests
{
	[Fact]
	public void ToTypeFilter_ShouldRequireOnlyOneEvent()
	{
		EventFilter sut = new();

		sut.Which(Filter.FromPredicate<EventInfo>(c => c.Name == nameof(TestDummy.Dummy1)));

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
		EventFilter sut = new();

		sut.Which(Filter.FromPredicate<EventInfo>(_ => filter1)).And
			.Which(Filter.FromPredicate<EventInfo>(_ => filter2));

		Filter<Type> typeFilter = sut.ToTypeFilter();
		bool result = typeFilter.Applies(typeof(TestDummy));

		result.Should().Be(expectedResult);
	} // ReSharper disable EventNeverSubscribedTo.Local
	private class TestDummy
	{
		public delegate void Dummy();

		#pragma warning disable CS8618
		#pragma warning disable CS0067
		public event Dummy Dummy1;
		public event Dummy Dummy2;
		#pragma warning restore CS0067
		#pragma warning restore CS8618
	}
}
