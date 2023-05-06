using FluentAssertions;
using System;
using System.Collections.Generic;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
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
			Type type = typeof(RequirementOnTypeExtensionsTests);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBePublic();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Theory]
		[MemberData(nameof(GetUnpublicTypes))]
		public void ShouldBePublic_UnpublicType_ShouldNotBeSatisfied(Type type)
		{
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBePublic();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be public");
		}

		[Fact]
		public void ShouldNotBePublic_PublicType_ShouldNotBeSatisfied()
		{
			Type type = typeof(RequirementOnTypeExtensionsTests);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBePublic();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be public");
		}

		[Theory]
		[MemberData(nameof(GetUnpublicTypes))]
		public void ShouldNotBePublic_UnpublicType_ShouldBeSatisfied(Type type)
		{
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBePublic();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
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
