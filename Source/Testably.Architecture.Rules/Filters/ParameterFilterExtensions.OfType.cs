using System;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class ParameterFilterExtensions
{
	/// <summary>
	///     Filter <see cref="ParameterInfo" />s that are of type <typeparamref name="TParameter" />.
	/// </summary>
	/// <param name="this">The <see cref="IParameterFilter{TResult}" />.</param>
	/// <param name="allowDerivedType">
	///     <see langword="true" /> to also allow derived types; otherwise, <see langword="false" />.<br />
	///     Defaults to <see langword="false" />
	/// </param>
	public static OfTypeOrderedFilterResult OfType<TParameter>(
		this IParameterFilter<IOrderedParameterFilterResult> @this,
		bool allowDerivedType = false)
	{
		return @this.OfType(typeof(TParameter), allowDerivedType);
	}

	/// <summary>
	///     Filter <see cref="ParameterInfo" />s that are of type <paramref name="parameterType" />.
	/// </summary>
	/// <param name="this">The <see cref="IParameterFilter{TResult}" />.</param>
	/// <param name="parameterType">The type of the parameter.</param>
	/// <param name="allowDerivedType">
	///     <see langword="true" /> to also allow derived types; otherwise, <see langword="false" />.<br />
	///     Defaults to <see langword="false" />
	/// </param>
	public static OfTypeOrderedFilterResult OfType(
		this IParameterFilter<IOrderedParameterFilterResult> @this,
		Type parameterType,
		bool allowDerivedType = false)
	{
		OfTypeOrderedFilterResult filter = new(@this);
		filter.OrOfType(parameterType, allowDerivedType);
		return filter;
	}

	/// <summary>
	///     Filter <see cref="ParameterInfo" />s that are of type <typeparamref name="TParameter" />.
	/// </summary>
	/// <param name="this">The <see cref="IParameterFilter{TResult}" />.</param>
	/// <param name="allowDerivedType">
	///     <see langword="true" /> to also allow derived types; otherwise, <see langword="false" />.<br />
	///     Defaults to <see langword="false" />
	/// </param>
	public static OfTypeUnorderedFilterResult OfType<TParameter>(
		this IParameterFilter<IUnorderedParameterFilterResult> @this,
		bool allowDerivedType = false)
	{
		return @this.OfType(typeof(TParameter), allowDerivedType);
	}

	/// <summary>
	///     Filter <see cref="ParameterInfo" />s that are of type <paramref name="parameterType" />.
	/// </summary>
	/// <param name="this">The <see cref="IParameterFilter{TResult}" />.</param>
	/// <param name="parameterType">The type of the parameter.</param>
	/// <param name="allowDerivedType">
	///     <see langword="true" /> to also allow derived types; otherwise, <see langword="false" />.<br />
	///     Defaults to <see langword="false" />
	/// </param>
	public static OfTypeUnorderedFilterResult OfType(
		this IParameterFilter<IUnorderedParameterFilterResult> @this,
		Type parameterType,
		bool allowDerivedType = false)
	{
		OfTypeUnorderedFilterResult filter = new(@this);
		filter.OrOfType(parameterType, allowDerivedType);
		return filter;
	}

	/// <summary>
	///     Add additional filters on a <see cref="ParameterInfo" /> which are of type.
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

		#region IUnorderedParameterFilterResult Members

		/// <inheritdoc />
		public override string FriendlyName()
			=> _filterResult.FriendlyName();

		/// <inheritdoc />
		public bool Apply(ParameterInfo[] parameterInfos)
			=> _filterResult.Apply(parameterInfos);

		#endregion

		/// <inheritdoc cref="Filter{ParameterInfo}.Applies(ParameterInfo)" />
		public override bool Applies(ParameterInfo type)
			=> Predicates.Any(predicate => predicate.Applies(type));

		/// <summary>
		///     Adds another filter <see cref="ParameterInfo" />s for a type <typeparamref name="TParameter" />.
		/// </summary>
		/// <param name="allowDerivedType">
		///     <see langword="true" /> to also allow derived types; otherwise, <see langword="false" />.<br />
		///     Defaults to <see langword="false" />
		/// </param>
		public OfTypeUnorderedFilterResult OrOfType<TParameter>(
			bool allowDerivedType = false)
		{
			return OrOfType(typeof(TParameter), allowDerivedType);
		}

		/// <summary>
		///     Adds another filter <see cref="ParameterInfo" />s for a type <paramref name="parameterType" />.
		/// </summary>
		/// <param name="parameterType">The type of the parameter.</param>
		/// <param name="allowDerivedType">
		///     <see langword="true" /> to also allow derived types; otherwise, <see langword="false" />.<br />
		///     Defaults to <see langword="false" />
		/// </param>
		public OfTypeUnorderedFilterResult OrOfType(
			Type parameterType,
			bool allowDerivedType = false)
		{
			Predicates.Add(Filter.FromPredicate<ParameterInfo>(
				parameterInfo => parameterInfo.ParameterType
					.IsOrInheritsFrom(parameterType, !allowDerivedType),
				$"is of type {parameterType.Name}"));
			return this;
		}

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
		{
			string result = string.Join(" or ", Predicates);
			if (Predicates.Count > 1)
			{
				return $"({result})";
			}

			return result;
		}
	}

	/// <summary>
	///     Add additional filters on a <see cref="ParameterInfo" /> which are of type.
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

		#region IOrderedParameterFilterResult Members

		/// <inheritdoc cref="IOrderedParameterFilterResult.Apply(ParameterInfo[])" />
		public bool Apply(ParameterInfo[] parameterInfos)
			=> _filterResult.Apply(parameterInfos);

		/// <inheritdoc cref="IOrderedParameterFilterResult.At(int)" />
		public IParameterFilter<IOrderedParameterFilterResult> At(int position)
			=> _filterResult.At(position);

		/// <inheritdoc cref="IOrderedParameterFilterResult.Then()" />
		public IParameterFilter<IOrderedParameterFilterResult> Then()
			=> _filterResult.Then();

		/// <inheritdoc cref="IParameterFilterResult{TResult}.FriendlyName()" />
		public override string FriendlyName()
			=> _filterResult.FriendlyName();

		#endregion

		/// <inheritdoc cref="Filter{ParameterInfo}.Applies(ParameterInfo)" />
		public override bool Applies(ParameterInfo type)
			=> Predicates.Any(predicate => predicate.Applies(type));

		/// <summary>
		///     Adds another filter <see cref="ParameterInfo" />s for a type <typeparamref name="TParameter" />.
		/// </summary>
		/// <param name="allowDerivedType">
		///     <see langword="true" /> to also allow derived types; otherwise, <see langword="false" />.<br />
		///     Defaults to <see langword="false" />
		/// </param>
		public OfTypeOrderedFilterResult OrOfType<TParameter>(
			bool allowDerivedType = false)
		{
			return OrOfType(typeof(TParameter), allowDerivedType);
		}

		/// <summary>
		///     Adds another filter <see cref="ParameterInfo" />s for a type <paramref name="parameterType" />.
		/// </summary>
		/// <param name="parameterType">The type of the parameter.</param>
		/// <param name="allowDerivedType">
		///     <see langword="true" /> to also allow derived types; otherwise, <see langword="false" />.<br />
		///     Defaults to <see langword="false" />
		/// </param>
		public OfTypeOrderedFilterResult OrOfType(
			Type parameterType,
			bool allowDerivedType = false)
		{
			Predicates.Add(Filter.FromPredicate<ParameterInfo>(
				parameterInfo => parameterInfo.ParameterType
					.IsOrInheritsFrom(parameterType, !allowDerivedType),
				$"is of type {parameterType.Name}"));
			return this;
		}

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
		{
			string result = string.Join(" or ", Predicates);
			if (Predicates.Count > 1)
			{
				return $"({result})";
			}

			return result;
		}
	}
}
