﻿using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace Testably.Architecture.Rules.Tests.Filters;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
public sealed partial class ConstructorFilterExtensionsTests
{
	public sealed class ParameterTests
	{
		#region Test Setup

		private readonly ITestOutputHelper _testOutputHelper;

		public ParameterTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}

		#endregion

		[Theory]
		[InlineData(0, false)]
		[InlineData(1, false)]
		[InlineData(2, true)]
		public void With_OrderedParameters_ShouldApplyParameterFilter(
			int parameterCount, bool expectFound)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TestClass)).And
				.Which(Have.Constructor
					.WithAttribute<ParameterCountAttribute>(p => p.Count == parameterCount).And
					.With(Parameters.First.WithName("value1").Then().WithName("value2")))
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectFound);
		}

		[Theory]
		[InlineData(0, false)]
		[InlineData(1, true)]
		[InlineData(2, true)]
		public void With_UnorderedParameter_ShouldApplyParameterFilter(
			int parameterCount, bool expectFound)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TestClass)).And
				.Which(Have.Constructor
					.WithAttribute<ParameterCountAttribute>(p => p.Count == parameterCount).And
					.With(Parameters.Any.WithName("value1")))
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectFound);
		}

		[Theory]
		[InlineData(0, true)]
		[InlineData(1, false)]
		[InlineData(2, false)]
		public void WithoutParameter_ShouldBeFoundWhenConstructorHasNoParameters(
			int parameterCount, bool expectFound)
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(TestClass)).And;

			ITypeFilterResult sut = source.Which(Have.Constructor
				.WithAttribute<ParameterCountAttribute>(p => p.Count == parameterCount).And
				.WithoutParameter());

			ITestResult result = sut.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.WithLog(_testOutputHelper.WriteLine).InAllLoadedAssemblies();
			result.ShouldBeViolatedIf(expectFound);
			sut.ToString().Should().Contain("without parameter");
		}

		[Theory]
		[InlineData(0, false)]
		[InlineData(1, true)]
		[InlineData(2, true)]
		public void WithParameter_ShouldBeFoundWhenConstructorHasAtLeastOneParameter(
			int parameterCount, bool expectFound)
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(TestClass)).And;

			ITypeFilterResult sut = source.Which(Have.Constructor
				.WithAttribute<ParameterCountAttribute>(p => p.Count == parameterCount).And
				.WithParameters());

			ITestResult result = sut.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();
			result.ShouldBeViolatedIf(expectFound);
			sut.ToString().Should()
				.Contain("with at least 1 parameter").And
				.NotContain("parameters");
		}

		[Theory]
		[InlineData(0, 2, false)]
		[InlineData(1, 2, false)]
		[InlineData(2, 2, true)]
		[InlineData(2, 3, false)]
		public void
			WithParameter_WithMinimumCount_ShouldBeFoundWhenConstructorHasAtLeastTheRequiredParameters(
				int parameterCount, int minimumCount, bool expectFound)
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(TestClass)).And;

			ITypeFilterResult sut = source.Which(Have.Constructor
				.WithAttribute<ParameterCountAttribute>(p => p.Count == parameterCount).And
				.WithParameters(minimumCount));

			ITestResult result = sut.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();
			result.ShouldBeViolatedIf(expectFound);
			sut.ToString().Should()
				.Contain($"with at least {minimumCount} parameters");
		}

		private class TestClass
		{
			[ParameterCount(0)]
			public TestClass()
			{
			}

			[ParameterCount(1)]
			public TestClass(string value1)
			{
			}

			[ParameterCount(2)]
			public TestClass(string value1, int value2)
			{
			}
		}
	}
}
