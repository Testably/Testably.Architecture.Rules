using System.Text.RegularExpressions;

namespace Testably.Architecture.Testing.Internal;

internal static class Helpers
{
	/// <remarks>
	///     <see href="https://stackoverflow.com/a/30300521" />
	/// </remarks>
	internal static string WildcardToRegular(string value)
	{
		string regex = Regex.Escape(value).Replace("\\?", ".").Replace("\\*", ".*");
		return $"^{regex}$";
	}
}
