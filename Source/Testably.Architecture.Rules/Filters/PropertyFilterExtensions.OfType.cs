using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class PropertyFilterExtensions
{
	/// <summary>
	///     Filter <see cref="PropertyInfo" />s that are of type <typeparamref name="TProperty" />.
	/// </summary>
	/// <param name="this">The <see cref="IPropertyFilter" />.</param>
	/// <param name="allowDerivedType">
	///     <see langword="true" /> to also allow derived types; otherwise, <see langword="false" />.<br />
	///     Defaults to <see langword="false" />
	/// </param>
	public static OfTypeFilterResult OfType<TProperty>(
		this IPropertyFilter @this,
		bool allowDerivedType = false)
	{
		return @this.OfType(typeof(TProperty), allowDerivedType);
	}

	/// <summary>
	///     Filter <see cref="PropertyInfo" />s that are of type <paramref name="propertyType" />.
	/// </summary>
	/// <param name="this">The <see cref="IPropertyFilter" />.</param>
	/// <param name="propertyType">The type of the property.</param>
	/// <param name="allowDerivedType">
	///     <see langword="true" /> to also allow derived types; otherwise, <see langword="false" />.<br />
	///     Defaults to <see langword="false" />
	/// </param>
	public static OfTypeFilterResult OfType(
		this IPropertyFilter @this,
		Type propertyType,
		bool allowDerivedType = false)
	{
		OfTypeFilterResult filter = new(@this);
		filter.OrOfType(propertyType, allowDerivedType);
		return filter;
	}

	/// <summary>
	///     Add additional filters on a <see cref="PropertyInfo" /> which are of type.
	/// </summary>
	public class OfTypeFilterResult : Filter.OnProperty
	{
		internal OfTypeFilterResult(IPropertyFilter propertyFilter)
			: base(propertyFilter)
		{
		}

		/// <summary>
		///     Adds another filter <see cref="PropertyInfo" />s for a type <typeparamref name="TProperty" />.
		/// </summary>
		/// <param name="allowDerivedType">
		///     <see langword="true" /> to also allow derived types; otherwise, <see langword="false" />.<br />
		///     Defaults to <see langword="false" />
		/// </param>
		public OfTypeFilterResult OrOfType<TProperty>(
			bool allowDerivedType = false)
		{
			return OrOfType(typeof(TProperty), allowDerivedType);
		}

		/// <summary>
		///     Adds another filter <see cref="PropertyInfo" />s for a type <paramref name="propertyType" />.
		/// </summary>
		/// <param name="propertyType">The type of the property.</param>
		/// <param name="allowDerivedType">
		///     <see langword="true" /> to also allow derived types; otherwise, <see langword="false" />.<br />
		///     Defaults to <see langword="false" />
		/// </param>
		public OfTypeFilterResult OrOfType(
			Type propertyType,
			bool allowDerivedType = false)
		{
			Predicates.Add(Filter.FromPredicate<PropertyInfo>(
				propertyInfo => propertyInfo.PropertyType
					.IsOrInheritsFrom(propertyType, !allowDerivedType),
				$"of type {propertyType.Name}"));
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
