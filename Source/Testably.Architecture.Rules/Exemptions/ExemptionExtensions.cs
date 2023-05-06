using System;

namespace Testably.Architecture.Rules;

/// <summary>
///     Generic extension methods for <see cref="IExemption{TType}" />.
/// </summary>
public static class ExemptionExtensions
{
	/// <summary>
	///     Defines an exception to rules by allowing the filter to return an empty source.
	/// </summary>
	/// <param name="this">The <see cref="IExemption{TType}" />.</param>
	public static IExemptionResult<TType> AllowEmpty<TType>(
		this IExemption<TType> @this)
		=> @this.Unless<TType, EmptySourceTestError>();

	/// <summary>
	///     Defines an exception to rules by allowing the filter to return an empty source.
	/// </summary>
	/// <param name="this">The <see cref="IExemption{TTestError}" />.</param>
	/// <param name="predicate">
	///     (optional) a predicate to further filter out test errors of type
	///     <typeparamref name="TTestError" />.
	/// </param>
	public static IExemptionResult<TType> Unless<TType, TTestError>(
		this IExemption<TType> @this,
		Func<TTestError, bool>? predicate = null)
		where TTestError : TestError
	{
		return @this.Unless(Exemption.For(predicate));
	}
}
