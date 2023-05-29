using FluentAssertions;
using System.Reflection;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnFieldExtensionsTests
{
	public sealed class MatchNameTests
	{
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldMatchName_IgnoreCase_ShouldConsiderParameter(bool ignoreCase)
		{
			FieldInfo field =
				typeof(DummyFooClass).GetField(nameof(DummyFooClass.DummyFooField1))!;
			IRule rule = Expect.That.Fields
				.WhichAre(field)
				.ShouldMatchName("DUMMYfooFIELD1", ignoreCase);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(!ignoreCase);
		}

		[Theory]
		[InlineData("*")]
		[InlineData("?????FooField1")]
		[InlineData("DummyFooField1")]
		public void ShouldMatchName_MatchingPattern_ShouldNotBeViolated(string matchingPattern)
		{
			FieldInfo field =
				typeof(DummyFooClass).GetField(nameof(DummyFooClass.DummyFooField1))!;
			IRule rule = Expect.That.Fields
				.WhichAre(field)
				.ShouldMatchName(matchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Theory]
		[InlineData("*Foo")]
		[InlineData("??FooField1")]
		[InlineData("dummyfoofield1")]
		public void ShouldMatchName_NotMatchingPattern_ShouldNotBeSatisfied(
			string notMatchingPattern)
		{
			FieldInfo field =
				typeof(DummyFooClass).GetField(nameof(DummyFooClass.DummyFooField1))!;
			IRule rule = Expect.That.Fields
				.WhichAre(field)
				.ShouldMatchName(notMatchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<FieldTestError>()
				.Which.Field.Should().BeSameAs(field);
			result.Errors[0].Should().BeOfType<FieldTestError>()
				.Which.ToString().Should().Contain(field.Name).And
				.Contain($"'{notMatchingPattern}'");
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void ShouldNotMatchName_IgnoreCase_ShouldConsiderParameter(bool ignoreCase)
		{
			FieldInfo field =
				typeof(DummyFooClass).GetField(nameof(DummyFooClass.DummyFooField1))!;
			IRule rule = Expect.That.Fields
				.WhichAre(field)
				.ShouldNotMatchName("DUMMYfooFIELD1", ignoreCase);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolatedIf(ignoreCase);
		}

		[Theory]
		[InlineData("*")]
		[InlineData("?????FooField1")]
		[InlineData("DummyFooField1")]
		public void ShouldNotMatchName_MatchingPattern_ShouldNotBeSatisfied(string matchingPattern)
		{
			FieldInfo field =
				typeof(DummyFooClass).GetField(nameof(DummyFooClass.DummyFooField1))!;
			IRule rule = Expect.That.Fields
				.WhichAre(field)
				.ShouldNotMatchName(matchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<FieldTestError>()
				.Which.Field.Should().BeSameAs(field);
			result.Errors[0].Should().BeOfType<FieldTestError>()
				.Which.ToString().Should().Contain(field.Name).And.Contain($"'{matchingPattern}'");
		}

		[Theory]
		[InlineData("*Foo")]
		[InlineData("??FooField1")]
		[InlineData("dummyfoofield1")]
		public void ShouldNotMatchName_NotMatchingPattern_ShouldNotBeViolated(
			string notMatchingPattern)
		{
			FieldInfo field =
				typeof(DummyFooClass).GetField(nameof(DummyFooClass.DummyFooField1))!;
			IRule rule = Expect.That.Fields
				.WhichAre(field)
				.ShouldNotMatchName(notMatchingPattern);

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}
	}
}
