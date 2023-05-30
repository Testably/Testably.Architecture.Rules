using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

// ReSharper disable UnusedMember.Local
public sealed partial class TypeFilterExtensionsTests
{
	public sealed class WithAccessModifierTests
	{
		[Theory]
		[InlineData(typeof(InternalTestClass), AccessModifiers.Internal)]
		[InlineData(typeof(PrivateTestClass), AccessModifiers.Private)]
		[InlineData(typeof(ProtectedTestClass), AccessModifiers.Protected)]
		[InlineData(typeof(PublicTestClass), AccessModifiers.Public)]
		public void With_AccessModifiers_Matching_ShouldNotBeViolated(Type type,
			AccessModifiers accessModifiers)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(type)
				.ShouldBe(accessModifiers)
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Theory]
		[InlineData(typeof(InternalTestClass),
			AccessModifiers.Private | AccessModifiers.Protected)]
		[InlineData(typeof(InternalTestClass),
			AccessModifiers.Public)]
		[InlineData(typeof(PrivateTestClass),
			AccessModifiers.Internal | AccessModifiers.Public)]
		[InlineData(typeof(PrivateTestClass),
			AccessModifiers.Protected)]
		[InlineData(typeof(ProtectedTestClass),
			AccessModifiers.Internal | AccessModifiers.Private)]
		[InlineData(typeof(ProtectedTestClass),
			AccessModifiers.Public)]
		[InlineData(typeof(PublicTestClass),
			AccessModifiers.Protected | AccessModifiers.Internal)]
		[InlineData(typeof(PublicTestClass), AccessModifiers.Private)]
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
		public class PublicTestClass
		{
		}

		internal class InternalTestClass
		{
		}

		#pragma warning disable CS0628
		protected class ProtectedTestClass
		{
		}
		#pragma warning restore CS0628

		private class PrivateTestClass
		{
		}
		#pragma warning restore CA1822
	}
}
