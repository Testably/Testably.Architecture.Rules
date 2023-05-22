using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class TypeFilterExtensionsTests
{
	public sealed class AreSealedTests
	{
		[Fact]
		public void WhichAreNotSealed_ShouldFilterForNotSealedTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(SealedClass), typeof(NotSealedClass)).And
				.WhichAreNotSealed()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(NotSealedClass).FullName);
		}

		[Fact]
		public void WhichAreSealed_ShouldFilterForSealedTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(SealedClass), typeof(NotSealedClass)).And
				.WhichAreSealed()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(SealedClass).FullName);
		}

		private class NotSealedClass
		{
		}

		private sealed class SealedClass
		{
		}
	}
}
