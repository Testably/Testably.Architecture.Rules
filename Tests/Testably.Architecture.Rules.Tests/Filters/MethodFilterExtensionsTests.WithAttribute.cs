﻿using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class MethodFilterExtensionsTests
{
	public sealed class WithAttributeTests
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

		// ReSharper disable ClassNeverInstantiated.Local
		// ReSharper disable UnusedMember.Local
		private class BarClass
		{
			[Bar]
			public void BarMethod()
			{
				// Do nothing
			}
		}

		[AttributeUsage(AttributeTargets.Method)]
		private class FooAttribute : Attribute
		{
		}

		private class FooClass
		{
			[Foo]
			public void FooMethod()
			{
				// Do nothing
			}
		}
	}
}
