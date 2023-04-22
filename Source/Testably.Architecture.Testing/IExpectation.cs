namespace Testably.Architecture.Testing;

public interface IExpectation
{
  IProjectExpectation AllLoadedProjects();
  IProjectExpectation ProjectContaining<T>();
}