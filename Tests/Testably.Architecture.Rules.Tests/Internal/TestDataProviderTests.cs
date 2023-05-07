using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Testably.Architecture.Rules.Internal;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class TestDataProviderTests
{
	[Fact]
	public void
		Filter_WithAssembly_WhenApplyExclusionFiltersIsSet_ShouldFilterOutExcludedAssemblies()
	{
		TestDataProvider sutWithAppliedFilter = new(
			AppDomain.CurrentDomain.GetAssemblies());
		TestDataProvider sutWithoutAppliedFilter = new(
			AppDomain.CurrentDomain.GetAssemblies(), false);

		List<Assembly> assembliesWithFilter =
			sutWithAppliedFilter.Filter(sutWithAppliedFilter.GetAssemblies()).ToList();
		List<Assembly> assembliesWithoutFilter = sutWithoutAppliedFilter
			.Filter(sutWithoutAppliedFilter.GetAssemblies()).ToList();

		assembliesWithFilter.Count.Should().BeLessThan(assembliesWithoutFilter.Count);
		assembliesWithFilter.Should().OnlyContain(a
			=> ExclusionLists.ExcludedAssemblyNamespaces.All(x
				=> a.FullName!.StartsWith(x) == false));
		assembliesWithoutFilter.Should().Contain(a
			=> ExclusionLists.ExcludedAssemblyNamespaces.Any(x
				=> a.FullName!.StartsWith(x) != false));
	}

	[Fact]
	public void Filter_WithType_WhenApplyExclusionFiltersIsSet_ShouldFilterOutExcludedTypes()
	{
		TestDataProvider sutWithAppliedFilter = new(
			AppDomain.CurrentDomain.GetAssemblies());
		TestDataProvider sutWithoutAppliedFilter = new(
			AppDomain.CurrentDomain.GetAssemblies(), false);
		List<Type> types = sutWithAppliedFilter.Filter(
				AppDomain.CurrentDomain.GetAssemblies())
			.SelectMany(x => x.GetTypes())
			.ToList();

		List<Type> typesWithFilter = sutWithAppliedFilter
			.Filter(types).ToList();
		List<Type> typesWithoutFilter = sutWithoutAppliedFilter
			.Filter(types).ToList();

		typesWithFilter.Count.Should().BeLessThan(typesWithoutFilter.Count);
		typesWithFilter.Should().OnlyContain(a
			=> ExclusionLists.ExcludedTypeNamespaces.All(x
				=> a.FullName!.StartsWith(x) == false));
		typesWithoutFilter.Should().Contain(a
			=> ExclusionLists.ExcludedTypeNamespaces.Any(x
				=> a.FullName!.StartsWith(x) != false));
	}
}
