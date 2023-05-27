using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class TypeFilterExtensionsTests
{
	public sealed class ArePublicTests
	{
		[Fact]
		public void WhichAreNotPublic_Nested_ShouldFilterForNotPublicTypes()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(PublicClass), typeof(PrivateClass)).And;

			ITypeFilterResult sut = source.WhichAreNotPublic();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is not public");
			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(PrivateClass).FullName);
		}

		[Fact]
		public void WhichAreNotPublic_ShouldFilterForNotPublicTypes()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(InternalUnnestedClass), typeof(PublicUnnestedClass)).And;

			ITypeFilterResult sut = source.WhichAreNotPublic();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is not public");
			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(InternalUnnestedClass).FullName);
		}

		[Fact]
		public void WhichArePublic_Nested_ShouldFilterForPublicTypes()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(PublicClass), typeof(PrivateClass)).And;

			ITypeFilterResult sut = source.WhichArePublic();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is public");
			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(PublicClass).FullName);
		}

		[Fact]
		public void WhichArePublic_ShouldFilterForPublicTypes()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(InternalUnnestedClass), typeof(PublicUnnestedClass)).And;

			ITypeFilterResult sut = source.WhichArePublic();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is public");
			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(PublicUnnestedClass).FullName);
		}

		private class PrivateClass
		{
		}

		public class PublicClass
		{
		}
	}
}

internal class InternalUnnestedClass
{
}

public class PublicUnnestedClass
{
}
