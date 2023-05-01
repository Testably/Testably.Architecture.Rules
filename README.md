![Testably.Architecture.Testing](https://raw.githubusercontent.com/Testably/Testably.Architecture.Testing/main/Docs/Images/social-preview.png)  
[![Nuget](https://img.shields.io/nuget/v/Testably.Architecture.Testing)](https://www.nuget.org/packages/Testably.Architecture.Testing)
[![Build](https://github.com/Testably/Testably.Architecture.Testing/actions/workflows/build.yml/badge.svg)](https://github.com/Testably/Testably.Architecture.Testing/actions/workflows/build.yml)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/5b9b2f79950447a69d69037b43acd590)](https://www.codacy.com/gh/Testably/Testably.Architecture.Testing/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Testably/Testably.Architecture.Testing&amp;utm_campaign=Badge_Grade)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Testably_Testably.Architecture.Testing&branch=main&metric=coverage)](https://sonarcloud.io/summary/overall?id=Testably_Testably.Architecture.Testing&branch=main)
[![Mutation testing badge](https://img.shields.io/endpoint?style=flat&url=https%3A%2F%2Fbadge-api.stryker-mutator.io%2Fgithub.com%2FTestably%2FTestably.Architecture.Testing%2Fmain)](https://dashboard.stryker-mutator.io/reports/github.com/Testably/Testably.Architecture.Testing/main)

This library is used to define architecture rules as expectations that can be run and checked as part of the unit test execution.

## Example

- Test classes should be sealed:
  ```csharp
  [Fact]
  public void ExpectTestsToBeSealed()
  {
      var result = Expect.That
	      .AssembliesMatching("*.Test")
          .Types.ShouldBeSealed();
  
      Assert.Empty(result.Errors);
  }
  ```
