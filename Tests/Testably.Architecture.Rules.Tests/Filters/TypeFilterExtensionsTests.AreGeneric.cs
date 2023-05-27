using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class TypeFilterExtensionsTests
{
	public sealed class AreGenericTests
	{
		[Fact]
		public void WhichAreGeneric_ShouldFilterForGenericTypes()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(GenericClass<>), typeof(NotGenericClass)).And;

			ITypeFilterResult sut = source.WhichAreGeneric();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is generic");
			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(GenericClass<>).FullName);
		}

		[Fact]
		public void WhichAreNotGeneric_ShouldFilterForNotGenericTypes()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(GenericClass<>), typeof(NotGenericClass)).And;

			ITypeFilterResult sut = source.WhichAreNotGeneric();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is not generic");
			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(NotGenericClass).FullName);
		}

		// ReSharper disable once UnusedTypeParameter
		private class GenericClass<T>
		{
		}

		private class NotGenericClass
		{
		}
	}
}
