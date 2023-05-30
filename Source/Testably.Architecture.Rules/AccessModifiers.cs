using System;

namespace Testably.Architecture.Rules;

/// <summary>
///     The access modifiers.<br />
///     <see href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers" />
/// </summary>
[Flags]
public enum AccessModifiers
{
	/// <summary>
	///     Internal access modifier.<br />
	///     <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/internal" />
	/// </summary>
	Internal = 1,

	/// <summary>
	///     Protected access modifier.<br />
	///     <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/protected" />
	/// </summary>
	Protected = 2,

	/// <summary>
	///     Private access modifier.<br />
	///     <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/private" />
	/// </summary>
	Private = 4,

	/// <summary>
	///     Public access modifier.<br />
	///     <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/public" />
	/// </summary>
	Public = 8,
}
