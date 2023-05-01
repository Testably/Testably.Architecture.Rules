using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Linq;
using Testably.Architecture.Testing.Internal;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed class TypeExpectationTests
{
	[Fact]
	public void ShouldSatisfy_DefaultError_ShouldIncludeTypeName()
	{
		Type type = typeof(TypeExpectationTests);
		string expectedTypeName = $"'{type.Name}'";
		IFilterableTypeExpectation sut = Expect.That.Type(type);

		ITestResult<ITypeExpectation> result = sut.ShouldSatisfy(_ => false);

		TestError error = result.Errors.Single();
		error.ToString().Should().Contain(expectedTypeName);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_False_ShouldIncludeError(TestError error)
	{
		IFilterableTypeExpectation sut =
			Expect.That.Type(typeof(TypeExpectationTests));

		ITestResult<ITypeExpectation> result =
			sut.ShouldSatisfy(_ => false, _ => error);

		result.Errors.Should().NotBeEmpty();
		result.Errors.Single().Should().Be(error);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_True_ShouldNotIncludeError(TestError error)
	{
		IFilterableTypeExpectation sut =
			Expect.That.Type(typeof(TypeExpectationTests));

		ITestResult<ITypeExpectation>
			result = sut.ShouldSatisfy(_ => true, _ => error);

		result.Errors.Should().BeEmpty();
	}

	[Fact]
	public void Which_ShouldFilterOutTypes()
	{
		int allTypesCount =
			Expect.That.AssemblyContaining<TypeExpectation>()
				.Types.ShouldSatisfy(_ => false).Errors.Length;
		IFilterableTypeExpectation sut = Expect.That
			.AssemblyContaining<TypeExpectation>().Types;

		ITestResult<ITypeExpectation> result = sut
			.Which(p => p.Name != nameof(TypeExpectation))
			.ShouldSatisfy(_ => false);

		result.Errors.Length.Should().Be(allTypesCount - 1);
	}
}
