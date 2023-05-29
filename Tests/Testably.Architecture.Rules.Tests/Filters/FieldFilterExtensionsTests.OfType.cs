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
		[Fact]
		public void OrAttribute_ShouldReturnBothTypes()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass), typeof(FooClass))
				.Should(Have.Field.OfType<FooClass>().OrOfType<BarClass>())
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Fact]
		public void OrAttribute_ShouldUseCorrectErrorMessage()
		{
			ITestResult result = Expect.That.Types
				.WhichAre(typeof(BarClass))
				.Should(Have.Field.OfType<int>())
				.Check.InAllLoadedAssemblies();

			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain($"type '{typeof(BarClass)}'")
				.And.Contain("field")
				.And.Contain(nameof(Int32));
		}

		[Fact]
		public void ShouldSatisfy_ShouldApplyOnFilteredResult()
		{
			IFieldFilterResult sut = Have.Field
				.OfType<FooClass>().OrOfType<BarClass>();

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
				.OfType<FooClass>().OrOfType<BarClass>();

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
			public FooClass FooField;
			public string StringField;
		}

		private class FooClass
		{
			public BarClass BarField;
			public string StringField;
		}
		#pragma warning restore CS8618
		#pragma warning restore CS0649
	}
}
