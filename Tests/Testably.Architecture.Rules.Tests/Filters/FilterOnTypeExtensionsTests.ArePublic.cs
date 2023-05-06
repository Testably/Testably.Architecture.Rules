using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class FilterOnTypeExtensionsTests
{
	public sealed class ArePublic
	{
		[Fact]
		public void WhichArePublic_ShouldFilterForPublicTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichArePublic()
				.ShouldAlwaysFail()
				.Check.InExecutingAssembly();

			result.Errors.Length.Should().Be(37);
		}

		private class PrivateClass
		{
		}
	}
}
