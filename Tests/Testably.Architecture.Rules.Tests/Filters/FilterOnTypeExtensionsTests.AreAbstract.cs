using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class FilterOnTypeExtensionsTests
{
	public sealed class AreAbstractTests
	{
		[Fact]
		public void WhichAreAbstract_ShouldFilterForAbstractTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(AbstractClass), typeof(NotAbstractClass)).And
				.WhichAreAbstract()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(AbstractClass).FullName);
		}

		[Fact]
		public void WhichAreNotAbstract_ShouldFilterForNotAbstractTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(AbstractClass), typeof(NotAbstractClass)).And
				.WhichAreNotAbstract()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(NotAbstractClass).FullName);
		}

		private abstract class AbstractClass
		{
		}

		private class NotAbstractClass
		{
		}
	}
}
