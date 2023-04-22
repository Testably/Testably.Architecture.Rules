namespace Testably.Architecture.Testing;

public interface ITestResult
{
  TestError[] Errors { get; }
  bool IsSatisfied { get; }
}