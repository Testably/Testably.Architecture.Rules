using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Requirements to enforce as part of the architecture rule.
/// </summary>
public static class Requirement
{
	/// <summary>
	///     Creates a new <see cref="Requirement{TType}" /> from the given <paramref name="predicate" />
	///     and <paramref name="errorGenerator" />.
	/// </summary>
	public static Requirement<TType> Create<TType>(Func<TType, bool> predicate,
		Func<TType, TestError> errorGenerator)
		=> new GenericRequirement<TType>(predicate, errorGenerator);

	/// <summary>
	///     Creates a new <see cref="Requirement{Assembly}" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Requirement<Assembly> ForAssembly(Expression<Func<Assembly, bool>> predicate)
	{
		Func<Assembly, bool> compiledPredicate = predicate.Compile();
		return new GenericRequirement<Assembly>(compiledPredicate,
			assembly => new AssemblyTestError(assembly,
				$"Assembly '{assembly.GetName().Name}' should satisfy the required condition {predicate}."));
	}

	/// <summary>
	///     Creates a new <see cref="Requirement{Assembly}" /> from the given <paramref name="predicate" />
	///     and <paramref name="errorGenerator" />.
	/// </summary>
	public static Requirement<Assembly> ForAssembly(Func<Assembly, bool> predicate,
		Func<Assembly, TestError> errorGenerator)
		=> new GenericRequirement<Assembly>(predicate, errorGenerator);

	/// <summary>
	///     Creates a new <see cref="Requirement{Type}" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Requirement<Type> ForType(Expression<Func<Type, bool>> predicate)
	{
		Func<Type, bool> compiledPredicate = predicate.Compile();
		return new GenericRequirement<Type>(compiledPredicate,
			type => new TypeTestError(type,
				$"Type '{type.Name}' should satisfy the required condition {predicate}."));
	}

	/// <summary>
	///     Creates a new <see cref="Requirement{Type}" /> from the given <paramref name="predicate" />
	///     and <paramref name="errorGenerator" />.
	/// </summary>
	public static Requirement<Type> ForType(Func<Type, bool> predicate,
		Func<Type, TestError> errorGenerator)
		=> new GenericRequirement<Type>(predicate, errorGenerator);

	private sealed class GenericRequirement<TType> : Requirement<TType>
	{
		private readonly Func<TType, TestError> _errorGenerator;
		private readonly Func<TType, bool> _predicate;

		public GenericRequirement(
			Func<TType, bool> predicate,
			Func<TType, TestError> errorGenerator)
		{
			_predicate = predicate;
			_errorGenerator = errorGenerator;
		}

		/// <inheritdoc cref="Requirement{TType}.CollectErrors" />
		public override void CollectErrors(TType type, List<TestError> errors)
		{
			if (!_predicate(type))
			{
				errors.Add(_errorGenerator.Invoke(type));
			}
		}
	}
}

/// <summary>
///     Requirement for <typeparamref name="TType" />.
/// </summary>
public abstract class Requirement<TType>
{
	/// <summary>
	///     Specifies if the requirement applies to the given <typeparamref name="TType" />.
	/// </summary>
	public abstract void CollectErrors(TType type, List<TestError> errors);
}
