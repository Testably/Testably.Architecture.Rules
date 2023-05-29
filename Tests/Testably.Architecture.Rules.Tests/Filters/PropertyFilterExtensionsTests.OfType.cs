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
		[Fact]
		public void OrAttribute_ShouldReturnBothTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass), typeof(FooClass))
				.Should(Have.Property.OfType<FooClass>().OrOfType<BarClass>())
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void OrAttribute_ShouldUseCorrectErrorMessage()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass))
				.Should(Have.Property.OfType<int>())
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain($"type '{typeof(BarClass)}'")
				.And.Contain("property")
				.And.Contain(nameof(Int32));
		}

		[Fact]
		public void ShouldSatisfy_ShouldApplyOnFilteredResult()
		{
			IPropertyFilterResult sut = Have.Property
				.OfType<FooClass>().OrOfType<BarClass>();

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
				.OfType<FooClass>().OrOfType<BarClass>();

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
			public FooClass FooProperty { get; set; }
			public string StringProperty { get; set; }
		}

		private class FooClass
		{
			public BarClass BarProperty { get; set; }
			public string StringProperty { get; set; }
		}
		#pragma warning restore CS8618
	}
}
