using FluentAssertions;
using Testably.Architecture.Rules.Tests.TestHelpers;
using Xunit;

namespace Testably.Architecture.Rules.Tests.Filters;

public sealed partial class TypeFilterExtensionsTests
{
	public sealed class AreAbstractTests
	{
		[Fact]
		public void WhichAreAbstract_ShouldFilterForAbstracTEntitys()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(AbstractClass), typeof(NotAbstractClass)).And;

			ITypeFilterResult sut = source.WhichAreAbstract();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is abstract");
			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(AbstractClass).FullName);
		}

		[Fact]
		public void WhichAreNotAbstract_ShouldFilterForNotAbstracTEntitys()
		{
			ITypeFilter source = Expect.That.Types
				.WhichAre(typeof(AbstractClass), typeof(NotAbstractClass)).And;

			ITypeFilterResult sut = source.WhichAreNotAbstract();

			ITestResult result = sut
				.ShouldAlwaysFail()
				.Check.InAllLoadedAssemblies();
			sut.ToString().Should().Contain("is not abstract");
			result.Errors.Length.Should().Be(1);
			result.Errors[0].ToString().Should()
				.Contain(typeof(NotAbstractClass).FullName);
		}

		private abstract class AbstractClass
		{
		}

		private class NotAbstractClass
		{
		}
	}
}
