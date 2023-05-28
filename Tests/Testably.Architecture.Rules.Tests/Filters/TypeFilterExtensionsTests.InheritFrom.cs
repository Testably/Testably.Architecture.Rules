using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

// ReSharper disable UnusedTypeParameter
public sealed partial class TypeFilterExtensionsTests
{
	public sealed class InheritFromTests
	{
		[Fact]
		public void WhichDoNotInheritFrom_WithForceDirect_WithClass_ShouldConsiderParameter()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(FooImplementor1), typeof(FooImplementor2)).And;

			ITypeFilterResult sut = source.WhichDoNotInheritFrom<FooBase>(true);

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain(
				$"does not inherit from {nameof(FooBase)}");
			result.Errors.Length.Should().Be(1);
			result.Errors.Should()
				.NotContain(e => e.ToString().Contains(nameof(FooImplementor1)));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(nameof(FooImplementor2)));
		}

		[Fact]
		public void WhichDoNotInheritFrom_WithForceDirect_WithGenericClass_ShouldConsiderParameter()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(FooGenericImplementor1<,>), typeof(FooGenericImplementor2<>)).And;

			ITypeFilterResult sut = source.WhichDoNotInheritFrom(typeof(FooGenericClass<,>), true);

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain(
				$"does not inherit from {typeof(FooGenericClass<,>).Name}");
			result.Errors.Length.Should().Be(1);
			result.Errors.Should()
				.NotContain(e => e.ToString().Contains(typeof(FooGenericImplementor1<,>).Name));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor2<>).Name));
		}

		[Fact]
		public void
			WhichDoNotInheritFrom_WithForceDirect_WithGenericClassAndInterface_ShouldConsiderParameter()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(
					typeof(FooImplementor1),
					typeof(FooImplementor2),
					typeof(FooGenericImplementor1<,>),
					typeof(FooGenericImplementor2<>)).And
				.WhichDoNotInheritFrom(typeof(IFooInterface), true)
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(2);
			result.Errors.Should()
				.NotContain(e => e.ToString().Contains(nameof(FooImplementor1)));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(nameof(FooImplementor2)));
			result.Errors.Should()
				.NotContain(e => e.ToString().Contains(typeof(FooGenericImplementor1<,>).Name));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor2<>).Name));
		}

		[Fact]
		public void WhichDoNotInheritFrom_WithoutForceDirect_WithClass_ShouldConsiderParameter()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(
					typeof(FooImplementor1),
					typeof(FooImplementor2)).And
				.WhichDoNotInheritFrom<FooBase>()
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void
			WhichDoNotInheritFrom_WithoutForceDirect_WithGenericClass_ShouldConsiderParameter()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(
					typeof(FooGenericImplementor1<,>),
					typeof(FooGenericImplementor2<>)).And
				.WhichDoNotInheritFrom(typeof(FooGenericClass<,>))
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void
			WhichDoNotInheritFrom_WithoutForceDirect_WithGenericClassAndInterface_ShouldConsiderParameter()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(
					typeof(FooImplementor1),
					typeof(FooImplementor2),
					typeof(FooGenericImplementor1<,>),
					typeof(FooGenericImplementor2<>)).And
				.WhichDoNotInheritFrom(typeof(IFooInterface))
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void WhichInheritFrom_WithForceDirect_WithClass_ShouldConsiderParameter()
		{
			ITypeFilter source = Expect.That.Types;

			ITypeFilterResult sut = source.WhichInheritFrom<FooBase>(true);

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain(
				$"inherits from {nameof(FooBase)}");
			result.Errors.Length.Should().Be(1);
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(nameof(FooImplementor1)));
			result.Errors.Should()
				.NotContain(e => e.ToString().Contains(nameof(FooImplementor2)));
		}

		[Fact]
		public void WhichInheritFrom_WithForceDirect_WithGenericClass_ShouldConsiderParameter()
		{
			ITypeFilter source = Expect.That.Types;

			ITypeFilterResult sut = source.WhichInheritFrom(typeof(FooGenericClass<,>), true);

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain(
				$"inherits from {typeof(FooGenericClass<,>).Name}");
			result.Errors.Length.Should().Be(1);
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor1<,>).Name));
			result.Errors.Should()
				.NotContain(e => e.ToString().Contains(typeof(FooGenericImplementor2<>).Name));
		}

		[Fact]
		public void
			WhichInheritFrom_WithForceDirect_WithGenericClassAndInterface_ShouldConsiderParameter()
		{
			ITestResult result = Expect.That.Types
				.WhichInheritFrom(typeof(IFooInterface), true)
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(2);
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(nameof(FooImplementor1)));
			result.Errors.Should()
				.NotContain(e => e.ToString().Contains(nameof(FooImplementor2)));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor1<,>).Name));
			result.Errors.Should()
				.NotContain(e => e.ToString().Contains(typeof(FooGenericImplementor2<>).Name));
		}

		[Fact]
		public void WhichInheritFrom_WithoutForceDirect_WithClass_ShouldConsiderParameter()
		{
			ITestResult result = Expect.That.Types
				.WhichInheritFrom<FooBase>()
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(2);
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(nameof(FooImplementor1)));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(nameof(FooImplementor2)));
		}

		[Fact]
		public void WhichInheritFrom_WithoutForceDirect_WithGenericClass_ShouldConsiderParameter()
		{
			ITestResult result = Expect.That.Types
				.WhichInheritFrom(typeof(FooGenericClass<,>))
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(2);
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor1<,>).Name));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor2<>).Name));
		}

		[Fact]
		public void
			WhichInheritFrom_WithoutForceDirect_WithGenericClassAndInterface_ShouldConsiderParameter()
		{
			ITestResult result = Expect.That.Types
				.WhichInheritFrom(typeof(IFooInterface))
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(4);
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(nameof(FooImplementor1)));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(nameof(FooImplementor2)));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor1<,>).Name));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor2<>).Name));
		}

		private abstract class FooBase
		{
		}

		private class FooGenericClass<T1, T2>
		{
		}

		private class FooGenericImplementor1<T1, T2> : FooGenericClass<T1, T2>,
			IGenericFooInterface<T1>,
			IFooInterface
		{
		}

		private class FooGenericImplementor2<T1> : FooGenericImplementor1<T1, FooBase>
		{
		}

		private class FooImplementor1 : FooBase, IFooInterface
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
