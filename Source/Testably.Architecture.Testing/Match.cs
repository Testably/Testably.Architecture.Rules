using System.Text.RegularExpressions;

namespace Testably.Architecture.Testing;

/// <summary>
///     Options to match a <see langword="string" /> against a pattern.
/// </summary>
public abstract class Match
{
	/// <summary>
	///     Matches the <paramref name="value" /> against the given match pattern.
	/// </summary>
	/// <param name="value">The value to match against the given pattern.</param>
	/// <param name="ignoreCase">Flag indicating if the match should be performed case sensitive or not.</param>
	/// <returns>
	///     <see langword="true" />, if the <paramref name="value" /> matches the pattern, otherwise
	///     <see langword="false" />.
	/// </returns>
	public abstract bool Matches(string? value, bool ignoreCase);

	/// <summary>
	///     Implicitly converts the <see langword="string" /> to a <see cref="Wildcard" />.<br />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </summary>
	public static implicit operator Match(string pattern) => Wildcard(pattern);

	/// <summary>
	///     A wildcard match.<br />
	///     Supports * to match zero or more characters and ? to match exactly one character.
	/// </summary>
	public static Match Wildcard(string pattern)
		=> new WildcardMatch(pattern);

	private sealed class WildcardMatch : Match
	{
		private readonly string _pattern;

		internal WildcardMatch(string pattern)
		{
			_pattern = WildcardToRegular(pattern);
		}

		/// <inheritdoc cref="Match.Matches(string, bool)" />
		public override bool Matches(string? value, bool ignoreCase)
		{
			if (value == null)
			{
				return false;
			}

			RegexOptions options = ignoreCase
				? RegexOptions.IgnoreCase
				: RegexOptions.None;
			return Regex.IsMatch(value, _pattern, options);
		}

		/// <remarks>
		///     <see href="https://stackoverflow.com/a/30300521" />
		/// </remarks>
		private static string WildcardToRegular(string value)
		{
			string regex = Regex.Escape(value).Replace("\\?", ".").Replace("\\*", ".*");
			return $"^{regex}$";
		}
	}
}
