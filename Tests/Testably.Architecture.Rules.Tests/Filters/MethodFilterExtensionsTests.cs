using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed class MethodFilterExtensionsTests
{
	public sealed class HaveAttributeTests
	{
		[Fact]
		public void OrAttribute_ShouldReturnBothTypes()
		{
			ITestResult result = Expect.That.Types
				.Which(Have.Method.WithAttribute<FooAttribute>().OrAttribute<BarAttribute>())
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(2);
			result.Errors.Should().Contain(m => m.ToString().Contains(nameof(FooClass)));
			result.Errors.Should().Contain(m => m.ToString().Contains(nameof(BarClass)));
		}

		[AttributeUsage(AttributeTargets.Method)]
		private class BarAttribute : Attribute
		{
		}

		// ReSharper disable once ClassNeverInstantiated.Local
		private class BarClass
		{
			[Bar]
			// ReSharper disable once UnusedMember.Local
			public void BarMethod()
			{
			}
		}

		[AttributeUsage(AttributeTargets.Method)]
		private class FooAttribute : Attribute
		{
		}

		// ReSharper disable once ClassNeverInstantiated.Local
		private class FooClass
		{
			[Foo]
			// ReSharper disable once UnusedMember.Local
			public void FooMethod()
			{
			}
		}
	}
}
