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
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(PrivateClass), typeof(PublicClass)).And
				.WhichAreNotPrivate()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(PublicClass).FullName);
		}

		[Fact]
		public void WhichAreNotPrivate_ShouldFilterForNotPrivateTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(InternalUnnestedClass), typeof(PublicUnnestedClass)).And
				.WhichAreNotPrivate()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(PublicUnnestedClass).FullName);
		}

		[Fact]
		public void WhichArePrivate_Nested_ShouldFilterForPublicTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(PrivateClass), typeof(PublicClass)).And
				.WhichArePrivate()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(PrivateClass).FullName);
		}

		[Fact]
		public void WhichArePrivate_ShouldFilterForPublicTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(InternalUnnestedClass), typeof(PublicUnnestedClass)).And
				.WhichArePrivate()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

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
