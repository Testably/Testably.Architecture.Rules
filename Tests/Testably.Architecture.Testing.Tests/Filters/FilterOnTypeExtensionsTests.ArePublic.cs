using FluentAssertions;
using System;
using Xunit;

namespace Testably.Architecture.Testing.Tests.Filters;

public sealed partial class FilterOnTypeExtensionsTests
{
	public sealed class ArePublic
	{
		[Fact]
		public void WhichArePublic_ShouldFilterForPublicTypes()
		{
			ITypeExpectation sut = Expect.That.Type(
				typeof(PrivateClass), typeof(FilterOnTypeExtensionsTests));

			IFilterResult<Type> result = sut.WhichArePublic();

			TestError[] errors = result.ShouldSatisfy(_ => false).Errors;
			errors.Length.Should().Be(1);
		}

		private class PrivateClass
		{
		}
	}
}
