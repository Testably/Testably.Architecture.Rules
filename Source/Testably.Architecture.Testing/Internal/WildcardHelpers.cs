using System.Text.RegularExpressions;

namespace Testably.Architecture.Testing.Internal;

internal class WildcardHelpers
{
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
