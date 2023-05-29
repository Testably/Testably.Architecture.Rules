using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class FieldFilterExtensions
{
	/// <summary>
	///     Filter <see cref="FieldInfo" />s that are of type <typeparamref name="TField" />.
	/// </summary>
	/// <param name="this">The <see cref="IFieldFilter" />.</param>
	/// <param name="allowDerivedType">
	///     <see langword="true" /> to also allow derived types; otherwise, <see langword="false" />.<br />
	///     Defaults to <see langword="false" />
	/// </param>
	public static OfTypeFilterResult OfType<TField>(
		this IFieldFilter @this,
		bool allowDerivedType = false)
	{
		return @this.OfType(typeof(TField), allowDerivedType);
	}

	/// <summary>
	///     Filter <see cref="FieldInfo" />s that are of type <paramref name="fieldType" />.
	/// </summary>
	/// <param name="this">The <see cref="IFieldFilter" />.</param>
	/// <param name="fieldType">The type of the field.</param>
	/// <param name="allowDerivedType">
	///     <see langword="true" /> to also allow derived types; otherwise, <see langword="false" />.<br />
	///     Defaults to <see langword="false" />
	/// </param>
	public static OfTypeFilterResult OfType(
		this IFieldFilter @this,
		Type fieldType,
		bool allowDerivedType = false)
	{
		OfTypeFilterResult filter = new(@this);
		filter.OrOfType(fieldType, allowDerivedType);
		return filter;
	}

	/// <summary>
	///     Add additional filters on a <see cref="FieldInfo" /> which are of type.
	/// </summary>
	public class OfTypeFilterResult : Filter.OnField
	{
		internal OfTypeFilterResult(IFieldFilter fieldFilter)
			: base(fieldFilter)
		{
		}

		/// <summary>
		///     Adds another filter <see cref="FieldInfo" />s for a type <typeparamref name="TField" />.
		/// </summary>
		/// <param name="allowDerivedType">
		///     <see langword="true" /> to also allow derived types; otherwise, <see langword="false" />.<br />
		///     Defaults to <see langword="false" />
		/// </param>
		public OfTypeFilterResult OrOfType<TField>(
			bool allowDerivedType = false)
		{
			return OrOfType(typeof(TField), allowDerivedType);
		}

		/// <summary>
		///     Adds another filter <see cref="FieldInfo" />s for a type <paramref name="fieldType" />.
		/// </summary>
		/// <param name="fieldType">The type of the field.</param>
		/// <param name="allowDerivedType">
		///     <see langword="true" /> to also allow derived types; otherwise, <see langword="false" />.<br />
		///     Defaults to <see langword="false" />
		/// </param>
		public OfTypeFilterResult OrOfType(
			Type fieldType,
			bool allowDerivedType = false)
		{
			Predicates.Add(Filter.FromPredicate<FieldInfo>(
				fieldInfo => fieldInfo.FieldType
					.IsOrInheritsFrom(fieldType, !allowDerivedType),
				$"of type {fieldType.Name}"));
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
