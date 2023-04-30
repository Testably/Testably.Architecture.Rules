using System.Text.RegularExpressions;

namespace Testably.Architecture.Testing.Internal;

internal static class WildcardHelpers
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
