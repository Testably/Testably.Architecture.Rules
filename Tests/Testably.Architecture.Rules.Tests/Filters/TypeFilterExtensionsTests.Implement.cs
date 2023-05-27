using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

// ReSharper disable UnusedTypeParameter
public sealed partial class TypeFilterExtensionsTests
{
	public sealed class ImplementTests
	{
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void WhichDoNotImplement_WithClass_ShouldAlwaysBeViolated(bool forceDirect)
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(FooImplementor1), typeof(FooImplementor2)).And;

			ITypeFilterResult sut = source.WhichDoNotImplement<FooImplementor1>(forceDirect);

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain(
				$"does not implement {typeof(FooImplementor1)}");
			result.ShouldBeViolated();
			result.Errors.Length.Should().Be(2);
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(nameof(FooImplementor1)));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(nameof(FooImplementor2)));
		}

		[Fact]
		public void
			WhichDoNotImplement_WithGenericInterface_WithForceDirect_ShouldReturnTypesDirectlyImplementingTheInterface()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(FooGenericImplementor1<>), typeof(FooGenericImplementor2<>)).And;

			ITypeFilterResult sut =
				source.WhichDoNotImplement(typeof(IGenericFooInterface<>), true);

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain(
				$"does not implement {typeof(IGenericFooInterface<>)}");
			result.Errors.Length.Should().Be(1);
			result.Errors.Should()
				.NotContain(e => e.ToString().Contains(typeof(FooGenericImplementor1<>).Name));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor2<>).Name));
		}

		[Fact]
		public void
			WhichDoNotImplement_WithGenericInterface_WithoutForceDirect_ShouldReturnAllTypesImplementingInterface()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(FooGenericImplementor1<>), typeof(FooGenericImplementor2<>)).And
				.WhichDoNotImplement(typeof(IGenericFooInterface<>))
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void
			WhichDoNotImplement_WithInterface_WithForceDirect_ShouldReturnTypesNotDirectlyImplementingTheInterface()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(FooImplementor1), typeof(FooImplementor2)).And
				.WhichDoNotImplement<IFooInterface>(true)
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors.Length.Should().Be(1);
			result.Errors.Should()
				.NotContain(e => e.ToString().Contains(nameof(FooImplementor1)));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(nameof(FooImplementor2)));
		}

		[Fact]
		public void
			WhichDoNotImplement_WithInterface_WithoutForceDirect_ShouldReturnAllTypesImplementingInterface()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(FooImplementor1), typeof(FooImplementor2)).And
				.WhichDoNotImplement<IFooInterface>()
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void WhichImplement_WithClass_ShouldReturnFalse(bool forceDirect)
		{
			ITestResult result = Expect.That.Types
				.WhichImplement<FooImplementor1>(forceDirect)
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void
			WhichImplement_WithGenericInterface_WithForceDirect_ShouldReturnTypesDirectlyImplementingTheInterface()
		{
			ITypeFilter source = Expect.That.Types;

			ITypeFilterResult sut = source.WhichImplement(typeof(IGenericFooInterface<>), true);

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain(
				$"implements {typeof(IGenericFooInterface<>)}");
			result.Errors.Length.Should().Be(1);
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor1<>).Name));
			result.Errors.Should()
				.NotContain(e => e.ToString().Contains(typeof(FooGenericImplementor2<>).Name));
		}

		[Fact]
		public void
			WhichImplement_WithGenericInterface_WithoutForceDirect_ShouldReturnAllTypesImplementingInterface()
		{
			ITestResult result = Expect.That.Types
				.WhichImplement(typeof(IGenericFooInterface<>))
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(2);
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor1<>).Name));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor2<>).Name));
		}

		[Fact]
		public void
			WhichImplement_WithInterface_WithForceDirect_ShouldReturnTypesDirectlyImplementingTheInterface()
		{
			ITypeFilter source = Expect.That.Types;

			ITypeFilterResult sut = source.WhichImplement<IFooInterface>(true);

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain(
				$"implements {typeof(IFooInterface)}");
			result.Errors.Length.Should().Be(2);
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(nameof(FooImplementor1)));
			result.Errors.Should()
				.NotContain(e => e.ToString().Contains(nameof(FooImplementor2)));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor1<>).Name));
			result.Errors.Should()
				.NotContain(e => e.ToString().Contains(typeof(FooGenericImplementor2<>).Name));
		}

		[Fact]
		public void
			WhichImplement_WithInterface_WithoutForceDirect_ShouldReturnAllTypesImplementingInterface()
		{
			ITestResult result = Expect.That.Types
				.WhichImplement<IFooInterface>()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(4);
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(nameof(FooImplementor1)));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(nameof(FooImplementor2)));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor1<>).Name));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor2<>).Name));
		}

		private class FooGenericImplementor1<T> : IGenericFooInterface<T>,
			IFooInterface
		{
		}

		private class FooGenericImplementor2<T> : FooGenericImplementor1<T>
		{
		}

		private class FooImplementor1 : IFooInterface
		{
		}

		private class FooImplementor2 : FooImplementor1
		{
		}

		private interface IFooInterface
		{
		}

		private interface IGenericFooInterface<T>
		{
		}
	}
}
