namespace Testably.Architecture.Testing;

public class TestError
{
  private readonly string _errorMessage;

  public TestError(string errorMessage)
  {
    _errorMessage = errorMessage;
  }

  public static implicit operator string(TestError error)
  {
    return error._errorMessage;
  }

  /// <inheritdoc />
  public override string ToString()
  {
    return _errorMessage;
  }
}