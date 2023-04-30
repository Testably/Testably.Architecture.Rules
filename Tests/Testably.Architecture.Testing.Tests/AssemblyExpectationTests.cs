using AutoFixture.Xunit2;
using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public class AssemblyExpectationTests
{
	[Fact]
	public void ShouldSatisfy_DefaultError_ShouldIncludeAssemblyName()
	{
		Assembly assembly = Assembly.GetExecutingAssembly();
		string expectedAssemblyName = $"'{assembly.GetName().Name}'";
		IFilterableAssemblyExpectation sut = Expect.That.Assembly(assembly);

		ITestResult<IAssemblyExpectation> result = sut.ShouldSatisfy(_ => false);

		TestError error = result.Errors.Single();
		error.ToString().Should().Contain(expectedAssemblyName);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_False_ShouldIncludeError(TestError error)
	{
		IFilterableAssemblyExpectation sut =
			Expect.That.Assembly(Assembly.GetExecutingAssembly());

		ITestResult<IAssemblyExpectation> result =
			sut.ShouldSatisfy(_ => false, _ => error);

		result.Errors.Should().NotBeEmpty();
		result.Errors.Single().Should().Be(error);
	}

	[Theory]
	[AutoData]
	public void ShouldSatisfy_True_ShouldNotIncludeError(TestError error)
	{
		IFilterableAssemblyExpectation sut =
			Expect.That.Assembly(Assembly.GetExecutingAssembly());

		ITestResult<IAssemblyExpectation>
			result = sut.ShouldSatisfy(_ => true, _ => error);

		result.Errors.Should().BeEmpty();
	}

	[Fact]
	public void Which_ShouldFilterOutAssemblies()
	{
		int allAssembliesCount =
			Expect.That.AllLoadedAssemblies().ShouldSatisfy(_ => false).Errors.Length;
		IFilterableAssemblyExpectation sut = Expect.That.AllLoadedAssemblies();

		ITestResult<IAssemblyExpectation> errors = sut
		   .Which(p => p.GetName().Name?.StartsWith("System") != true)
		   .ShouldSatisfy(_ => false);

		errors.Errors.Length.Should().BeLessThan(allAssembliesCount);
	}
}
