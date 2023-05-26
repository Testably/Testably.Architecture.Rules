using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

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

		[Fact(Skip = "TODO")]
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

		[AttributeUsage(AttributeTargets.Field)]
		private class BarAttribute : Attribute
		{
		} // ReSharper disable ClassNeverInstantiated.Local
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
