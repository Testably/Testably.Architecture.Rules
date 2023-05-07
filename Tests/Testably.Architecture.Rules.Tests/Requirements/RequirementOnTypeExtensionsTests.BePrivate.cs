using FluentAssertions;
using System;
using System.Collections.Generic;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Requirements;

public sealed partial class RequirementOnTypeExtensionsTests
{
	public sealed class BePrivateTests
	{
		#region Test Setup

		public static IEnumerable<object[]> GetUnprivateTypes
		{
			get
			{
				yield return new object[]
				{
					typeof(InternalType)
				};
				yield return new object[]
				{
					typeof(PublicType)
				};
				yield return new object[]
				{
					typeof(ProtectedType)
				};
			}
		}

		#endregion

		[Fact]
		public void ShouldBePrivate_PrivateType_ShouldNotBeViolated()
		{
			Type type = typeof(PrivateType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBePrivate();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		[Theory]
		[MemberData(nameof(GetUnprivateTypes))]
		public void ShouldBePrivate_UnprivateType_ShouldNotBeSatisfied(Type type)
		{
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldBePrivate();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should be private");
		}

		[Fact]
		public void ShouldNotBePrivate_PrivateType_ShouldNotBeSatisfied()
		{
			Type type = typeof(PrivateType);
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBePrivate();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldBeViolated();
			result.Errors[0].Should().BeOfType<TypeTestError>()
				.Which.Type.Should().Be(type);
			result.Errors[0].ToString().Should().Contain("should not be private");
		}

		[Theory]
		[MemberData(nameof(GetUnprivateTypes))]
		public void ShouldNotBePrivate_UnprivateType_ShouldNotBeViolated(Type type)
		{
			IRule rule = Expect.That.Types
				.WhichAre(type)
				.ShouldNotBePrivate();

			ITestResult result = rule.Check
				.InAllLoadedAssemblies();

			result.ShouldNotBeViolated();
		}

		public class PublicType
		{
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
