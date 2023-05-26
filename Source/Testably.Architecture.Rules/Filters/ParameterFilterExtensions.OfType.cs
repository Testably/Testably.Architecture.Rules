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
		private readonly IUnorderedParameterFilterResult _filterResult;

		internal OfTypeUnorderedFilterResult(
			IParameterFilter<IUnorderedParameterFilterResult> typeFilter) : base(typeFilter)
		{
			_filterResult = typeFilter.Which(this);
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
				parameterInfo => parameterInfo.ParameterType.IsOrInheritsFrom(typeof(TParameter), !inherit),
				ToString()));
			return this;
		}

		/// <inheritdoc />
		public bool Apply(ParameterInfo[] parameterInfos)
			=> _filterResult.Apply(parameterInfos);

		/// <inheritdoc cref="Filter{ParameterInfo}.Applies(ParameterInfo)" />
		public override bool Applies(ParameterInfo type)
			=> Predicates.Any(predicate => predicate.Applies(type));

		/// <inheritdoc />
		public override string FriendlyName()
			=> _filterResult.FriendlyName();

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
		{
			return string.Join(" or ", Predicates);
		}
	}

	/// <summary>
	///     Add additional filters on a <see cref="ParameterInfo" /> which has an attribute.
	/// </summary>
	public class OfTypeOrderedFilterResult : Filter.OnParameter<IOrderedParameterFilterResult>,
		IOrderedParameterFilterResult
	{
		private readonly IOrderedParameterFilterResult _filterResult;

		internal OfTypeOrderedFilterResult(
			IParameterFilter<IOrderedParameterFilterResult> typeFilter) : base(typeFilter)
		{
			_filterResult = typeFilter.Which(this);
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
				$"is of type {typeof(TParameter).Name}"));
			return this;
		}

		/// <inheritdoc />
		public IParameterFilter<IOrderedParameterFilterResult> Then()
			=> _filterResult.Then();

		/// <inheritdoc />
		public bool Apply(ParameterInfo[] parameterInfos)
			=> _filterResult.Apply(parameterInfos);

		/// <inheritdoc />
		public override string FriendlyName()
			=> _filterResult.FriendlyName();

		/// <inheritdoc cref="Filter{ParameterInfo}.Applies(ParameterInfo)" />
		public override bool Applies(ParameterInfo type)
			=> Predicates.Any(predicate => predicate.Applies(type));

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
		{
			if (Predicates.Count > 0)
			{
				return $"({string.Join(" or ", Predicates)})";
			}
			return string.Join(" or ", Predicates);
		}
	}
}
