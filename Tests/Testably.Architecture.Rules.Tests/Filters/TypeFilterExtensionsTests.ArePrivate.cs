using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class TypeFilterExtensionsTests
{
	public sealed class ArePrivateTests
	{
		[Fact]
		public void WhichAreNotPrivate_Nested_ShouldFilterForNotPrivateTypes()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(PrivateClass), typeof(PublicClass)).And;

			ITypeFilterResult sut = source.WhichAreNotPrivate();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is not private");
			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(PublicClass).FullName);
		}

		[Fact]
		public void WhichAreNotPrivate_ShouldFilterForNotPrivateTypes()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(InternalUnnestedClass), typeof(PublicUnnestedClass)).And;

			ITypeFilterResult sut = source.WhichAreNotPrivate();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is not private");
			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(PublicUnnestedClass).FullName);
		}

		[Fact]
		public void WhichArePrivate_Nested_ShouldFilterForPublicTypes()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(PrivateClass), typeof(PublicClass)).And;

			ITypeFilterResult sut = source.WhichArePrivate();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is private");
			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(PrivateClass).FullName);
		}

		[Fact]
		public void WhichArePrivate_ShouldFilterForPublicTypes()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(InternalUnnestedClass), typeof(PublicUnnestedClass)).And;

			ITypeFilterResult sut = source.WhichArePrivate();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is private");
			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(InternalUnnestedClass).FullName);
		}

		private class PrivateClass
		{
		}

		public class PublicClass
		{
		}
	}
}
