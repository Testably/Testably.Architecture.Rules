namespace Testably.Architecture.Testing.TestErrors;

/// <summary>
///     Basic error for architectural tests.
///     <para />
///     This class can be inherited to include more details about the error.
/// </summary>
public class TestError
{
	private string _errorMessage;

	/// <summary>
	///     Creates an instance of <see cref="TestError" /> with the given <paramref name="errorMessage" />.
	/// </summary>
	public TestError(string errorMessage)
	{
		_errorMessage = errorMessage;
	}

	/// <summary>
	///     Implicitly converts a <see cref="TestError" /> to string by returning the error message.
	/// </summary>
	public static implicit operator string(TestError error) => error._errorMessage;

	/// <inheritdoc cref="object.ToString()" />
	public override string ToString() => _errorMessage;

	/// <summary>
	///     Updates the error message from derived <see cref="TestError" />s.
	/// </summary>
	/// <param name="errorMessage"></param>
	protected void UpdateMessage(string errorMessage) => _errorMessage = errorMessage;
}
