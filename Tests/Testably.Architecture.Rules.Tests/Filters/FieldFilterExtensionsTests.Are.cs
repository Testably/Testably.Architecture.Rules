using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class FieldFilterExtensionsTests
{
	public sealed class AreTests
	{
		[Theory]
		[InlineData(typeof(TestClassWithInternalField), AccessModifiers.Internal)]
		[InlineData(typeof(TestClassWithPrivateField), AccessModifiers.Private)]
		[InlineData(typeof(TestClassWithProtectedField), AccessModifiers.Protected)]
		[InlineData(typeof(TestClassWithPublicField), AccessModifiers.Public)]
		public void With_AccessModifiers_Matching_ShouldNotBeViolated(Type type,
			AccessModifiers accessModifiers)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(type)
				.Should(Have.Field.WhichAre(accessModifiers))
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Theory]
		[InlineData(typeof(TestClassWithInternalField),
			AccessModifiers.Private | AccessModifiers.Protected)]
		[InlineData(typeof(TestClassWithInternalField),
			AccessModifiers.Public)]
		[InlineData(typeof(TestClassWithPrivateField),
			AccessModifiers.Internal | AccessModifiers.Public)]
		[InlineData(typeof(TestClassWithPrivateField),
			AccessModifiers.Protected)]
		[InlineData(typeof(TestClassWithProtectedField),
			AccessModifiers.Internal | AccessModifiers.Private)]
		[InlineData(typeof(TestClassWithProtectedField),
			AccessModifiers.Public)]
		[InlineData(typeof(TestClassWithPublicField),
			AccessModifiers.Protected | AccessModifiers.Internal)]
		[InlineData(typeof(TestClassWithPublicField), AccessModifiers.Private)]
		public void With_AccessModifiers_NotMatching_ShouldBeViolated(Type type,
			AccessModifiers accessModifiers)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(type)
				.Should(Have.Field.WhichAre(accessModifiers))
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].ToString().Should()
				.Contain(
					$"type '{type.FullName}' should have a field with {accessModifiers} access modifier");
		}

		#pragma warning disable CS0414
		private class TestClassWithPublicField
		{
			public int PublicTestField = 1;
		}

		private class TestClassWithInternalField
		{
			internal int InternalTestField = 1;
		}

		private class TestClassWithProtectedField
		{
			protected int ProtectedTestField = 1;
		}

		private class TestClassWithPrivateField
		{
			// ReSharper disable once InconsistentNaming
			private readonly int PrivateTestField = 1;
		}
		#pragma warning restore CS0414
	}
}
