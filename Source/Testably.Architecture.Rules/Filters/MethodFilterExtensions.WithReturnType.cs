using System;
using System.Reflection;

namespace Testably.Architecture.Rules;

public static partial class MethodFilterExtensions
{
	/// <summary>
	///     Filter for <see cref="MethodInfo" /> that has a return type of <typeparamref name="TReturnType" />.<br />
	///     If <paramref name="allowDerivedType" /> is set to <see langword="true" />, also derived types are supported.
	/// </summary>
	/// <param name="this">The <see cref="IMethodFilter" />.</param>
	/// <param name="allowDerivedType">
	///     <see langword="true" /> to also allow derived types of <typeparamref name="TReturnType" />;
	///     otherwise <see langword="false" />.<br />
	///     Defaults to <see langword="false" />.
	/// </param>
	public static WithReturnTypeFilterResult WithReturnType<TReturnType>(
		this IMethodFilter @this,
		bool allowDerivedType = false)
	{
		return @this.WithReturnType(typeof(TReturnType), allowDerivedType);
	}

	/// <summary>
	///     Filter for <see cref="MethodInfo" /> that has a return type of <paramref name="returnType" />.<br />
	///     If <paramref name="allowDerivedType" /> is set to <see langword="true" />, also derived types are supported.
	/// </summary>
	/// <param name="this">The <see cref="IMethodFilter" />.</param>
	/// <param name="returnType">The <see cref="Type" /> of the return value.</param>
	/// <param name="allowDerivedType">
	///     <see langword="true" /> to also allow derived types of <paramref name="returnType" />;
	///     otherwise <see langword="false" />.<br />
	///     Defaults to <see langword="false" />.
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
	///     Add additional filters on a <see cref="MethodInfo" /> with return type.
	/// </summary>
	public class WithReturnTypeFilterResult : Filter.OnMethod
	{
		internal WithReturnTypeFilterResult(
			IMethodFilter typeFilter) : base(typeFilter)
		{
		}

		/// <summary>
		///     Adds another possible <typeparamref name="TReturnType" /> for the <see cref="MethodInfo" />.<br />
		///     If <paramref name="allowDerivedType" /> is set to <see langword="true" />, also derived types are supported.
		/// </summary>
		/// <param name="allowDerivedType">
		///     <see langword="true" /> to also allow derived types of <typeparamref name="TReturnType" />;
		///     otherwise <see langword="false" />.<br />
		///     Defaults to <see langword="false" />.
		/// </param>
		public WithReturnTypeFilterResult OrReturnType<TReturnType>(
			bool allowDerivedType = false)
		{
			return OrReturnType(typeof(TReturnType), allowDerivedType);
		}

		/// <summary>
		///     Adds another possible <paramref name="returnType" /> for the <see cref="MethodInfo" />.<br />
		///     If <paramref name="allowDerivedType" /> is set to <see langword="true" />, also derived types are supported.
		/// </summary>
		/// <param name="returnType">The <see cref="Type" /> of the return value.</param>
		/// <param name="allowDerivedType">
		///     <see langword="true" /> to also allow derived types of <paramref name="returnType" />;
		///     otherwise <see langword="false" />.<br />
		///     Defaults to <see langword="false" />.
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
