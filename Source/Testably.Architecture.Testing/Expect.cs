using Testably.Architecture.Testing.Internal;

namespace Testably.Architecture.Testing
{
    public static class Expect
    {
      public static IExpectation That { get; } = new Expectation();
    }
}