using FluentAssertions;
using System;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable UnusedMember.Local
public sealed partial class MethodFilterExtensionsTests
{
	public sealed class WithAttributeTests
	{
		[Fact]
		public void OrAttribute_ShouldReturnBothTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass), typeof(FooClass))
				.Should(Have.Method.WithAttribute<FooAttribute>().OrAttribute<BarAttribute>())
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void OrAttribute_ShouldUseCorrectErrorMessage()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass))
				.Should(Have.Method.WithAttribute<FooAttribute>())
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain($"type '{typeof(BarClass)}'")
				.And.Contain("method")
				.And.Contain(nameof(FooAttribute));
		}

		[Fact]
		public void ShouldSatisfy_ShouldApplyOnFilteredResult()
		{
			IMethodFilterResult sut = Have.Method
				.WithAttribute<FooAttribute>().OrAttribute<BarAttribute>();

			IRequirementResult<MethodInfo> rule = sut.ShouldSatisfy(_ => false);

			ITestResult result = rule.Check.InAllLoadedAssemblies();
			result.Errors.Length.Should().Be(2);
			result.Errors.Should().Contain(e
				=> ((MethodTestError)e).Method.DeclaringType == typeof(FooClass));
			result.Errors.Should().Contain(e
				=> ((MethodTestError)e).Method.DeclaringType == typeof(BarClass));
		}

		[Fact]
		public void Types_ShouldForwardToFilteredResult()
		{
			IMethodFilterResult sut = Have.Method
				.WithAttribute<FooAttribute>().OrAttribute<BarAttribute>();

			IRequirementResult<Type> rule = sut.Types.ShouldSatisfy(_ => false);

			ITestResult result = rule.Check.InAllLoadedAssemblies();
			result.Errors.Length.Should().Be(2);
			result.Errors.Should().Contain(e
				=> ((TypeTestError)e).Type == typeof(FooClass));
			result.Errors.Should().Contain(e
				=> ((TypeTestError)e).Type == typeof(BarClass));
		}

		[AttributeUsage(AttributeTargets.Method)]
		private class BarAttribute : Attribute
		{
		}

		#pragma warning disable CA1822
		private class BarClass
		{
			[Bar]
			public void BarMethod()
			{
				// Do nothing
			}
		}

		[AttributeUsage(AttributeTargets.Method)]
		private class FooAttribute : Attribute
		{
		}

		private class FooClass
		{
			[Foo]
			public void FooMethod()
			{
				// Do nothing
			}
		}
		#pragma warning restore CA1822
	}
}
