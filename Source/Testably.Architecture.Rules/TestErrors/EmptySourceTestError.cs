namespace Testably.Architecture.Rules;

/// <summary>
///     This test error is added when the filtered source collection is empty, unless
///     <see cref="ExemptionExtensions.AllowEmpty{TEntity}(IExemption{TEntity})" /> is called.
/// </summary>
public class EmptySourceTestError : TestError
{
	/// <summary>
	///     Creates an instance of <see cref="EmptySourceTestError" /> with the given <paramref name="errorMessage" />.
	/// </summary>
	internal EmptySourceTestError(string errorMessage)
		: base(errorMessage)
	{
	}
}
