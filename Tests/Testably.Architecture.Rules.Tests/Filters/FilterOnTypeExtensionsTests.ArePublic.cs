using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class FilterOnTypeExtensionsTests
{
	public sealed class ArePublicTests
	{
		[Fact]
		public void WhichAreNotPublic_Nested_ShouldFilterForNotPublicTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(PublicClass), typeof(PrivateClass)).And
				.WhichAreNotPublic()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(PrivateClass).FullName);
		}

		[Fact]
		public void WhichAreNotPublic_ShouldFilterForNotPublicTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(PublicUnnestedClass), typeof(InternalUnnestedClass)).And
				.WhichAreNotPublic()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(InternalUnnestedClass).FullName);
		}

		[Fact]
		public void WhichArePublic_Nested_ShouldFilterForPublicTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(PublicClass), typeof(PrivateClass)).And
				.WhichArePublic()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(PublicClass).FullName);
		}

		[Fact]
		public void WhichArePublic_ShouldFilterForPublicTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(PublicUnnestedClass), typeof(InternalUnnestedClass)).And
				.WhichArePublic()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

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
