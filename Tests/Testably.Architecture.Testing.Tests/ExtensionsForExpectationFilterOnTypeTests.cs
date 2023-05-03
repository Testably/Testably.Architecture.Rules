using FluentAssertions;
using System;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed partial class ExtensionsForExpectationFilterOnTypeTests
{
	[Fact]
	public void And_WithContraryConditions_ShouldReturnEmptyArray()
	{
		IExpectationFilterResult<Type> result = Expect.That.AllLoadedTypes()
			.OrNone()
			.WhichArePublic().And.WhichAreNotPublic();

		TestError[] errors = result.ShouldSatisfy(_ => false).Errors;
		errors.Length.Should().Be(0);
	}
}
