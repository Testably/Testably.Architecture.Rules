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
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(GenericClass<>), typeof(NotGenericClass)).And
				.WhichAreGeneric()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(GenericClass<>).FullName);
		}

		[Fact]
		public void WhichAreNotGeneric_ShouldFilterForNotGenericTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(GenericClass<>), typeof(NotGenericClass)).And
				.WhichAreNotGeneric()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

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
