using System.Collections.Generic;

namespace Testably.Architecture.Testing.Internal;

internal class ArchitectureResultBuilder
{
    private readonly List<TestError> _errors = new();

    public ArchitectureResultBuilder Add(ITestResult result)
    {
        _errors.AddRange(result.Errors);
        return this;
    }

    public ITestResult Build()
    {
        return new TestResult(_errors.ToArray());
    }
}