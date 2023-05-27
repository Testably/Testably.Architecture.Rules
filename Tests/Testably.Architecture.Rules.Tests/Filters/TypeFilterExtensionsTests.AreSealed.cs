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
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(SealedClass), typeof(NotSealedClass)).And;

			ITypeFilterResult sut = source.WhichAreNotSealed();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is not sealed");
			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(NotSealedClass).FullName);
		}

		[Fact]
		public void WhichAreSealed_ShouldFilterForSealedTypes()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(SealedClass), typeof(NotSealedClass)).And;

			ITypeFilterResult sut = source.WhichAreSealed();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is sealed");
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
