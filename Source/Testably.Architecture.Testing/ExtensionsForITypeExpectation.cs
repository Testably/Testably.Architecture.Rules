namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for <see cref="ITypeExpectation" />.
/// </summary>
public static class ExtensionsForITypeExpectation
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
