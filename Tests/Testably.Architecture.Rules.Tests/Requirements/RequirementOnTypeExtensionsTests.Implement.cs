using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

// ReSharper disable UnusedTypeParameter
public sealed partial class RequirementOnTypeExtensionsTests
{
	public sealed class ImplementTests
	{
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldImplement_ForceDirect_WithInterface_ShouldConsiderParameter(
			bool forceDirect)
		{
			Type type = typeof(FooImplementor2);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldImplement<IFooInterface>(
					forceDirect: forceDirect);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(forceDirect);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldImplement_ToString_ShouldBeCorrect(bool forceDirect)
		{
			Type type = typeof(FooImplementor2);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldImplement<IOtherFooInterface>(
					forceDirect: forceDirect);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain($"type '{type.Name}'")
				.And.Contain(
					$"should{(forceDirect ? " directly" : "")} implement '{nameof(IOtherFooInterface)}'.");
		}

		[Fact]
		public void ShouldImplement_WithClass_ShouldNotBeSatisfied()
		{
			Type type = typeof(FooImplementor2);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldImplement<FooImplementor1>();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
		}

		[Fact]
		public void ShouldImplement_WithGenericClass_ShouldNotBeSatisfied()
		{
			Type type = typeof(GenericFooImplementor2<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldImplement(typeof(GenericFooImplementor1<>));

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void
			ShouldImplement_WithGenericInterface_ForceDirect_ShouldConsiderParameter(
				bool forceDirect)
		{
			Type type = typeof(GenericFooImplementor2<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldImplement(typeof(IGenericFooInterface<>),
					forceDirect: forceDirect);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(forceDirect);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldNotImplement_ForceDirect_WithInterface_ShouldConsiderParameter(
			bool forceDirect)
		{
			Type type = typeof(FooImplementor2);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotImplement<IFooInterface>(
					forceDirect: forceDirect);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(!forceDirect);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldNotImplement_ToString_ShouldBeCorrect(bool forceDirect)
		{
			Type type = typeof(FooImplementor1);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotImplement<IFooInterface>(
					forceDirect: forceDirect);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain($"type '{type.Name}'")
				.And.Contain(
					$"should not{(forceDirect ? " directly" : "")} implement '{nameof(IFooInterface)}'.");
		}

		[Fact]
		public void ShouldNotImplement_WithClass_ShouldNotBeSatisfied()
		{
			Type type = typeof(FooImplementor2);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotImplement<FooImplementor1>();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldNotImplement_WithGenericClass_ShouldNotBeSatisfied()
		{
			Type type = typeof(GenericFooImplementor2<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotImplement(typeof(GenericFooImplementor1<>));

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void
			ShouldNotImplement_WithGenericInterface_ForceDirect_ShouldConsiderParameter(
				bool forceDirect)
		{
			Type type = typeof(GenericFooImplementor2<>);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotImplement(typeof(IGenericFooInterface<>),
					forceDirect: forceDirect);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(!forceDirect);
		}

		private class FooImplementor1 : IFooInterface
		{
		}

		private class FooImplementor2 : FooImplementor1
		{
		}

		private class GenericFooImplementor1<T> : IGenericFooInterface<T>,
			IFooInterface
		{
		}

		private class GenericFooImplementor2<T> : GenericFooImplementor1<T>
		{
		}

		private interface IFooInterface
		{
		}

		private interface IGenericFooInterface<T>
		{
		}

		private interface IOtherFooInterface
		{
		}
	}
}
