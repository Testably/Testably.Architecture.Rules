using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

// ReSharper disable UnusedParameter.Local
public sealed partial class MethodFilterExtensionsTests
{
	public sealed class ParameterTests
	{
		[Theory]
		[InlineData(nameof(TestClass.TestMethodWithoutParameters), false)]
		[InlineData(nameof(TestClass.TestMethodWithStringParameter), false)]
		[InlineData(nameof(TestClass.TestMethodWithStringAndIntParameter), true)]
		public void With_OrderedParameters_ShouldApplyParameterFilter(
			string methodName, bool expectFound)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TestClass)).And
				.Which(Have.Method.WithName(methodName).And
					.With(Parameters.InOrder.WithName("value1").Then().WithName("value2")))
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectFound);
		}

		[Theory]
		[InlineData(nameof(TestClass.TestMethodWithoutParameters), false)]
		[InlineData(nameof(TestClass.TestMethodWithStringParameter), true)]
		[InlineData(nameof(TestClass.TestMethodWithStringAndIntParameter), true)]
		public void With_UnorderedParameter_ShouldApplyParameterFilter(
			string methodName, bool expectFound)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TestClass)).And
				.Which(Have.Method.WithName(methodName).And
					.With(Parameters.Any.WithName("value1")))
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectFound);
		}

		[Theory]
		[InlineData(nameof(TestClass.TestMethodWithoutParameters), true)]
		[InlineData(nameof(TestClass.TestMethodWithStringParameter), false)]
		[InlineData(nameof(TestClass.TestMethodWithStringAndIntParameter), false)]
		public void WithoutParameter_ShouldBeFoundWhenMethodHasNoParameters(
			string methodName, bool expectFound)
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(TestClass)).And;

			ITypeFilterResult sut = source
				.Which(Have.Method.WithName(methodName).And
					.WithoutParameter());

			ITestResult result = sut.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();
			result.ShouldBeViolatedIf(expectFound);
			sut.ToString().Should().Contain("without parameter");
		}

		#pragma warning disable CA1822
		private class TestClass
		{
			public void TestMethodWithoutParameters()
			{
				// Do nothing
			}

			public void TestMethodWithStringAndIntParameter(int value1, string value2)
			{
				// Do nothing
			}

			public void TestMethodWithStringParameter(string value1)
			{
				// Do nothing
			}
		}
		#pragma warning restore CA1822
	}
}
