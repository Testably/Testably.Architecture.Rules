using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

// ReSharper disable UnusedMember.Local
public sealed partial class ConstructorFilterExtensionsTests
{
	public sealed class AreTests
	{
		[Theory]
		[InlineData(typeof(TestClassWithInternalConstructor), AccessModifiers.Internal)]
		[InlineData(typeof(TestClassWithPrivateConstructor), AccessModifiers.Private)]
		[InlineData(typeof(TestClassWithProtectedConstructor), AccessModifiers.Protected)]
		[InlineData(typeof(TestClassWithPublicConstructor), AccessModifiers.Public)]
		public void With_AccessModifiers_Matching_ShouldNotBeViolated(Type type,
			AccessModifiers accessModifiers)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(type)
				.Should(Have.Constructor.WhichAre(accessModifiers))
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Theory]
		[InlineData(typeof(TestClassWithInternalConstructor),
			AccessModifiers.Private | AccessModifiers.Protected)]
		[InlineData(typeof(TestClassWithInternalConstructor),
			AccessModifiers.Public)]
		[InlineData(typeof(TestClassWithPrivateConstructor),
			AccessModifiers.Internal | AccessModifiers.Public)]
		[InlineData(typeof(TestClassWithPrivateConstructor),
			AccessModifiers.Protected)]
		[InlineData(typeof(TestClassWithProtectedConstructor),
			AccessModifiers.Internal | AccessModifiers.Private)]
		[InlineData(typeof(TestClassWithProtectedConstructor),
			AccessModifiers.Public)]
		[InlineData(typeof(TestClassWithPublicConstructor),
			AccessModifiers.Protected | AccessModifiers.Internal)]
		[InlineData(typeof(TestClassWithPublicConstructor), AccessModifiers.Private)]
		public void With_AccessModifiers_NotMatching_ShouldBeViolated(Type type,
			AccessModifiers accessModifiers)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(type)
				.Should(Have.Constructor.WhichAre(accessModifiers))
				.Check.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].ToString().Should()
				.Contain(
					$"type '{type.FullName}' should have a constructor with {accessModifiers} access modifier");
		}

		#pragma warning disable CA1822
		private class TestClassWithPublicConstructor
		{
			public TestClassWithPublicConstructor(int value)
			{
				_ = value;
			}
		}

		private class TestClassWithInternalConstructor
		{
			internal TestClassWithInternalConstructor(int value)
			{
				_ = value;
			}
		}

		private class TestClassWithProtectedConstructor
		{
			protected TestClassWithProtectedConstructor(int value)
			{
				_ = value;
			}
		}

		private class TestClassWithPrivateConstructor
		{
			private TestClassWithPrivateConstructor(int value)
			{
				_ = value;
			}
		}
		#pragma warning restore CA1822
	}
}
