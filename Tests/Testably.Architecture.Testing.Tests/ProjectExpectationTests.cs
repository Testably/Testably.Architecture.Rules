using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public class ProjectExpectationTests
{
	[Fact]
	public void ShouldSatisfy_DefaultError_ShouldIncludeAssemblyName()
	{
		Assembly assembly = Assembly.GetExecutingAssembly();
		string expectedAssemblyName = $"'{assembly.GetName().Name}'";
		IFilterableProjectExpectation sut = Expect.That.FromAssembly(assembly);

		ITestResult<IProjectExpectation> result = sut.ShouldSatisfy(_ => false);

		TestError error = result.Errors.Single();
		error.ToString().Should().Contain(expectedAssemblyName);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_False_ShouldIncludeError(TestError error)
	{
		IFilterableProjectExpectation sut =
			Expect.That.FromAssembly(Assembly.GetExecutingAssembly());

		ITestResult<IProjectExpectation> result = sut.ShouldSatisfy(_ => false, _ => error);

		result.Errors.Should().NotBeEmpty();
		result.Errors.Single().Should().Be(error);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_True_ShouldNotIncludeError(TestError error)
	{
		IFilterableProjectExpectation sut =
			Expect.That.FromAssembly(Assembly.GetExecutingAssembly());

		ITestResult<IProjectExpectation> result = sut.ShouldSatisfy(_ => true, _ => error);

		result.Errors.Should().BeEmpty();
	}

	[Fact]
	public void Which_ShouldFilterOutProjects()
	{
		int allProjectCount =
			Expect.That.AllLoadedProjects().ShouldSatisfy(_ => false).Errors.Length;
		IFilterableProjectExpectation sut = Expect.That.AllLoadedProjects();

		ITestResult<IProjectExpectation> errors = sut
			.Which(p => !p.Name.StartsWith("System"))
			.ShouldSatisfy(_ => false);

		errors.Errors.Length.Should().BeLessThan(allProjectCount);
	}
}
