using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Linq;
using Testably.Architecture.Testing.Exceptions;
using Testably.Architecture.Testing.Internal;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests.Internal;

public sealed class TypeExpectationTests
{
	[Fact]
	public void ShouldSatisfy_DefaultError_ShouldIncludeTypeName()
	{
		Type type = typeof(TypeExpectationTests);
		string expectedTypeName = $"'{type.Name}'";
		ITypeExpectation sut = Expect.That.Type(type);

		ITestResult result = sut.ShouldSatisfy(_ => false);

		TestError error = result.Errors.Single();
		error.ToString().Should().Contain(expectedTypeName);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_False_ShouldIncludeError(TestError error)
	{
		ITypeExpectation sut =
			Expect.That.Type(typeof(TypeExpectationTests));

		ITestResult result = sut.ShouldSatisfy(_ => false, _ => error);

		result.Errors.Should().NotBeEmpty();
		result.Errors.Single().Should().Be(error);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_True_ShouldNotIncludeError(TestError error)
	{
		ITypeExpectation sut =
			Expect.That.Type(typeof(TypeExpectationTests));

		ITestResult result = sut.ShouldSatisfy(_ => true, _ => error);

		result.Errors.Should().BeEmpty();
	}

	[Fact]
	public void Which_ShouldFilterOutTypes()
	{
		int allTypesCount =
			Expect.That.AssemblyContaining<TypeExpectationStart>()
				.Types.ShouldSatisfy(_ => false).Errors.Length;
		ITypeExpectation sut = Expect.That
			.AssemblyContaining<TypeExpectationStart>().Types;

		ITestResult result = sut
			.Which(p => p.Name != nameof(TypeExpectationStart))
			.ShouldSatisfy(_ => false);

		result.Errors.Length.Should().Be(allTypesCount - 1);
	}

	[Fact]
	public void Which_WithOrNoneSet_WithoutMatch_ShouldNotThrowException()
	{
		IExpectationFilterResult<Type> sut = Expect.That
			.AllLoadedTypes()
			.OrNone()
			.Which(_ => false);

		ITestResult result = sut.ShouldSatisfy(_ => false);

		result.IsSatisfied.Should().BeTrue();
	}

	[Fact]
	public void Which_WithoutMatch_ShouldThrowEmptyDataException()
	{
		IExpectationFilterResult<Type> sut = Expect.That
			.AllLoadedTypes()
			.Which(_ => false);

		Exception? exception = Record.Exception(() =>
		{
			sut.ShouldSatisfy(_ => false);
		});

		exception.Should().BeOfType<EmptyDataException>();
	}
}
