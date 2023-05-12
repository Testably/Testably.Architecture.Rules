using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class FilterOnTypeExtensionsTests
{
	public sealed class InheritFromTests
	{
		[Fact]
		public void WhichInheritFrom_WithForceDirect_WithClass_ShouldConsiderParameter()
		{
			ITestResult result = Expect.That.Types
				.WhichInheritFrom<FooBase>(true)
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(nameof(FooImplementor1)));
			result.Errors.Should()
				.NotContain(e => e.ToString().Contains(nameof(FooImplementor2)));
		}

		[Fact]
		public void WhichInheritFrom_WithForceDirect_WithGenericClass_ShouldConsiderParameter()
		{
			ITestResult result = Expect.That.Types
				.WhichInheritFrom(typeof(FooGenericClass<>), true)
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor1<>).Name));
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
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor1<>).Name));
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
				.WhichInheritFrom(typeof(FooGenericClass<>))
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
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor1<>).Name));
			result.Errors.Should()
				.Contain(e => e.ToString().Contains(typeof(FooGenericImplementor2<>).Name));
		}

		private abstract class FooBase
		{
		}

		// ReSharper disable once UnusedTypeParameter
		private class FooGenericClass<T>
		{
		}

		private class FooGenericImplementor1<T> : FooGenericClass<T>, IGenericFooInterface<T>,
			IFooInterface
		{
		}

		private class FooGenericImplementor2<T> : FooGenericImplementor1<T>
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

		// ReSharper disable once UnusedTypeParameter
		private interface IGenericFooInterface<T>
		{
		}
	}
}
