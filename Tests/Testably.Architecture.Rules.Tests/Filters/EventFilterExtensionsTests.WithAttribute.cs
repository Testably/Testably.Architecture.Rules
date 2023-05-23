using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class EventFilterExtensionsTests
{
	public sealed class WithAttributeTests
	{
		[Fact]
		public void OrAttribute_ShouldReturnBothTypes()
		{
			ITestResult result = Expect.That.Types
				.Which(Have.Event.WithAttribute<FooAttribute>().OrAttribute<BarAttribute>())
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(2);
			result.Errors.Should().Contain(m => m.ToString().Contains(nameof(FooClass)));
			result.Errors.Should().Contain(m => m.ToString().Contains(nameof(BarClass)));
		}

		[AttributeUsage(AttributeTargets.Event)]
		private class BarAttribute : Attribute
		{
		}

		public delegate void Dummy();

		// ReSharper disable ClassNeverInstantiated.Local
#pragma warning disable CS8618
#pragma warning disable CS0067
		private class BarClass
		{
			[Bar]
			public event Dummy BarEvent;
		}

		[AttributeUsage(AttributeTargets.Event)]
		private class FooAttribute : Attribute
		{
		}

		private class FooClass
		{
			[Foo]
			public event Dummy FooEvent;
		}
		#pragma warning restore CS0067
		#pragma warning restore CS8618
	}
}
