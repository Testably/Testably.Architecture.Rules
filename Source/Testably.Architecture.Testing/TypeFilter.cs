using System;

namespace Testably.Architecture.Testing;

/// <summary>
///     Filter for <see cref="Type" />s.
/// </summary>
public abstract class TypeFilter
{
	/// <summary>
	///     Specifies if the filter applies to the given <see cref="Type" />.
	/// </summary>
	public abstract bool Applies(Type type);

	/// <summary>
	///     Implicitly converts the <paramref name="filter" /> to a <see cref="TypeFilter" />.
	/// </summary>
	public static implicit operator TypeFilter(Func<Type, bool> filter)
		=> new GenericTypeFilter(filter);

	private sealed class GenericTypeFilter : TypeFilter
	{
		private readonly Func<Type, bool> _filter;

		public GenericTypeFilter(Func<Type, bool> filter)
		{
			_filter = filter;
		}

		/// <inheritdoc cref="TypeFilter.Applies(Type)" />
		public override bool Applies(Type type)
			=> _filter(type);
	}
}
