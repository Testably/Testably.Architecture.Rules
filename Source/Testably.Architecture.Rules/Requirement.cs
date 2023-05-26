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
	public static Requirement<TType> Create<TType>(
		Func<TType, bool> predicate,
		Func<TType, TestError> errorGenerator)
		=> new GenericRequirement<TType>(predicate, errorGenerator);

	/// <summary>
	///     Creates a new <see cref="Requirement{TType}" /> by delegating it for all <typeparamref name="TDelegate" />
	///     generated
	///     by the <paramref name="delegateGenerator" /> to the <paramref name="delegateRequirement" />.
	/// </summary>
	public static Requirement<TType> Delegate<TType, TDelegate>(
		Func<TType, IEnumerable<TDelegate>> delegateGenerator,
		Requirement<TDelegate> delegateRequirement)
		=> new DelegateRequirement<TType, TDelegate>(delegateGenerator, delegateRequirement);

	/// <summary>
	///     Creates a new <see cref="Requirement{Assembly}" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Requirement<Assembly> ForAssembly(Expression<Func<Assembly, bool>> predicate)
		=> new GenericRequirement<Assembly>(
			predicate.Compile(),
			assembly => new AssemblyTestError(assembly,
				$"Assembly '{assembly.GetName().Name}' should satisfy the required condition {predicate}."));

	/// <summary>
	///     Creates a new <see cref="Requirement{Assembly}" /> from the given <paramref name="predicate" />
	///     and <paramref name="errorGenerator" />.
	/// </summary>
	public static Requirement<Assembly> ForAssembly(
		Func<Assembly, bool> predicate,
		Func<Assembly, TestError> errorGenerator)
		=> new GenericRequirement<Assembly>(predicate, errorGenerator);

	/// <summary>
	///     Creates a new <see cref="Requirement{ConstructorInfo}" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Requirement<ConstructorInfo> ForConstructor(
		Expression<Func<ConstructorInfo, bool>> predicate)
		=> new GenericRequirement<ConstructorInfo>(
			predicate.Compile(),
			constructor => new ConstructorTestError(constructor,
				$"The constructor '{constructor.Name}' should satisfy the required condition {predicate}."));

	/// <summary>
	///     Creates a new <see cref="Requirement{ConstructorInfo}" /> from the given <paramref name="predicate" />
	///     and <paramref name="errorGenerator" />.
	/// </summary>
	public static Requirement<ConstructorInfo> ForConstructor(
		Func<ConstructorInfo, bool> predicate,
		Func<ConstructorInfo, TestError> errorGenerator)
		=> new GenericRequirement<ConstructorInfo>(predicate, errorGenerator);

	/// <summary>
	///     Creates a new <see cref="Requirement{EventInfo}" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Requirement<EventInfo> ForEvent(Expression<Func<EventInfo, bool>> predicate)
		=> new GenericRequirement<EventInfo>(
			predicate.Compile(),
			@event => new EventTestError(@event,
				$"The event '{@event.Name}' should satisfy the required condition {predicate}."));

	/// <summary>
	///     Creates a new <see cref="Requirement{EventInfo}" /> from the given <paramref name="predicate" />
	///     and <paramref name="errorGenerator" />.
	/// </summary>
	public static Requirement<EventInfo> ForEvent(
		Func<EventInfo, bool> predicate,
		Func<EventInfo, TestError> errorGenerator)
		=> new GenericRequirement<EventInfo>(predicate, errorGenerator);

	/// <summary>
	///     Creates a new <see cref="Requirement{FieldInfo}" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Requirement<FieldInfo> ForField(Expression<Func<FieldInfo, bool>> predicate)
		=> new GenericRequirement<FieldInfo>(
			predicate.Compile(),
			field => new FieldTestError(field,
				$"The field '{field.Name}' should satisfy the required condition {predicate}."));

	/// <summary>
	///     Creates a new <see cref="Requirement{FieldInfo}" /> from the given <paramref name="predicate" />
	///     and <paramref name="errorGenerator" />.
	/// </summary>
	public static Requirement<FieldInfo> ForField(
		Func<FieldInfo, bool> predicate,
		Func<FieldInfo, TestError> errorGenerator)
		=> new GenericRequirement<FieldInfo>(predicate, errorGenerator);

	/// <summary>
	///     Creates a new <see cref="Requirement{MethodInfo}" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Requirement<MethodInfo> ForMethod(Expression<Func<MethodInfo, bool>> predicate)
		=> new GenericRequirement<MethodInfo>(
			predicate.Compile(),
			method => new MethodTestError(method,
				$"The method '{method.Name}' should satisfy the required condition {predicate}."));

	/// <summary>
	///     Creates a new <see cref="Requirement{MethodInfo}" /> from the given <paramref name="predicate" />
	///     and <paramref name="errorGenerator" />.
	/// </summary>
	public static Requirement<MethodInfo> ForMethod(
		Func<MethodInfo, bool> predicate,
		Func<MethodInfo, TestError> errorGenerator)
		=> new GenericRequirement<MethodInfo>(predicate, errorGenerator);

	/// <summary>
	///     Creates a new <see cref="Requirement{PropertyInfo}" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Requirement<PropertyInfo> ForProperty(
		Expression<Func<PropertyInfo, bool>> predicate)
		=> new GenericRequirement<PropertyInfo>(
			predicate.Compile(),
			property => new PropertyTestError(property,
				$"The property '{property.Name}' should satisfy the required condition {predicate}."));

	/// <summary>
	///     Creates a new <see cref="Requirement{PropertyInfo}" /> from the given <paramref name="predicate" />
	///     and <paramref name="errorGenerator" />.
	/// </summary>
	public static Requirement<PropertyInfo> ForProperty(
		Func<PropertyInfo, bool> predicate,
		Func<PropertyInfo, TestError> errorGenerator)
		=> new GenericRequirement<PropertyInfo>(predicate, errorGenerator);

	/// <summary>
	///     Creates a new <see cref="Requirement{Type}" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Requirement<Type> ForType(Expression<Func<Type, bool>> predicate)
		=> new GenericRequirement<Type>(
			predicate.Compile(),
			type => new TypeTestError(type,
				$"The type '{type.Name}' should satisfy the required condition {predicate}."));

	/// <summary>
	///     Creates a new <see cref="Requirement{Type}" /> from the given <paramref name="predicate" />
	///     and <paramref name="errorGenerator" />.
	/// </summary>
	public static Requirement<Type> ForType(
		Func<Type, bool> predicate,
		Func<Type, TestError> errorGenerator)
		=> new GenericRequirement<Type>(predicate, errorGenerator);

	private sealed class DelegateRequirement<TType, TDelegate> : Requirement<TType>
	{
		private readonly Func<TType, IEnumerable<TDelegate>> _delegateGenerator;
		private readonly Requirement<TDelegate> _delegateRequirement;

		public DelegateRequirement(
			Func<TType, IEnumerable<TDelegate>> delegateGenerator,
			Requirement<TDelegate> delegateRequirement)
		{
			_delegateGenerator = delegateGenerator;
			_delegateRequirement = delegateRequirement;
		}

		/// <inheritdoc cref="Requirement{TType}.CollectErrors" />
		public override void CollectErrors(TType type, List<TestError> errors)
		{
			foreach (TDelegate? @delegate in _delegateGenerator(type))
			{
				_delegateRequirement.CollectErrors(@delegate, errors);
			}
		}
	}

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
