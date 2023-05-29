using System;

namespace Testably.Architecture.Rules;

/// <summary>
///     Generic extension methods for <see cref="IExemption{TEntity}" />.
/// </summary>
public static class ExemptionExtensions
{
	/// <summary>
	///     Defines an exception to rules by allowing the filter to return an empty source.
	/// </summary>
	/// <param name="this">The <see cref="IExemption{TEntity}" />.</param>
	public static IExemptionResult<TEntity> AllowEmpty<TEntity>(
		this IExemption<TEntity> @this)
		=> @this.Unless<TEntity, EmptySourceTestError>();

	/// <summary>
	///     Defines an exception to rules by allowing the filter to return an empty source.
	/// </summary>
	/// <param name="this">The <see cref="IExemption{TTestError}" />.</param>
	/// <param name="predicate">
	///     (optional) a predicate to further filter out test errors of type
	///     <typeparamref name="TTestError" />.
	/// </param>
	public static IExemptionResult<TEntity> Unless<TEntity, TTestError>(
		this IExemption<TEntity> @this,
		Func<TTestError, bool>? predicate = null)
		where TTestError : TestError
	{
		return @this.Unless(Exemption.For(predicate));
	}
}
