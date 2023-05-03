using System;

namespace Testably.Architecture.Testing;

/// <summary>
///     Filter for <typeparamref name="T"/>.
/// </summary>
public abstract class Filter<T>
{
	/// <summary>
	///     Specifies if the filter applies to the given <see cref="Type" />.
	/// </summary>
	public abstract bool Applies(T type);

	/// <summary>
	///     Implicitly converts the <paramref name="filter" /> to a <see cref="Filter{T}" />.
	/// </summary>
	public static implicit operator Filter<T>(Func<T, bool> filter)
		=> new GenericFilter(filter);

	private sealed class GenericFilter : Filter<T>
	{
		private readonly Func<T, bool> _filter;

		public GenericFilter(Func<T, bool> filter)
		{
			_filter = filter;
		}

		/// <inheritdoc cref="Filter{T}.Applies(T)" />
		public override bool Applies(T type)
			=> _filter(type);
	}
}
