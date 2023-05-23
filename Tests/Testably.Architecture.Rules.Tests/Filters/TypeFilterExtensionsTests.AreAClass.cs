using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class TypeFilterExtensionsTests
{
	public sealed class AreAClassTests
	{
		[Fact]
		public void WhichAreAClass_ShouldFilterForAClassTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(ClassType), typeof(InterfaceType)).And
				.WhichAreAClass()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(ClassType).FullName);
		}

		[Fact]
		public void WhichAreNotAClass_ShouldFilterForNotAClassTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(ClassType), typeof(InterfaceType)).And
				.WhichAreNotAClass()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(InterfaceType).FullName);
		}

		private class ClassType
		{
		}

		private interface InterfaceType
		{
		}
	}
}
