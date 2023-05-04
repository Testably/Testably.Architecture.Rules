using FluentAssertions;
using System;
using Xunit;

namespace Testably.Architecture.Testing.Tests.Filters;

public sealed partial class FilterOnTypeExtensionsTests
{
	[Fact]
	public void And_WithContraryConditions_ShouldReturnEmptyArray()
	{
		IFilterResult<Type> result = Expect.That.AllLoadedTypes()
			.OrNone()
			.WhichArePublic().And.WhichAreNotPublic();

		TestError[] errors = result.ShouldSatisfy(_ => false).Errors;
		errors.Length.Should().Be(0);
	}
}
