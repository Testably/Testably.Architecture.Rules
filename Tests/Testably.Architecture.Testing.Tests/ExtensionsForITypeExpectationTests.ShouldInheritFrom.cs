﻿using FluentAssertions;
using System;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed partial class ExtensionsForITypeExpectationTests
{
	public sealed class ShouldInherit
	{
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldInheritFrom_ForceDirect_WithClass_ShouldConsiderParameter(bool forceDirect)
		{
			Type type = typeof(FooImplementor2);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldInheritFrom<FooBase>(
				forceDirect: forceDirect);

			result.IsSatisfied.Should().Be(!forceDirect);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldInheritFrom_ForceDirect_WithGenericClass_ShouldConsiderParameter(
			bool forceDirect)
		{
			Type type = typeof(GenericFooImplementor2<>);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldInheritFrom(
				typeof(IGenericFooInterface<>),
				forceDirect: forceDirect);

			result.IsSatisfied.Should().Be(!forceDirect);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldInheritFrom_ForceDirect_WithGenericClassAndInterface_ShouldConsiderParameter(
			bool forceDirect)
		{
			Type type = typeof(GenericFooImplementor2<>);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldInheritFrom(typeof(IFooInterface),
				forceDirect: forceDirect);

			result.IsSatisfied.Should().Be(!forceDirect);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldInheritFrom_ForceDirect_WithGenericInterface_ShouldConsiderParameter(
			bool forceDirect)
		{
			Type type = typeof(GenericFooImplementor2<>);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldInheritFrom(typeof(GenericFooClass<>),
				forceDirect: forceDirect);

			result.IsSatisfied.Should().Be(!forceDirect);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldInheritFrom_ForceDirect_WithInterface_ShouldConsiderParameter(
			bool forceDirect)
		{
			Type type = typeof(FooImplementor2);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldInheritFrom<IFooInterface>(
				forceDirect: forceDirect);

			result.IsSatisfied.Should().Be(!forceDirect);
		}

		[Fact]
		public void ShouldInheritFrom_WithClass_ShouldBeSatisfied()
		{
			Type type = typeof(FooImplementor3);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldInheritFrom<FooBase>();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldInheritFrom_WithGenericClass_ShouldBeSatisfied()
		{
			Type type = typeof(GenericFooImplementor3<>);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation>
				result = sut.ShouldInheritFrom(typeof(GenericFooClass<>));

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldInheritFrom_WithGenericInterface_ShouldBeSatisfied()
		{
			Type type = typeof(GenericFooImplementor3<>);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation>
				result = sut.ShouldInheritFrom(typeof(IGenericFooInterface<>));

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldInheritFrom_WithInterface_ShouldBeSatisfied()
		{
			Type type = typeof(FooImplementor3);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldInheritFrom<IFooInterface>();

			result.IsSatisfied.Should().BeTrue();
		}

		[Fact]
		public void ShouldInheritFrom_WithoutClass_ShouldNotBeSatisfied()
		{
			Type type = typeof(GenericFooImplementor3<>);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldInheritFrom<BarClass>();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
		}

		[Fact]
		public void ShouldInheritFrom_WithoutGenericClass_ShouldNotBeSatisfied()
		{
			Type type = typeof(GenericFooImplementor3<>);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation>
				result = sut.ShouldInheritFrom(typeof(GenericBarClass<>));

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
		}

		[Fact]
		public void ShouldInheritFrom_WithoutGenericInterface_ShouldNotBeSatisfied()
		{
			Type type = typeof(GenericFooImplementor3<>);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation>
				result = sut.ShouldInheritFrom(typeof(IOtherGenericFooInterface<>));

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
		}

		[Fact]
		public void ShouldInheritFrom_WithoutInterface_ShouldNotBeSatisfied()
		{
			Type type = typeof(GenericFooImplementor3<>);
			IFilterableTypeExpectation sut = Expect.That.Type(type);

			ITestResult<ITypeExpectation> result = sut.ShouldInheritFrom<IOtherFooInterface>();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
		}

		private abstract class FooBase
		{
		}

		private class FooImplementor1 : FooBase, IFooInterface
		{
		}

		private class FooImplementor2 : FooImplementor1
		{
		}

		private class FooImplementor3 : FooImplementor2
		{
		}

		// ReSharper disable once UnusedTypeParameter
		private class GenericFooClass<T>
		{
		}

		private class GenericFooImplementor1<T> : GenericFooClass<T>, IGenericFooInterface<T>,
			IFooInterface
		{
		}

		private class GenericFooImplementor2<T> : GenericFooImplementor1<T>
		{
		}

		private class GenericFooImplementor3<T> : GenericFooImplementor2<T>
		{
		}

		private interface IFooInterface
		{
		}

		// ReSharper disable once UnusedTypeParameter
		private interface IGenericFooInterface<T>
		{
		}

		private interface IOtherFooInterface
		{
		}

		// ReSharper disable once UnusedTypeParameter
		private interface IOtherGenericFooInterface<T>
		{
		}

		private abstract class BarClass
		{
		}

		// ReSharper disable once UnusedTypeParameter
		private class GenericBarClass<T>
		{
		}
	}
}
