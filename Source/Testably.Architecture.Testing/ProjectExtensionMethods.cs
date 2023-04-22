using System.Globalization;

namespace Testably.Architecture.Testing
{
    public static class ProjectExtensionMethods
    {
        public static ITestResult ShouldNotHaveDependenciesOn(
          this IProjectExpectation @this, string assemblyNamePrefix, bool ignoreCase = false)
        {
            return @this.ShouldOnlyHaveDependenciesThatSatisfy(a =>
              a.Name?.StartsWith(assemblyNamePrefix,
                ignoreCase,
                CultureInfo.InvariantCulture) != true);
        }
    }
}