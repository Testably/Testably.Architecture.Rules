namespace Testably.Architecture.Testing.TestErrors;

/// <summary>
///     Basic error for architectural tests.
///     <para />
///     This class can be inherited to include more details about the error.
/// </summary>
public class TestError
{
	private readonly string _errorMessage;

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
	public static implicit operator string(TestError error)
	{
		return error._errorMessage;
	}

	/// <inheritdoc cref="object.ToString()" />
	public override string ToString()
	{
		return _errorMessage;
	}
}
