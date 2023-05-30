using FluentAssertions;
using System;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

// ReSharper disable UnusedMember.Local
public sealed partial class PropertyFilterExtensionsTests
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
				.Should(Have.Property.OfType<DummyFooBase>(allowDerivedType))
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectViolation);
		}

		[Fact]
		public void OfType_ShouldUseCorrectErrorMessage()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(FooClass))
				.Should(Have.Property.OfType<int>())
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(
					$"type '{typeof(FooClass)}' should have a property of type {nameof(Int32)}");
		}

		[Fact]
		public void OrAttribute_ShouldReturnBothTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass), typeof(FooClass))
				.Should(Have.Property.OfType<DummyFooClass>().OrOfType<DummyBarClass>())
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void OrAttribute_ShouldUseCorrectErrorMessage()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(FooClass))
				.Should(Have.Property.OfType<int>().OrOfType<long>())
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(
					$"type '{typeof(FooClass)}' should have a property (of type {nameof(Int32)} or of type {nameof(Int64)})");
		}

		[Fact]
		public void ShouldSatisfy_ShouldApplyOnFilteredResult()
		{
			IPropertyFilterResult sut = Have.Property
				.OfType<DummyFooClass>().OrOfType<DummyBarClass>();

			IRequirementResult<PropertyInfo> rule = sut.ShouldSatisfy(_ => false);

			ITestResult result = rule.Check.InAllLoadedAssemblies();
			result.Errors.Length.Should().Be(2);
			result.Errors.Should().Contain(e
				=> ((PropertyTestError)e).Property.DeclaringType == typeof(FooClass));
			result.Errors.Should().Contain(e
				=> ((PropertyTestError)e).Property.DeclaringType == typeof(BarClass));
		}

		[Fact]
		public void Types_ShouldForwardToFilteredResult()
		{
			IPropertyFilterResult sut = Have.Property
				.OfType<DummyFooClass>().OrOfType<DummyBarClass>();

			IRequirementResult<Type> rule = sut.Types.ShouldSatisfy(_ => false);

			ITestResult result = rule.Check.InAllLoadedAssemblies();
			result.Errors.Length.Should().Be(2);
			result.Errors.Should().Contain(e
				=> ((TypeTestError)e).Type == typeof(FooClass));
			result.Errors.Should().Contain(e
				=> ((TypeTestError)e).Type == typeof(BarClass));
		}

		#pragma warning disable CS8618
		private class BarClass
		{
			public DummyBarClass FooProperty { get; set; }
			public string StringProperty { get; set; }
		}

		private class FooClass
		{
			public DummyFooClass BarProperty { get; set; }
			public string StringProperty { get; set; }
		}
		#pragma warning restore CS8618
	}
}
