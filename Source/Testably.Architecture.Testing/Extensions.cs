﻿using System.Text.RegularExpressions;

namespace Testably.Architecture.Testing;

/// <summary>
///     Extension methods for architecture testing
/// </summary>
public static partial class Extensions
{
	/// <remarks>
	///     <see href="https://stackoverflow.com/a/30300521" />
	/// </remarks>
	internal static string WildcardToRegular(string value, bool ignoreCase)
	{
		string regex = "^" +
		               Regex.Escape(value).Replace("\\?", ".").Replace("\\*", ".*") + "$";
		if (ignoreCase)
		{
			regex = $"(?i){regex}";
		}

		return regex;
	}
}