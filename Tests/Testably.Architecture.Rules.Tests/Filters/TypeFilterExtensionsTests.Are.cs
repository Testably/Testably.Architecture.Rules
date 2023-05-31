using FluentAssertions;
using System;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

// ReSharper disable UnusedMember.Local
public sealed partial class TypeFilterExtensionsTests
{
	public sealed class AreTests
	{
		[Theory]
		[InlineData(typeof(InternalTestClass), AccessModifiers.Internal)]
		[InlineData(typeof(PrivateTestClass), AccessModifiers.Private)]
		[InlineData(typeof(ProtectedTestClass), AccessModifiers.Protected)]
		[InlineData(typeof(PublicTestClass), AccessModifiers.Public)]
		[InlineData(typeof(UnnestedPublicType), AccessModifiers.Public)]
		[InlineData(typeof(UnnestedInternalType), AccessModifiers.Internal)]
		public void WhichAre_AccessModifiers_Matching_ShouldBeChecked(
			Type type,
			AccessModifiers accessModifiers)
		{
			ITypeFilterResult sut = Expect.That.Types
				.WhichAre(type).And
				.WhichAre(accessModifiers);

			ITestResult result =
				sut.ShouldSatisfy(_ => false)
					.AllowEmpty()
					.Check.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			sut.ToString().Should()
				.Contain($"with {accessModifiers} access modifier");
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
		public void WhichAre_AccessModifiers_NotMatching_ShouldNotBeChecked(
			Type type,
			AccessModifiers accessModifiers)
		{
			ITestResult result = Expect.That.Types
				.WhichAre(type).And
				.WhichAre(accessModifiers)
				.ShouldAlwaysFail()
				.AllowEmpty()
				.Check.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
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
