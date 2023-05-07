using FluentAssertions;
using System;
using Testably.Architecture.Rules.Internal;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Internal;

public sealed class RuleTests
{
	[Fact]
	public void ShouldSatisfy_And_ShouldAddMultipleRequirements()
	{
		Requirement<int> requirement1 =
			Requirement.Create<int>(_ => true, _ => new TestError("foo"));
		Requirement<int> requirement2 =
			Requirement.Create<int>(_ => false, _ => new TestError("bar"));
		DummyRule sut = new();

		sut.ShouldSatisfy(requirement1).And.ShouldSatisfy(requirement2);

		sut.GetFilters().Should().BeEmpty();
		sut.GetRequirements().Length.Should().Be(2);
		sut.GetRequirements().Should().Contain(x => x == requirement1);
		sut.GetRequirements().Should().Contain(x => x == requirement2);
		sut.GetExemptions().Should().BeEmpty();
	}

	[Fact]
	public void ShouldSatisfy_ShouldAddRequirement()
	{
		Requirement<int> requirement =
			Requirement.Create<int>(_ => true, _ => new TestError("foo"));
		DummyRule sut = new();

		sut.ShouldSatisfy(requirement);

		sut.GetFilters().Should().BeEmpty();
		sut.GetRequirements().Length.Should().Be(1);
		sut.GetRequirements().Should().Contain(x => x == requirement);
		sut.GetExemptions().Should().BeEmpty();
	}

	[Fact]
	public void Unless_And_ShouldAddMultipleExemptions()
	{
		Exemption exemption1 = Exemption.FromPredicate(_ => true);
		Exemption exemption2 = Exemption.FromPredicate(_ => false);
		DummyRule sut = new();

		sut.Unless(exemption1).And.Unless(exemption2);

		sut.GetFilters().Should().BeEmpty();
		sut.GetRequirements().Should().BeEmpty();
		sut.GetExemptions().Length.Should().Be(2);
		sut.GetExemptions().Should().Contain(x => x == exemption1);
		sut.GetExemptions().Should().Contain(x => x == exemption2);
	}

	[Fact]
	public void Unless_ShouldAddExemption()
	{
		Exemption exemption = Exemption.FromPredicate(_ => true);
		DummyRule sut = new();

		sut.Unless(exemption);

		sut.GetFilters().Should().BeEmpty();
		sut.GetRequirements().Should().BeEmpty();
		sut.GetExemptions().Length.Should().Be(1);
		sut.GetExemptions().Should().Contain(x => x == exemption);
	}

	internal class DummyRule : Rule<int>
	{
		/// <inheritdoc cref="Rule{TType}.Check" />
		public override IRuleCheck Check
			=> throw new NotSupportedException();

		public Exemption[] GetExemptions() => Exemptions.ToArray();
		public Filter<int>[] GetFilters() => Filters.ToArray();
		public Requirement<int>[] GetRequirements() => Requirements.ToArray();
	}
}
