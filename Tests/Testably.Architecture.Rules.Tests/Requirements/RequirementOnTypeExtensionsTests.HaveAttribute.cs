using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	public sealed class HaveAttribute
	{
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldHaveAttribute_Inherit_ShouldConsiderParameter(bool inherit)
		{
			Type type = typeof(TestClassWithInheritedAttribute);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldHaveAttribute<DummyAttribute>(
				inherit: inherit);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(!inherit);
		}

		[Theory]
		[InlineData(1, false)]
		[InlineData(2, true)]
		public void ShouldHaveAttribute_Predicate_ShouldConsiderParameter(int value,
			bool expectIsViolated)
		{
			Type type = typeof(TestClassWithInheritedAttribute);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldHaveAttribute<DummyAttribute>(
				predicate: d => d.Value == value);
			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectIsViolated);
		}

		[Fact]
		public void ShouldHaveAttribute_WithAttribute_ShouldBeSatisfied()
		{
			Type type = typeof(TestClassWithAttribute);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldHaveAttribute<DummyAttribute>();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void ShouldHaveAttribute_WithoutAttribute_ShouldNotBeSatisfied()
		{
			Type type = typeof(TestClassWithoutAttribute);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldHaveAttribute<DummyAttribute>();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should have correct attribute")
				.And.Contain(nameof(DummyAttribute));
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldNotHaveAttribute_Inherit_ShouldConsiderParameter(bool inherit)
		{
			Type type = typeof(TestClassWithInheritedAttribute);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotHaveAttribute<DummyAttribute>(
				inherit: inherit);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(inherit);
		}

		[Theory]
		[InlineData(1, true)]
		[InlineData(2, false)]
		public void ShouldNotHaveAttribute_Predicate_ShouldConsiderParameter(
			int value,
			bool expectIsViolated)
		{
			Type type = typeof(TestClassWithInheritedAttribute);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotHaveAttribute<DummyAttribute>(
				predicate: d => d.Value == value);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(expectIsViolated);
		}

		[Fact]
		public void ShouldNotHaveAttribute_WithAttribute_ShouldNotBeSatisfied()
		{
			Type type = typeof(TestClassWithAttribute);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotHaveAttribute<DummyAttribute>();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not have correct attribute")
				.And.Contain(nameof(DummyAttribute));
		}

		[Fact]
		public void ShouldNotHaveAttribute_WithoutAttribute_ShouldBeSatisfied()
		{
			Type type = typeof(TestClassWithoutAttribute);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotHaveAttribute<DummyAttribute>();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
		private class DummyAttribute : Attribute
		{
			public int Value { get; }

			public DummyAttribute(int value)
			{
				Value = value;
			}
		}

		[Dummy(1)]
		private class TestClassWithAttribute
		{
			// ReSharper disable once UnusedMember.Local
			public void Method1WithoutAttribute()
				=> throw new NotSupportedException();

			// ReSharper disable once UnusedMember.Local
			public void Method2WithoutAttribute()
				=> throw new NotSupportedException();
		}

		private class TestClassWithInheritedAttribute : TestClassWithAttribute
		{
		}

		private class TestClassWithoutAttribute
		{
			[Dummy(1)]
			// ReSharper disable once UnusedMember.Local
			public void Method1WithAttribute()
				=> throw new NotSupportedException();

			[Dummy(2)]
			// ReSharper disable once UnusedMember.Local
			public void Method2WithAttribute()
				=> throw new NotSupportedException();
		}
	}
}
