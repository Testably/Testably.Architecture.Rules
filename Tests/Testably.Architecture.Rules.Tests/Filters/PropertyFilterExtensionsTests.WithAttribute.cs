using FluentAssertions;
using System;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable UnusedMember.Local
public sealed partial class PropertyFilterExtensionsTests
{
	public sealed class WithAttributeTests
	{
		[Fact]
		public void OrAttribute_ShouldReturnBothTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass), typeof(FooClass))
				.Should(Have.Property.WithAttribute<FooAttribute>().OrAttribute<BarAttribute>())
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void OrAttribute_ShouldUseCorrectErrorMessage()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass))
				.Should(Have.Property.WithAttribute<FooAttribute>())
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain($"type '{typeof(BarClass)}'")
				.And.Contain("property")
				.And.Contain(nameof(FooAttribute));
		}

		[Fact]
		public void ShouldSatisfy_ShouldApplyOnFilteredResult()
		{
			IPropertyFilterResult sut = Have.Property
				.WithAttribute<FooAttribute>().OrAttribute<BarAttribute>();

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
				.WithAttribute<FooAttribute>().OrAttribute<BarAttribute>();

			IRequirementResult<Type> rule = sut.Types.ShouldSatisfy(_ => false);

			ITestResult result = rule.Check.InAllLoadedAssemblies();
			result.Errors.Length.Should().Be(2);
			result.Errors.Should().Contain(e
				=> ((TypeTestError)e).Type == typeof(FooClass));
			result.Errors.Should().Contain(e
				=> ((TypeTestError)e).Type == typeof(BarClass));
		}

		[AttributeUsage(AttributeTargets.Property)]
		private class BarAttribute : Attribute
		{
		}

		private class BarClass
		{
			[Bar]
			public int BarProperty { get; set; }
		}

		[AttributeUsage(AttributeTargets.Property)]
		private class FooAttribute : Attribute
		{
		}

		private class FooClass
		{
			[Foo]
			public int FooProperty { get; set; }
		}
	}
}
