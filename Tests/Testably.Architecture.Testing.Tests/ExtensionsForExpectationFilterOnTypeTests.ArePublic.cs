using FluentAssertions;
using System;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed partial class ExtensionsForExpectationFilterOnTypeTests
{
	public sealed class ArePublic
	{
		[Fact]
		public void WhichArePublic_()
		{
			ITypeExpectation sut = Expect.That.Type(
				typeof(PrivateClass), typeof(ExtensionsForExpectationFilterOnTypeTests));

			IExpectationFilterResult<Type> result = sut.WhichArePublic();

			TestError[] errors = result.ShouldSatisfy(_ => false).Errors;
			errors.Length.Should().Be(1);
		}

		private class PrivateClass
		{
		}
	}
}
