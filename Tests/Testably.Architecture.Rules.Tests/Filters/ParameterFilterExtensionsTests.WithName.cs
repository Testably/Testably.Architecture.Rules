using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class ParameterFilterExtensionsTests
{
	public sealed class WithNameTests
	{
		[Theory]
		[InlineData("tESTParameter", false)]
		[InlineData("testParameter", true)]
		[InlineData("t???Parameter", true)]
		[InlineData("*Parameter", true)]
		[InlineData("test*", true)]
		[InlineData("TEST*", false)]
		[InlineData("t*r", true)]
		[InlineData("*parameter", false)]
		public void WhichMatchName_CaseSensitive_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(TestClass)).And;

			ITypeFilterResult sut = source
				.Which(Have.Method.With(Parameters.Any.WithName(pattern)));

			ITestResult result = sut
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectMatch);
			sut.ToString().Should()
				.Contain($"name matches '{pattern}'");
		}

		[Theory]
		[InlineData("TESTParameter", true)]
		[InlineData("TestParameter", true)]
		[InlineData("t???parameter", true)]
		[InlineData("*parameter", true)]
		[InlineData("test*", true)]
		[InlineData("test???", false)]
		[InlineData("t*r", true)]
		public void WhichMatchName_WithIgnoreCase_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(TestClass)).And;

			ITypeFilterResult sut = source
				.Which(Have.Method.With(Parameters.Any.WithName(pattern, true)));

			ITestResult result = sut
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectMatch);
			sut.ToString().Should()
				.Contain($"name matches '{pattern}'");
		} // ReSharper disable UnusedMember.Local
		// ReSharper disable UnusedParameter.Local
		private class TestClass
		{
			public void DummyMethod(string testParameter)
			{
				// Do nothing
			}
		}
	}
}
