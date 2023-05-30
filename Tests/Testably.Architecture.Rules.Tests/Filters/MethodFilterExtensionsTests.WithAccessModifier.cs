using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

// ReSharper disable UnusedMember.Local
public sealed partial class MethodFilterExtensionsTests
{
	public sealed class WithAccessModifierTests
	{
		[Theory]
		[InlineData(typeof(TestClassWithInternalMethod), AccessModifiers.Internal)]
		[InlineData(typeof(TestClassWithPrivateMethod), AccessModifiers.Private)]
		[InlineData(typeof(TestClassWithProtectedMethod), AccessModifiers.Protected)]
		[InlineData(typeof(TestClassWithPublicMethod), AccessModifiers.Public)]
		public void With_AccessModifiers_Matching_ShouldNotBeViolated(Type type,
			AccessModifiers accessModifiers)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(type)
				.Should(Have.Method.With(accessModifiers))
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Theory]
		[InlineData(typeof(TestClassWithInternalMethod),
			AccessModifiers.Private | AccessModifiers.Protected)]
		[InlineData(typeof(TestClassWithInternalMethod),
			AccessModifiers.Public)]
		[InlineData(typeof(TestClassWithPrivateMethod),
			AccessModifiers.Internal | AccessModifiers.Public)]
		[InlineData(typeof(TestClassWithPrivateMethod),
			AccessModifiers.Protected)]
		[InlineData(typeof(TestClassWithProtectedMethod),
			AccessModifiers.Internal | AccessModifiers.Private)]
		[InlineData(typeof(TestClassWithProtectedMethod),
			AccessModifiers.Public)]
		[InlineData(typeof(TestClassWithPublicMethod),
			AccessModifiers.Protected | AccessModifiers.Internal)]
		[InlineData(typeof(TestClassWithPublicMethod), AccessModifiers.Private)]
		public void With_AccessModifiers_NotMatching_ShouldBeViolated(Type type,
			AccessModifiers accessModifiers)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(type)
				.Should(Have.Method.With(accessModifiers))
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].ToString().Should()
				.Contain(
					$"type '{type.FullName}' should have a method with {accessModifiers} access modifier");
		}

		#pragma warning disable CA1822
		private class TestClassWithPublicMethod
		{
			public void PublicTestMethod()
			{
				// Do nothing
			}
		}

		private class TestClassWithInternalMethod
		{
			internal void InternalTestMethod()
			{
				// Do nothing
			}
		}

		private class TestClassWithProtectedMethod
		{
			protected void ProtectedTestMethod()
			{
				// Do nothing
			}
		}

		private class TestClassWithPrivateMethod
		{
			private void PrivateTestMethod()
			{
				// Do nothing
			}
		}
		#pragma warning restore CA1822
	}
}
