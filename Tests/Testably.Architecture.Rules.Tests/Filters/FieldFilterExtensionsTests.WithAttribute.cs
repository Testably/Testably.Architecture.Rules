﻿using FluentAssertions;
using System;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

// ReSharper disable ClassNeverInstantiated.Local
public sealed partial class FieldFilterExtensionsTests
{
	public sealed class WithAttributeTests
	{
		[Fact]
		public void OrAttribute_ShouldReturnBothTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass), typeof(FooClass))
				.Should(Have.Field.WithAttribute<FooAttribute>().OrAttribute<BarAttribute>())
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void OrAttribute_ShouldUseCorrectErrorMessage()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass))
				.Should(Have.Field.WithAttribute<FooAttribute>())
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain($"type '{typeof(BarClass)}'")
				.And.Contain("field")
				.And.Contain(nameof(FooAttribute));
		}

		[Fact]
		public void ShouldSatisfy_ShouldApplyOnFilteredResult()
		{
			IFieldFilterResult sut = Have.Field
				.WithAttribute<FooAttribute>().OrAttribute<BarAttribute>();

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
				.WithAttribute<FooAttribute>().OrAttribute<BarAttribute>();

			IRequirementResult<Type> rule = sut.Types.ShouldSatisfy(_ => false);

			ITestResult result = rule.Check.InAllLoadedAssemblies();
			result.Errors.Length.Should().Be(2);
			result.Errors.Should().Contain(e
				=> ((TypeTestError)e).Type == typeof(FooClass));
			result.Errors.Should().Contain(e
				=> ((TypeTestError)e).Type == typeof(BarClass));
		}

		[AttributeUsage(AttributeTargets.Field)]
		private class BarAttribute : Attribute
		{
		}
		#pragma warning disable CS0649
		private class BarClass
		{
			[Bar] public int BarField;
		}

		[AttributeUsage(AttributeTargets.Field)]
		private class FooAttribute : Attribute
		{
		}

		private class FooClass
		{
			[Foo] public int FooField;
		}
		#pragma warning restore CS0649
	}
}
