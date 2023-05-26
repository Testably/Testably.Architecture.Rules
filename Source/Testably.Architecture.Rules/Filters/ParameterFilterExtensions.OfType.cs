using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class ParameterFilterExtensions
{
	/// <summary>
	///     Filter <see cref="ParameterInfo" />s that have an attribute of type <typeparamref name="TParameter" />.
	/// </summary>
	/// <param name="this">The <see cref="IParameterFilter{TResult}" />.</param>
	/// <param name="inherit">
	///     <see langword="true" /> to search the inheritance chain to find the attributes; otherwise,
	///     <see langword="false" />.<br />
	///     Defaults to <see langword="true" />
	/// </param>
	public static OfTypeOrderedFilterResult OfType<TParameter>(
		this IParameterFilter<IOrderedParameterFilterResult> @this,
		bool inherit = true)
	{
		OfTypeOrderedFilterResult filter = new(@this);
		filter.OrOfType<TParameter>(inherit);
		return filter;
	}

	/// <summary>
	///     Filter <see cref="ParameterInfo" />s that have an attribute of type <typeparamref name="TParameter" />.
	/// </summary>
	/// <param name="this">The <see cref="IParameterFilter{TResult}" />.</param>
	/// <param name="inherit">
	///     <see langword="true" /> to search the inheritance chain to find the attributes; otherwise,
	///     <see langword="false" />.<br />
	///     Defaults to <see langword="true" />
	/// </param>
	public static OfTypeUnorderedFilterResult OfType<TParameter>(
		this IParameterFilter<IUnorderedParameterFilterResult> @this,
		bool inherit = true)
	{
		OfTypeUnorderedFilterResult filter = new(@this);
		filter.OrOfType<TParameter>(inherit);
		return filter;
	}

	/// <summary>
	///     Add additional filters on a <see cref="ParameterInfo" /> which has an attribute.
	/// </summary>
	public class OfTypeUnorderedFilterResult : Filter.OnParameter<IUnorderedParameterFilterResult>,
		IUnorderedParameterFilterResult
	{
		private readonly IParameterFilter<IUnorderedParameterFilterResult> _typeFilter;

		internal OfTypeUnorderedFilterResult(
			IParameterFilter<IUnorderedParameterFilterResult> typeFilter) : base(typeFilter)
		{
			_typeFilter = typeFilter;
		}

		/// <summary>
		///     Adds another filter <see cref="ParameterInfo" />s for an attribute of type <typeparamref name="TParameter" />.
		/// </summary>
		/// <param name="inherit">
		///     <see langword="true" /> to search the inheritance chain to find the attributes; otherwise,
		///     <see langword="false" />.<br />
		///     Defaults to <see langword="true" />
		/// </param>
		public OfTypeUnorderedFilterResult OrOfType<TParameter>(
			bool inherit = true)
		{
			Predicates.Add(Filter.FromPredicate<ParameterInfo>(
				parameterInfo
					=> parameterInfo.ParameterType.IsOrInheritsFrom(typeof(TParameter), !inherit),
				$"parameter is of type {typeof(TParameter).Name}"));
			return this;
		}

		/// <inheritdoc />
		public bool Apply(ParameterInfo[] parameterInfos)
			=> _typeFilter
				.Which(
					Filter.FromPredicate<ParameterInfo>(
						parameterInfo => Predicates.Any(predicate => predicate.Applies(parameterInfo))))
				.Apply(parameterInfos);

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> $"{_typeFilter} and ({string.Join(" or ", Predicates)})";
	}

	/// <summary>
	///     Add additional filters on a <see cref="ParameterInfo" /> which has an attribute.
	/// </summary>
	public class OfTypeOrderedFilterResult : Filter.OnParameter<IOrderedParameterFilterResult>,
		IOrderedParameterFilterResult
	{
		private readonly IParameterFilter<IOrderedParameterFilterResult> _typeFilter;

		internal OfTypeOrderedFilterResult(
			IParameterFilter<IOrderedParameterFilterResult> typeFilter) : base(typeFilter)
		{
			_typeFilter = typeFilter;
		}

		/// <summary>
		///     Adds another filter <see cref="ParameterInfo" />s for an attribute of type <typeparamref name="TParameter" />.
		/// </summary>
		/// <param name="inherit">
		///     <see langword="true" /> to search the inheritance chain to find the attributes; otherwise,
		///     <see langword="false" />.<br />
		///     Defaults to <see langword="true" />
		/// </param>
		public OfTypeOrderedFilterResult OrOfType<TParameter>(
			bool inherit = true)
		{
			Predicates.Add(Filter.FromPredicate<ParameterInfo>(
				parameterInfo
					=> parameterInfo.ParameterType.InheritsFrom(typeof(TParameter), !inherit),
				$"parameter of type {typeof(TParameter).Name}"));
			return this;
		}

		/// <inheritdoc />
		public IParameterFilter<IOrderedParameterFilterResult> Then
			=> _typeFilter;

		/// <inheritdoc />
		public bool Apply(ParameterInfo[] parameterInfos)
			=> ApplyAny(parameterInfos);

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> $"{_typeFilter} and ({string.Join(" or ", Predicates)})";
	}
}
