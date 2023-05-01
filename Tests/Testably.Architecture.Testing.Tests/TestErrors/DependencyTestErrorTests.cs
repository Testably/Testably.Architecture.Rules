using FluentAssertions;
using System;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests.TestErrors;

public sealed class DependencyTestErrorTests
{
	[Fact]
	public void Except_All_ShouldReturnTrueAndClearAllAssemblyReferences()
	{
		Assembly assembly = GetType().Assembly;
		DependencyTestError sut = new(assembly, assembly.GetReferencedAssemblies());

		bool result = sut.Except((_, _) => true);

		result.Should().BeTrue();
		sut.AssemblyReferences.Should().BeEmpty();
		sut.ToString().Should().Contain($"'{assembly.GetName().Name}'")
			.And.Contain(" no incorrect references");
	}

	[Fact]
	public void Except_ShouldRemoveMatchingAssemblyReferences()
	{
		Assembly assembly = GetType().Assembly;
		string? assemblyReferenceName = assembly
			.GetReferencedAssemblies()
			.First()
			.Name;
		DependencyTestError sut = new(assembly, assembly.GetReferencedAssemblies());
		int previousCount = sut.AssemblyReferences.Length;

		bool result = sut.Except((_, a) => a.Name == assemblyReferenceName);

		result.Should().BeFalse();
		sut.AssemblyReferences.Length.Should().Be(previousCount - 1);
		sut.AssemblyReferences.Select(x => x.Name)
			.Should().NotContain(assemblyReferenceName);
	}

	[Fact]
	public void Except_ShouldUpdateMessage()
	{
		Assembly assembly = GetType().Assembly;
		string? assemblyReferenceName = assembly
			.GetReferencedAssemblies()
			.First()
			.Name;
		DependencyTestError sut = new(assembly, assembly.GetReferencedAssemblies());

		bool result = sut.Except((_, a) => a.Name != assemblyReferenceName);

		result.Should().BeFalse();
		sut.ToString().Should().Contain($"'{assembly.GetName().Name}'")
			.And.Contain(" an incorrect reference ")
			.And.Contain($"'{assemblyReferenceName}'");
	}

	[Fact]
	public void ToString_WithMultipleReference_ShouldSortReferencedAssembliesAlphabetically()
	{
		Assembly assembly = GetType().Assembly;
		AssemblyName[] assemblyReferences = assembly.GetReferencedAssemblies()
			.OrderBy(x => x.Name)
			.ToArray();
		DependencyTestError sut = new(assembly, assemblyReferences);

		string result = sut.ToString();

		int lastIndex = 0;
		foreach (AssemblyName assemblyReference in assemblyReferences)
		{
			int foundIndex =
				result.IndexOf($"'{assemblyReference.Name}'", StringComparison.InvariantCulture);
			foundIndex.Should().BeGreaterThan(lastIndex,
				$"Reference '{assemblyReference.Name}' should be in the correct sort order");
			lastIndex = foundIndex;
		}

		result.Should().Contain($"'{string.Join("', '",
			assemblyReferences
				.Skip(1)
				.Take(assemblyReferences.Length - 2)
				.Select(x => x.Name))}'");
		result.Should().Contain($"and '{assemblyReferences.Last().Name}'");
	}

	[Fact]
	public void ToString_WithMultipleReferences_ShouldReturnMessageWithIncorrectReferences()
	{
		Assembly assembly = GetType().Assembly;
		AssemblyName[] assemblyReferences = assembly.GetReferencedAssemblies();
		DependencyTestError sut = new(assembly, assemblyReferences);

		string result = sut.ToString();

		result.Should().Contain($"'{assembly.GetName().Name}'")
			.And.Contain(" incorrect references ");

		assemblyReferences.Should().AllSatisfy(assemblyReference =>
			result.Should().Contain($"'{assemblyReference.Name}'"));
	}

	[Fact]
	public void ToString_WithoutAssemblyReferences_ShouldReturnNoIncorrectReferences()
	{
		Assembly assembly = GetType().Assembly;
		AssemblyName[] assemblyReferences = Array.Empty<AssemblyName>();
		DependencyTestError sut = new(assembly, assemblyReferences);

		string result = sut.ToString();

		result.Should().Contain($"'{assembly.GetName().Name}'")
			.And.Contain(" no incorrect references");
	}

	[Fact]
	public void ToString_WithSingleReference_ShouldReturnMessageWithAnIncorrectReference()
	{
		Assembly assembly = GetType().Assembly;
		AssemblyName assemblyReference = assembly
			.GetReferencedAssemblies()
			.First();
		AssemblyName[] assemblyReferences =
		{
			assemblyReference
		};
		DependencyTestError sut = new(assembly, assemblyReferences);

		string result = sut.ToString();

		result.Should().Contain($"'{assembly.GetName().Name}'")
			.And.Contain(" an incorrect reference ")
			.And.Contain($"'{assemblyReference.Name}'");
	}
}
