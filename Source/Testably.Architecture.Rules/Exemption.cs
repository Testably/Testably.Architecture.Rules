using System;
using System.Linq.Expressions;

namespace Testably.Architecture.Rules;

/// <summary>
///     Exemption for <see cref="TestError" />.
/// </summary>
public abstract class Exemption
{
	/// <summary>
	///     Specifies if the exemption applies to the given <see cref="TestError" />.
	/// </summary>
	public abstract bool Exempt(TestError type);

	/// <summary>
	///     Creates a new <see cref="Exemption" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Exemption For<TTestError>(Func<TTestError, bool>? predicate, string? name = null)
		where TTestError : TestError
	{
		predicate ??= _ => true;
		return new GenericExemption(
			testError => testError is TTestError value && predicate(value),
			name ?? $"Unless {typeof(TTestError).Name} matches predicate");
	}

	/// <summary>
	///     Creates a new <see cref="Exemption" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Exemption FromPredicate(Expression<Func<TestError, bool>> predicate)

	{
		Func<TestError, bool> compiledPredicate = predicate.Compile();
		return new GenericExemption(compiledPredicate, predicate.ToString());
	}

	/// <summary>
	///     Creates a new <see cref="Exemption" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Exemption FromPredicate(Func<TestError, bool> predicate, string name)
		=> new GenericExemption(predicate, name);

	private sealed class GenericExemption : Exemption
	{
		private readonly Func<TestError, bool> _exemption;
		private readonly string _name;

		public GenericExemption(Func<TestError, bool> exemption, string name)
		{
			_exemption = exemption;
			_name = name;
		}

		/// <inheritdoc cref="Exemption.Exempt(TestError)" />
		public override bool Exempt(TestError type)
			=> _exemption(type);

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> _name;
	}
}
