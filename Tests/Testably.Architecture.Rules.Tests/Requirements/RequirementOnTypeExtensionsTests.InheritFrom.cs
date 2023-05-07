using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	public sealed class InheritFromTests
	{
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldInheritFrom_ForceDirect_WithClass_ShouldConsiderParameter(
			bool forceDirect)
		{
			Type type = typeof(FooImplementor2);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldInheritFrom<FooBase>(
					forceDirect: forceDirect);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(forceDirect);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldInheritFrom_ForceDirect_WithGenericClass_ShouldConsiderParameter(
			bool forceDirect)
		{
			Type type = typeof(GenericFooImplementor2<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldInheritFrom(
					typeof(IGenericFooInterface<>),
					forceDirect: forceDirect);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(forceDirect);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void
			ShouldInheritFrom_ForceDirect_WithGenericClassAndInterface_ShouldConsiderParameter(
				bool forceDirect)
		{
			Type type = typeof(GenericFooImplementor2<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldInheritFrom(typeof(IFooInterface),
					forceDirect: forceDirect);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(forceDirect);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldInheritFrom_ForceDirect_WithGenericInterface_ShouldConsiderParameter(
			bool forceDirect)
		{
			Type type = typeof(GenericFooImplementor2<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldInheritFrom(typeof(GenericFooClass<>),
					forceDirect: forceDirect);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(forceDirect);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldInheritFrom_ForceDirect_WithInterface_ShouldConsiderParameter(
			bool forceDirect)
		{
			Type type = typeof(FooImplementor2);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldInheritFrom<IFooInterface>(
					forceDirect: forceDirect);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(forceDirect);
		}

		[Fact]
		public void ShouldInheritFrom_WithClass_ShouldNotBeViolated()
		{
			Type type = typeof(FooImplementor3);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldInheritFrom<FooBase>();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldInheritFrom_WithGenericClass_ShouldNotBeViolated()
		{
			Type type = typeof(GenericFooImplementor3<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldInheritFrom(typeof(GenericFooClass<>));

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldInheritFrom_WithGenericInterface_ShouldNotBeViolated()
		{
			Type type = typeof(GenericFooImplementor3<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldInheritFrom(typeof(IGenericFooInterface<>));

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldInheritFrom_WithInterface_ShouldNotBeViolated()
		{
			Type type = typeof(FooImplementor3);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldInheritFrom<IFooInterface>();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldInheritFrom_WithoutClass_ShouldNotBeSatisfied()
		{
			Type type = typeof(GenericFooImplementor3<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldInheritFrom<BarClass>();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
		}

		[Fact]
		public void ShouldInheritFrom_WithoutGenericClass_ShouldNotBeSatisfied()
		{
			Type type = typeof(GenericFooImplementor3<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldInheritFrom(typeof(GenericBarClass<>));

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
		}

		[Fact]
		public void ShouldInheritFrom_WithoutGenericInterface_ShouldNotBeSatisfied()
		{
			Type type = typeof(GenericFooImplementor3<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldInheritFrom(typeof(IOtherGenericFooInterface<>));

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
		}

		[Fact]
		public void ShouldInheritFrom_WithoutInterface_ShouldNotBeSatisfied()
		{
			Type type = typeof(GenericFooImplementor3<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldInheritFrom<IOtherFooInterface>();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldNotInheritFrom_ForceDirect_WithClass_ShouldConsiderParameter(
			bool forceDirect)
		{
			Type type = typeof(FooImplementor2);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotInheritFrom<FooBase>(
					forceDirect: forceDirect);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(!forceDirect);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldNotInheritFrom_ForceDirect_WithGenericClass_ShouldConsiderParameter(
			bool forceDirect)
		{
			Type type = typeof(GenericFooImplementor2<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotInheritFrom(
					typeof(IGenericFooInterface<>),
					forceDirect: forceDirect);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(!forceDirect);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void
			ShouldNotInheritFrom_ForceDirect_WithGenericClassAndInterface_ShouldConsiderParameter(
				bool forceDirect)
		{
			Type type = typeof(GenericFooImplementor2<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotInheritFrom(typeof(IFooInterface),
					forceDirect: forceDirect);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(!forceDirect);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldNotInheritFrom_ForceDirect_WithGenericInterface_ShouldConsiderParameter(
			bool forceDirect)
		{
			Type type = typeof(GenericFooImplementor2<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotInheritFrom(
					typeof(GenericFooClass<>),
					forceDirect: forceDirect);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(!forceDirect);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldNotInheritFrom_ForceDirect_WithInterface_ShouldConsiderParameter(
			bool forceDirect)
		{
			Type type = typeof(FooImplementor2);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotInheritFrom<IFooInterface>(
					forceDirect: forceDirect);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(!forceDirect);
		}

		[Fact]
		public void ShouldNotInheritFrom_WithClass_ShouldNotBeSatisfied()
		{
			Type type = typeof(FooImplementor3);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotInheritFrom<FooBase>();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
		}

		[Fact]
		public void ShouldNotInheritFrom_WithGenericClass_ShouldNotBeSatisfied()
		{
			Type type = typeof(GenericFooImplementor3<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotInheritFrom(typeof(GenericFooClass<>));

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
		}

		[Fact]
		public void ShouldNotInheritFrom_WithGenericInterface_ShouldNotBeSatisfied()
		{
			Type type = typeof(GenericFooImplementor3<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotInheritFrom(typeof(IGenericFooInterface<>));

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
		}

		[Fact]
		public void ShouldNotInheritFrom_WithInterface_ShouldNotBeSatisfied()
		{
			Type type = typeof(FooImplementor3);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotInheritFrom<IFooInterface>();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
		}

		[Fact]
		public void ShouldNotInheritFrom_WithoutClass_ShouldNotBeViolated()
		{
			Type type = typeof(GenericFooImplementor3<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotInheritFrom<BarClass>();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldNotInheritFrom_WithoutGenericClass_ShouldNotBeViolated()
		{
			Type type = typeof(GenericFooImplementor3<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotInheritFrom(typeof(GenericBarClass<>));

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldNotInheritFrom_WithoutGenericInterface_ShouldNotBeViolated()
		{
			Type type = typeof(GenericFooImplementor3<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotInheritFrom(typeof(IOtherGenericFooInterface<>));

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldNotInheritFrom_WithoutInterface_ShouldNotBeViolated()
		{
			Type type = typeof(GenericFooImplementor3<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotInheritFrom<IOtherFooInterface>();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		private abstract class BarClass
		{
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
		private class GenericBarClass<T>
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
	}
}
