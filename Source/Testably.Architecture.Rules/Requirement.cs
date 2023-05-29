using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Requirements define the conditions that the filtered entities must satisfy.
/// </summary>
public static class Requirement
{
	/// <summary>
	///     Creates a new <see cref="Requirement{TEntity}" /> from the given <paramref name="predicate" />
	///     and <paramref name="errorGenerator" />.
	///     <para />
	///     Whenever the <paramref name="predicate" /> returns false, the <see cref="TestError" />s from the
	///     <paramref name="errorGenerator" /> are added to the total list of errors.
	/// </summary>
	public static Requirement<TEntity> Create<TEntity>(
		Func<TEntity, bool> predicate,
		Func<TEntity, TestError> errorGenerator)
		=> new GenericRequirement<TEntity>(predicate, errorGenerator);

	/// <summary>
	///     Creates a new <see cref="Requirement{TEntity}" /> by delegating it to the <paramref name="delegateGenerator" />.
	///     <para />
	///     If any <typeparamref name="TDelegate" /> created by the <paramref name="delegateGenerator" /> fails the
	///     <paramref name="predicate" />, the <paramref name="errorGenerator" /> is used to create the
	///     <see cref="TestError" />s.
	/// </summary>
	public static Requirement<TEntity> DelegateAny<TEntity, TDelegate>(
		Func<TEntity, IEnumerable<TDelegate>> delegateGenerator,
		Func<TDelegate, bool> predicate,
		Func<TEntity, ICollection<TDelegate>, TestError> errorGenerator)
	{
		return new DelegateAnyRequirement<TEntity, TDelegate>(
			delegateGenerator,
			predicate,
			errorGenerator);
	}

	/// <summary>
	///     Creates a new <see cref="Requirement{Assembly}" /> from the given <paramref name="predicate" />.
	///     <para />
	///     Whenever the <paramref name="predicate" /> returns false, a single <see cref="AssemblyTestError" /> is added
	///     to the total list of errors.
	/// </summary>
	public static Requirement<Assembly> ForAssembly(Expression<Func<Assembly, bool>> predicate)
		=> new GenericRequirement<Assembly>(
			predicate.Compile(),
			assembly => new AssemblyTestError(assembly,
				$"Assembly '{assembly.GetName().Name}' should satisfy the required condition {predicate}."));

	/// <summary>
	///     Creates a new <see cref="Requirement{Assembly}" /> from the given <paramref name="predicate" />
	///     and <paramref name="errorGenerator" />.
	///     <para />
	///     Whenever the <paramref name="predicate" /> returns false, the <see cref="TestError" />s from the
	///     <paramref name="errorGenerator" /> are added to the total list of errors.
	/// </summary>
	public static Requirement<Assembly> ForAssembly(
		Func<Assembly, bool> predicate,
		Func<Assembly, TestError> errorGenerator)
		=> new GenericRequirement<Assembly>(predicate, errorGenerator);

	/// <summary>
	///     Creates a new <see cref="Requirement{ConstructorInfo}" /> from the given <paramref name="predicate" />.
	///     <para />
	///     Whenever the <paramref name="predicate" /> returns false, a single <see cref="ConstructorTestError" /> is added
	///     to the total list of errors.
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
	///     <para />
	///     Whenever the <paramref name="predicate" /> returns false, the <see cref="TestError" />s from the
	///     <paramref name="errorGenerator" /> are added to the total list of errors.
	/// </summary>
	public static Requirement<ConstructorInfo> ForConstructor(
		Func<ConstructorInfo, bool> predicate,
		Func<ConstructorInfo, TestError> errorGenerator)
		=> new GenericRequirement<ConstructorInfo>(predicate, errorGenerator);

	/// <summary>
	///     Creates a new <see cref="Requirement{EventInfo}" /> from the given <paramref name="predicate" />.
	///     <para />
	///     Whenever the <paramref name="predicate" /> returns false, a single <see cref="EventTestError" /> is added
	///     to the total list of errors.
	/// </summary>
	public static Requirement<EventInfo> ForEvent(Expression<Func<EventInfo, bool>> predicate)
		=> new GenericRequirement<EventInfo>(
			predicate.Compile(),
			@event => new EventTestError(@event,
				$"The event '{@event.Name}' should satisfy the required condition {predicate}."));

	/// <summary>
	///     Creates a new <see cref="Requirement{EventInfo}" /> from the given <paramref name="predicate" />
	///     and <paramref name="errorGenerator" />.
	///     <para />
	///     Whenever the <paramref name="predicate" /> returns false, the <see cref="TestError" />s from the
	///     <paramref name="errorGenerator" /> are added to the total list of errors.
	/// </summary>
	public static Requirement<EventInfo> ForEvent(
		Func<EventInfo, bool> predicate,
		Func<EventInfo, TestError> errorGenerator)
		=> new GenericRequirement<EventInfo>(predicate, errorGenerator);

	/// <summary>
	///     Creates a new <see cref="Requirement{FieldInfo}" /> from the given <paramref name="predicate" />.
	///     <para />
	///     Whenever the <paramref name="predicate" /> returns false, a single <see cref="FieldTestError" /> is added
	///     to the total list of errors.
	/// </summary>
	public static Requirement<FieldInfo> ForField(Expression<Func<FieldInfo, bool>> predicate)
		=> new GenericRequirement<FieldInfo>(
			predicate.Compile(),
			field => new FieldTestError(field,
				$"The field '{field.Name}' should satisfy the required condition {predicate}."));

	/// <summary>
	///     Creates a new <see cref="Requirement{FieldInfo}" /> from the given <paramref name="predicate" />
	///     and <paramref name="errorGenerator" />.
	///     <para />
	///     Whenever the <paramref name="predicate" /> returns false, the <see cref="TestError" />s from the
	///     <paramref name="errorGenerator" /> are added to the total list of errors.
	/// </summary>
	public static Requirement<FieldInfo> ForField(
		Func<FieldInfo, bool> predicate,
		Func<FieldInfo, TestError> errorGenerator)
		=> new GenericRequirement<FieldInfo>(predicate, errorGenerator);

	/// <summary>
	///     Creates a new <see cref="Requirement{MethodInfo}" /> from the given <paramref name="predicate" />.
	///     <para />
	///     Whenever the <paramref name="predicate" /> returns false, a single <see cref="MethodTestError" /> is added
	///     to the total list of errors.
	/// </summary>
	public static Requirement<MethodInfo> ForMethod(Expression<Func<MethodInfo, bool>> predicate)
		=> new GenericRequirement<MethodInfo>(
			predicate.Compile(),
			method => new MethodTestError(method,
				$"The method '{method.Name}' should satisfy the required condition {predicate}."));

	/// <summary>
	///     Creates a new <see cref="Requirement{MethodInfo}" /> from the given <paramref name="predicate" />
	///     and <paramref name="errorGenerator" />.
	///     <para />
	///     Whenever the <paramref name="predicate" /> returns false, the <see cref="TestError" />s from the
	///     <paramref name="errorGenerator" /> are added to the total list of errors.
	/// </summary>
	public static Requirement<MethodInfo> ForMethod(
		Func<MethodInfo, bool> predicate,
		Func<MethodInfo, TestError> errorGenerator)
		=> new GenericRequirement<MethodInfo>(predicate, errorGenerator);

	/// <summary>
	///     Creates a new <see cref="Requirement{PropertyInfo}" /> from the given <paramref name="predicate" />.
	///     <para />
	///     Whenever the <paramref name="predicate" /> returns false, a single <see cref="PropertyTestError" /> is added
	///     to the total list of errors.
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
	///     <para />
	///     Whenever the <paramref name="predicate" /> returns false, the <see cref="TestError" />s from the
	///     <paramref name="errorGenerator" /> are added to the total list of errors.
	/// </summary>
	public static Requirement<PropertyInfo> ForProperty(
		Func<PropertyInfo, bool> predicate,
		Func<PropertyInfo, TestError> errorGenerator)
		=> new GenericRequirement<PropertyInfo>(predicate, errorGenerator);

	/// <summary>
	///     Creates a new <see cref="Requirement{Type}" /> from the given <paramref name="predicate" />.
	///     <para />
	///     Whenever the <paramref name="predicate" /> returns false, a single <see cref="TypeTestError" /> is added
	///     to the total list of errors.
	/// </summary>
	public static Requirement<Type> ForType(Expression<Func<Type, bool>> predicate)
		=> new GenericRequirement<Type>(
			predicate.Compile(),
			type => new TypeTestError(type,
				$"The type '{type.Name}' should satisfy the required condition {predicate}."));

	/// <summary>
	///     Creates a new <see cref="Requirement{Type}" /> from the given <paramref name="predicate" />
	///     and <paramref name="errorGenerator" />.
	///     <para />
	///     Whenever the <paramref name="predicate" /> returns false, the <see cref="TestError" />s from the
	///     <paramref name="errorGenerator" /> are added to the total list of errors.
	/// </summary>
	public static Requirement<Type> ForType(
		Func<Type, bool> predicate,
		Func<Type, TestError> errorGenerator)
		=> new GenericRequirement<Type>(predicate, errorGenerator);

	private sealed class DelegateAnyRequirement<TEntity, TDelegate> : Requirement<TEntity>
	{
		private readonly Func<TEntity, IEnumerable<TDelegate>> _delegateGenerator;
		private readonly Func<TEntity, ICollection<TDelegate>, TestError> _errorGenerator;
		private readonly Func<TDelegate, bool> _predicate;

		public DelegateAnyRequirement(
			Func<TEntity, IEnumerable<TDelegate>> delegateGenerator,
			Func<TDelegate, bool> predicate,
			Func<TEntity, ICollection<TDelegate>, TestError> errorGenerator)
		{
			_delegateGenerator = delegateGenerator;
			_predicate = predicate;
			_errorGenerator = errorGenerator;
		}

		/// <inheritdoc cref="Requirement{TEntity}.CollectErrors" />
		public override void CollectErrors(TEntity type, List<TestError> errors)
		{
			List<TDelegate> delegates = _delegateGenerator(type).ToList();
			if (delegates.Any(_predicate))
			{
				return;
			}

			errors.Add(_errorGenerator.Invoke(type, delegates));
		}
	}

	private sealed class GenericRequirement<TEntity> : Requirement<TEntity>
	{
		private readonly Func<TEntity, TestError> _errorGenerator;
		private readonly Func<TEntity, bool> _predicate;

		public GenericRequirement(
			Func<TEntity, bool> predicate,
			Func<TEntity, TestError> errorGenerator)
		{
			_predicate = predicate;
			_errorGenerator = errorGenerator;
		}

		/// <inheritdoc cref="Requirement{TEntity}.CollectErrors" />
		public override void CollectErrors(TEntity type, List<TestError> errors)
		{
			if (!_predicate(type))
			{
				errors.Add(_errorGenerator.Invoke(type));
			}
		}
	}
}

/// <summary>
///     Requirement for <typeparamref name="TEntity" />.
/// </summary>
public abstract class Requirement<TEntity>
{
	/// <summary>
	///     Specifies if the requirement applies to the given <typeparamref name="TEntity" />.
	/// </summary>
	public abstract void CollectErrors(TEntity type, List<TestError> errors);
}
