namespace Testably.Architecture.Rules;

/// <summary>
///     This test error is added when the filtered source collection is empty.
/// </summary>
public class EmptySourceTestError : TestError
{
	/// <summary>
	///     Creates an instance of <see cref="EmptySourceTestError" /> with the given <paramref name="errorMessage" />.
	/// </summary>
	public EmptySourceTestError(string errorMessage)
		: base(errorMessage)
	{
	}
}
