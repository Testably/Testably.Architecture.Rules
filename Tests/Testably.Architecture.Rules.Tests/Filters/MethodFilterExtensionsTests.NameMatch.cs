﻿using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

// ReSharper disable UnusedMember.Local
public sealed partial class MethodFilterExtensionsTests
{
	public sealed class NameMatchTests
	{
		[Theory]
		[InlineData("TESTMethod", false)]
		[InlineData("TestMethod", true)]
		[InlineData("T???Method", true)]
		[InlineData("*Method", true)]
		[InlineData("Test*", true)]
		[InlineData("test*", false)]
		[InlineData("T*d", true)]
		[InlineData("*method", false)]
		public void WhichNameMatches_CaseSensitive_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TestClass)).And
				.Which(Have.Method.WhichNameMatches(pattern))
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectMatch);
		}

		[Theory]
		[InlineData("TESTMethod", true)]
		[InlineData("TestMethod", true)]
		[InlineData("t???method", true)]
		[InlineData("*method", true)]
		[InlineData("test*", true)]
		[InlineData("test???", false)]
		[InlineData("t*d", true)]
		public void WhichNameMatches_WithIgnoreCase_ShouldReturnExpectedValue(
			string pattern, bool expectMatch)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(TestClass)).And
				.Which(Have.Method.WhichNameMatches(pattern, true))
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectMatch);
		}

		#pragma warning disable CA1822
		private class TestClass
		{
			public void TestMethod()
			{
				// Do nothing
			}
		}
		#pragma warning restore CA1822
	}
}
