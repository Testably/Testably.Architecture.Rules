using System;

namespace Testably.Architecture.Testing;

/// <summary>
///     Filter for <typeparamref name="TType" />.
/// </summary>
public abstract class Filter<TType>
{
	/// <summary>
	///     Specifies if the filter applies to the given <typeparamref name="TType" />.
	/// </summary>
	public abstract bool Applies(TType type);

	/// <summary>
	///     Implicitly converts the <paramref name="filter" /> to a <see cref="Filter{TType}" />.
	/// </summary>
	public static implicit operator Filter<TType>(Func<TType, bool> filter)
		=> new GenericFilter(filter);

	private sealed class GenericFilter : Filter<TType>
	{
		private readonly Func<TType, bool> _filter;

		public GenericFilter(Func<TType, bool> filter)
		{
			_filter = filter;
		}

		/// <inheritdoc cref="Filter{TType}.Applies(TType)" />
		public override bool Applies(TType type)
			=> _filter(type);
	}
}
