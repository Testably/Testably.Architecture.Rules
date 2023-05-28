using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class MethodFilterExtensions
{
	/// <summary>
	///     Filter <see cref="MethodInfo" />s that have a return type of <typeparamref name="TReturnType" />.
	/// </summary>
	/// <param name="this">The <see cref="IMethodFilter" />.</param>
	/// <param name="allowDerivedType">
	///     <see langword="true" /> to search the inheritance chain to find the attributes; otherwise,
	///     <see langword="false" />.<br />
	///     Defaults to <see langword="true" />
	/// </param>
	public static WithReturnTypeFilterResult WithReturnType<TReturnType>(
		this IMethodFilter @this,
		bool allowDerivedType = false)
	{
		return @this.WithReturnType(typeof(TReturnType), allowDerivedType);
	}

	/// <summary>
	///     Filter <see cref="MethodInfo" />s that have a return type of <paramref name="returnType" />.
	/// </summary>
	/// <param name="this">The <see cref="IMethodFilter" />.</param>
	/// <param name="returnType">The <see cref="Type" /> of the return value.</param>
	/// <param name="allowDerivedType">
	///     <see langword="true" /> to also allow derived types of <paramref name="returnType" />;
	///     otherwise <see langword="false" />.<br />
	///     Defaults to <see langword="true" />.
	/// </param>
	public static WithReturnTypeFilterResult WithReturnType(
		this IMethodFilter @this,
		Type returnType,
		bool allowDerivedType = false)
	{
		WithReturnTypeFilterResult filter = new(
			@this);
		filter.OrReturnType(returnType, allowDerivedType);
		return filter;
	}

	/// <summary>
	///     Add additional filters on a <see cref="MethodInfo" /> which has an attribute.
	/// </summary>
	public class WithReturnTypeFilterResult : Filter.OnMethod
	{
		internal WithReturnTypeFilterResult(
			IMethodFilter typeFilter) : base(typeFilter)
		{
		}

		/// <summary>
		///     Adds another filter <see cref="MethodInfo" />s for an attribute of type <typeparamref name="TReturnType" />.
		/// </summary>
		/// <param name="allowDerivedType">
		///     <see langword="true" /> to also allow derived types of <typeparamref name="TReturnType" />;
		///     otherwise <see langword="false" />.<br />
		///     Defaults to <see langword="true" />.
		/// </param>
		public WithReturnTypeFilterResult OrReturnType<TReturnType>(
			bool allowDerivedType = false)
		{
			return OrReturnType(typeof(TReturnType), allowDerivedType);
		}

		/// <summary>
		///     Adds another filter <see cref="MethodInfo" />s for an attribute of type <paramref name="returnType" />.
		/// </summary>
		/// <param name="returnType">The <see cref="Type" /> of the return value.</param>
		/// <param name="allowDerivedType">
		///     <see langword="true" /> to also allow derived types of <paramref name="returnType" />;
		///     otherwise <see langword="false" />.<br />
		///     Defaults to <see langword="true" />.
		/// </param>
		public WithReturnTypeFilterResult OrReturnType(
			Type returnType,
			bool allowDerivedType = false)
		{
			Predicates.Add(Filter.FromPredicate<MethodInfo>(
				method => method.ReturnType.IsOrInheritsFrom(returnType, !allowDerivedType),
				$"with return type {returnType.Name}"));
			return this;
		}
	}
}
