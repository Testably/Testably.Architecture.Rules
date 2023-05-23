using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class TypeFilterExtensionsTests
{
	public sealed class AreStaticTests
	{
		[Fact]
		public void WhichAreNotStatic_ShouldFilterForNotStaticTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(StaticClass), typeof(NotStaticClass)).And
				.WhichAreNotStatic()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(NotStaticClass).FullName);
		}

		[Fact]
		public void WhichAreStatic_ShouldFilterForStaticTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(StaticClass), typeof(NotStaticClass)).And
				.WhichAreStatic()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(StaticClass).FullName);
		}

		private class NotStaticClass
		{
		}

		private static class StaticClass
		{
		}
	}
}
