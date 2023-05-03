using FluentAssertions;
using System;
using System.Collections.Generic;
using Testably.Architecture.Testing.TestErrors;
using Xunit;

namespace Testably.Architecture.Testing.Tests;

public sealed partial class ExtensionsForITypeExpectationTests
{
	public sealed class BePublic
	{
		#region Test Setup

		public static IEnumerable<object[]> GetUnpublicTypes
		{
			get
			{
				yield return new object[]
				{
					typeof(InternalType)
				};
				yield return new object[]
				{
					typeof(PrivateType)
				};
				yield return new object[]
				{
					typeof(ProtectedType)
				};
			}
		}

		#endregion

		[Fact]
		public void ShouldBePublic_PublicType_ShouldBeSatisfied()
		{
			Type type = typeof(ExtensionsForITypeExpectationTests);
			ITypeExpectation sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldBePublic();

			result.IsSatisfied.Should().BeTrue();
		}

		[Theory]
		[MemberData(nameof(GetUnpublicTypes))]
		public void ShouldBePublic_UnpublicType_ShouldNotBeSatisfied(Type type)
		{
			ITypeExpectation sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldBePublic();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be public");
		}

		[Fact]
		public void ShouldNotBePublic_PublicType_ShouldNotBeSatisfied()
		{
			Type type = typeof(ExtensionsForITypeExpectationTests);
			ITypeExpectation sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldNotBePublic();

			result.IsSatisfied.Should().BeFalse();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be public");
		}

		[Theory]
		[MemberData(nameof(GetUnpublicTypes))]
		public void ShouldNotBePublic_UnpublicType_ShouldBeSatisfied(Type type)
		{
			ITypeExpectation sut = Expect.That.Type(type);

			IExpectationResult<Type> result = sut.ShouldNotBePublic();

			result.IsSatisfied.Should().BeTrue();
		}

		internal class InternalType
		{
		}

		private class PrivateType
		{
		}

		#pragma warning disable CS0628
		protected class ProtectedType
		{
		}
		#pragma warning restore CS0628
	}
}
