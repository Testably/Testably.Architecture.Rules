using System;
using System.Linq;

namespace Testably.Architecture.Testing.Internal;

internal class Expectation : IExpectation
{
    public IProjectExpectation AllLoadedProjects()
    {
        return new CompositeAssemblyContainer(AppDomain.CurrentDomain.GetAssemblies()
         .Select(x => new AssemblyContainer(x)));
    }

    public IProjectExpectation ProjectContaining<T>()
    {
        return new AssemblyContainer(typeof(T).Assembly);
    }
}