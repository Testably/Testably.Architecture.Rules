using FluentAssertions;
using System.Linq;
using System.Reflection;
using Testably.Abstractions.Testing;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnAssemblyExtensionsTests
{
	public sealed class HaveDependenciesOnTests
	{
		[Fact]
		public void ShouldNotHaveDependenciesOn_CaseSensitivity_ShouldDefaultToSensitive()
		{
			IRule rule = Expect.That.Assemblies
				.ShouldNotHaveDependenciesOn("testably.*");

			ITestResult result = rule.Check
				.InAssemblyContaining<MockFileSystem>();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldNotHaveDependenciesOn_ErrorsShouldIncludeNameOfAllFailedReferences()
		{
			Assembly testAssembly = typeof(MockFileSystem).Assembly;
			string[] expectedReferences = testAssembly.GetReferencedAssemblies()
				.Select(x => x.FullName)
				.ToArray();

			IRule rule = Expect.That.Assemblies
				.ShouldNotHaveDependenciesOn("*");

			ITestResult result = rule.Check
				.InAssemblyContaining<MockFileSystem>();

			result.ShouldBeViolated();

			result.Errors.Length.Should().Be(1);
			TestError error = result.Errors.Single();
			error.Should().BeOfType<DependencyTestError>();
			DependencyTestError dependencyTestError = (DependencyTestError)error;

			dependencyTestError.Assembly.Should().BeSameAs(testAssembly);
			dependencyTestError.AssemblyReferences.Length.Should()
				.Be(expectedReferences.Length);
			foreach (string reference in expectedReferences)
			{
				dependencyTestError.AssemblyReferences
					.Select(x => x.FullName)
					.Should().Contain(reference);
			}
		}

		[Fact]
		public void ShouldNotHaveDependenciesOn_WithDependencyStartingWithPrefix_ShouldReturnFalse()
		{
			IRule rule = Expect.That.Assemblies
				.ShouldNotHaveDependenciesOn("Testably.*");

			ITestResult result = rule.Check
				.InAssemblyContaining<MockFileSystem>();

			result.ShouldBeViolated();
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldNotHaveDependenciesOn_WithIgnoreCaseParameter_ShouldConsiderCaseSensitivity(
			bool ignoreCase)
		{
			IRule rule = Expect.That.Assemblies
				.ShouldNotHaveDependenciesOn("testably.*", ignoreCase);

			ITestResult result = rule.Check
				.InAssemblyContaining<MockFileSystem>();

			result.ShouldBeViolatedIf(ignoreCase);
		}
	}
}
