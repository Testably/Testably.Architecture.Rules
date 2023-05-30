using FluentAssertions;
using System;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class FieldFilterExtensionsTests
{
	public sealed class OfTypeTests
	{
		[Theory]
		[InlineData(true, false)]
		[InlineData(false, true)]
		public void DerivedType_ShouldBeViolatedIfNotAllowDerivedType(
			bool allowDerivedType,
			bool expectViolation)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(FooClass))
				.Should(Have.Field.OfType<DummyFooBase>(allowDerivedType))
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectViolation);
		}

		[Fact]
		public void OfType_ShouldUseCorrectErrorMessage()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(FooClass))
				.Should(Have.Field.OfType<int>())
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain($"type '{typeof(FooClass)}' should have a field of type {nameof(Int32)}");
		}

		[Fact]
		public void OrAttribute_ShouldReturnBothTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(FooClass), typeof(BarClass))
				.Should(Have.Field.OfType<DummyFooClass>().OrOfType<DummyBarClass>())
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void OrAttribute_ShouldUseCorrectErrorMessage()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(FooClass))
				.Should(Have.Field.OfType<int>().OrOfType<long>())
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(
					$"type '{typeof(FooClass)}' should have a field (of type {nameof(Int32)} or of type {nameof(Int64)})");
		}

		[Fact]
		public void ShouldSatisfy_ShouldApplyOnFilteredResult()
		{
			IFieldFilterResult sut = Have.Field
				.OfType<DummyFooClass>().OrOfType<DummyBarClass>();

			IRequirementResult<FieldInfo> rule = sut.ShouldSatisfy(_ => false);

			ITestResult result = rule.Check.InAllLoadedAssemblies();
			result.Errors.Length.Should().Be(2);
			result.Errors.Should().Contain(e
				=> ((FieldTestError)e).Field.DeclaringType == typeof(FooClass));
			result.Errors.Should().Contain(e
				=> ((FieldTestError)e).Field.DeclaringType == typeof(BarClass));
		}

		[Fact]
		public void Types_ShouldForwardToFilteredResult()
		{
			IFieldFilterResult sut = Have.Field
				.OfType<DummyFooClass>().OrOfType<DummyBarClass>();

			IRequirementResult<Type> rule = sut.Types.ShouldSatisfy(_ => false);

			ITestResult result = rule.Check.InAllLoadedAssemblies();
			result.Errors.Length.Should().Be(2);
			result.Errors.Should().Contain(e
				=> ((TypeTestError)e).Type == typeof(FooClass));
			result.Errors.Should().Contain(e
				=> ((TypeTestError)e).Type == typeof(BarClass));
		}

		#pragma warning disable CS0649
		#pragma warning disable CS8618
		private class BarClass
		{
			public DummyBarClass FooField;
			public string StringField;
		}

		private class FooClass
		{
			public DummyFooClass BarField;
			public string StringField;
		}
		#pragma warning restore CS8618
		#pragma warning restore CS0649
	}
}
