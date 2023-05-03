using AutoFixture.Xunit2;
using FluentAssertions;
using System;
using System.Linq;
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
		var sut = Expect.That.Type(type);

		ITestResult<IExpectationCondition<Type>> result = sut.ShouldSatisfy(_ => false);

		TestError error = result.Errors.Single();
		error.ToString().Should().Contain(expectedTypeName);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_False_ShouldIncludeError(TestError error)
	{
		var sut =
			Expect.That.Type(typeof(TypeExpectationTests));

		ITestResult<IExpectationCondition<Type>> result =
			sut.ShouldSatisfy(_ => false, _ => error);

		result.Errors.Should().NotBeEmpty();
		result.Errors.Single().Should().Be(error);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_True_ShouldNotIncludeError(TestError error)
	{
		var sut =
			Expect.That.Type(typeof(TypeExpectationTests));

		ITestResult<IExpectationCondition<Type>>
			result = sut.ShouldSatisfy(_ => true, _ => error);

		result.Errors.Should().BeEmpty();
	}

	[Fact]
	public void Which_ShouldFilterOutTypes()
	{
		int allTypesCount =
			Expect.That.AssemblyContaining<TypeExpectationStart>()
				.Types.ShouldSatisfy(_ => false).Errors.Length;
		var sut = Expect.That
			.AssemblyContaining<TypeExpectationStart>().Types;

		ITestResult<IExpectationCondition<Type>> result = sut
			.Which(p => p.Name != nameof(TypeExpectationStart))
			.ShouldSatisfy(_ => false);

		result.Errors.Length.Should().Be(allTypesCount - 1);
	}
}
