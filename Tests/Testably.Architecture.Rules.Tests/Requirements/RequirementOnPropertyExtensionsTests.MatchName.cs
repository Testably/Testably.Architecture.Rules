using FluentAssertions;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed class RequirementOnPropertyExtensionsTests
{
	public sealed class MatchNameTests
	{
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldMatchName_IgnoreCase_ShouldConsiderParameter(bool ignoreCase)
		{
			PropertyInfo property =
				typeof(DummyFooClass).GetProperty(nameof(DummyFooClass.DummyFooProperty1))!;
			IRule rule = Expect.That.Properties
				.WhichAre(property)
				.ShouldMatchName("DUMMYfooPROPERTY1", ignoreCase);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(!ignoreCase);
		}

		[Theory]
		[InlineData("*")]
		[InlineData("?????FooProperty1")]
		[InlineData("DummyFooProperty1")]
		public void ShouldMatchName_MatchingPattern_ShouldNotBeViolated(string matchingPattern)
		{
			PropertyInfo property =
				typeof(DummyFooClass).GetProperty(nameof(DummyFooClass.DummyFooProperty1))!;
			IRule rule = Expect.That.Properties
				.WhichAre(property)
				.ShouldMatchName(matchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Theory]
		[InlineData("*Foo")]
		[InlineData("??FooProperty1")]
		[InlineData("dummyfooproperty1")]
		public void ShouldMatchName_NotMatchingPattern_ShouldNotBeSatisfied(
			string notMatchingPattern)
		{
			PropertyInfo property =
				typeof(DummyFooClass).GetProperty(nameof(DummyFooClass.DummyFooProperty1))!;
			IRule rule = Expect.That.Properties
				.WhichAre(property)
				.ShouldMatchName(notMatchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<PropertyTestError>()
				.Which.Property.Should().BeSameAs(property);
			result.Errors[0].Should().BeOfType<PropertyTestError>()
				.Which.ToString().Should().Contain(property.Name).And
				.Contain($"'{notMatchingPattern}'");
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldNotMatchName_IgnoreCase_ShouldConsiderParameter(bool ignoreCase)
		{
			PropertyInfo property =
				typeof(DummyFooClass).GetProperty(nameof(DummyFooClass.DummyFooProperty1))!;
			IRule rule = Expect.That.Properties
				.WhichAre(property)
				.ShouldNotMatchName("DUMMYfooPROPERTY1", ignoreCase);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(ignoreCase);
		}

		[Theory]
		[InlineData("*")]
		[InlineData("?????FooProperty1")]
		[InlineData("DummyFooProperty1")]
		public void ShouldNotMatchName_MatchingPattern_ShouldNotBeSatisfied(string matchingPattern)
		{
			PropertyInfo property =
				typeof(DummyFooClass).GetProperty(nameof(DummyFooClass.DummyFooProperty1))!;
			IRule rule = Expect.That.Properties
				.WhichAre(property)
				.ShouldNotMatchName(matchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<PropertyTestError>()
				.Which.Property.Should().BeSameAs(property);
			result.Errors[0].Should().BeOfType<PropertyTestError>()
				.Which.ToString().Should().Contain(property.Name).And
				.Contain($"'{matchingPattern}'");
		}

		[Theory]
		[InlineData("*Foo")]
		[InlineData("??FooProperty1")]
		[InlineData("dummyfooproperty1")]
		public void ShouldNotMatchName_NotMatchingPattern_ShouldNotBeViolated(
			string notMatchingPattern)
		{
			PropertyInfo property =
				typeof(DummyFooClass).GetProperty(nameof(DummyFooClass.DummyFooProperty1))!;
			IRule rule = Expect.That.Properties
				.WhichAre(property)
				.ShouldNotMatchName(notMatchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}
	}
}
