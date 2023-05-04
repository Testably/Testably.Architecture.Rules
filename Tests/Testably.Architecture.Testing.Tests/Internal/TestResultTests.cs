using AutoFixture.Xunit2;
using FluentAssertions;
using System.Reflection;
using Xunit;

namespace Testably.Architecture.Testing.Tests.Internal;

public sealed class TestResultTests
{
	[Theory]
	[AutoData]
	public void And_FirstFailed_ShouldIncludeOnlyFirstError(
		TestError error1, TestError error2)
	{
		IAssemblyExpectation sut = Expect.That.Assembly(Assembly.GetExecutingAssembly());

		ITestResult result = sut
			.ShouldSatisfy(_ => false, _ => error1).And
			.ShouldSatisfy(_ => true, _ => error2);

		result.IsSatisfied.Should().BeFalse();
		result.Errors.Should().NotBeEmpty();
		result.Errors.Length.Should().Be(1);
		result.Errors.Should().Contain(error1);
	}

	[Theory]
	[AutoData]
	public void And_NoneFailed_ShouldBeEmpty(TestError error1, TestError error2)
	{
		IAssemblyExpectation sut = Expect.That.Assembly(Assembly.GetExecutingAssembly());

		ITestResult result = sut
			.ShouldSatisfy(_ => true, _ => error1).And
			.ShouldSatisfy(_ => true, _ => error2);

		result.IsSatisfied.Should().BeTrue();
		result.Errors.Should().BeEmpty();
	}

	[Theory]
	[AutoData]
	public void And_SecondFailed_ShouldIncludeOnlySecondError(
		TestError error1, TestError error2)
	{
		IAssemblyExpectation sut = Expect.That.Assembly(Assembly.GetExecutingAssembly());

		ITestResult result = sut
			.ShouldSatisfy(_ => true, _ => error1).And
			.ShouldSatisfy(_ => false, _ => error2);

		result.IsSatisfied.Should().BeFalse();
		result.Errors.Should().NotBeEmpty();
		result.Errors.Length.Should().Be(1);
		result.Errors.Should().Contain(error2);
	}

	[Theory]
	[AutoData]
	public void And_ShouldIncludeMultipleErrors(TestError error1, TestError error2)
	{
		IAssemblyExpectation sut = Expect.That.Assembly(Assembly.GetExecutingAssembly());

		ITestResult result = sut
			.ShouldSatisfy(_ => false, _ => error1).And
			.ShouldSatisfy(_ => false, _ => error2);

		result.IsSatisfied.Should().BeFalse();
		result.Errors.Should().NotBeEmpty();
		result.Errors.Length.Should().Be(2);
		result.Errors.Should().Contain(error1);
		result.Errors.Should().Contain(error2);
	}
}
