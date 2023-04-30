namespace Testably.Architecture.Testing;

public static partial class Extensions
{
	/// <summary>
	///     Expect the type to be sealed.
	/// </summary>
	public static ITestResult<ITypeExpectation> ShouldBeSealed(
		this ITypeExpectation @this,
		bool isSealed = true)
	{
		return @this.ShouldSatisfy(t => t.IsSealed == isSealed);
	}
}
