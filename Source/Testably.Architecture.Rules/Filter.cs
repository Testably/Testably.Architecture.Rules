using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Testably.Architecture.Rules;

/// <summary>
///     Filters to apply in the architecture rule.
/// </summary>
public static class Filter
{
	/// <summary>
	///     Creates a new <see cref="Filter{TType}" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Filter<TType> FromPredicate<TType>(Expression<Func<TType, bool>> predicate)
	{
		Func<TType, bool> compiledPredicate = predicate.Compile();
		return new GenericFilter<TType>(compiledPredicate, predicate.ToString());
	}

	/// <summary>
	///     Creates a new <see cref="Filter{TType}" /> from the given <paramref name="predicate" />.
	/// </summary>
	public static Filter<TType> FromPredicate<TType>(Func<TType, bool> predicate, string name)
		=> new GenericFilter<TType>(predicate, name);

	private sealed class GenericFilter<TType> : Filter<TType>
	{
		private readonly Func<TType, bool> _filter;
		private readonly string _name;

		public GenericFilter(Func<TType, bool> filter, string name)
		{
			_filter = filter;
			_name = name;
		}

		/// <inheritdoc cref="Filter{TType}.Applies(TType)" />
		public override bool Applies(TType type)
			=> _filter(type);

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> _name;
	}

	/// <summary>
	///     Base class for additional filters on <see cref="ConstructorInfo" />.
	/// </summary>
	public abstract class OnConstructor : Filter<ConstructorInfo>, IConstructorFilterResult
	{
		/// <summary>
		///     The list of predicates.
		/// </summary>
		protected readonly List<Filter<ConstructorInfo>> Predicates = new();

		/// <summary>
		///     Initializes a new instance of <see cref="OnConstructor" />.
		/// </summary>
		protected OnConstructor(IConstructorFilter typeFilter)
		{
			typeFilter.Which(this);
			And = typeFilter;
		}

		#region IConstructorFilterResult Members

		/// <inheritdoc cref="IConstructorFilterResult.And" />
		public IConstructorFilter And { get; }

		/// <inheritdoc />
		public Filter<Type> ToTypeFilter()
		{
			return FromPredicate<Type>(
				t => Predicates.Any(p => t.GetConstructors().Any(p.Applies)), ToString());
		}

		#endregion

		/// <inheritdoc cref="Filter{T}.Applies(T)" />
		public override bool Applies(ConstructorInfo type)
			=> Predicates.Any(p => p.Applies(type));

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> string.Join(" or ", Predicates.Select(x => x.ToString()));
	}

	/// <summary>
	///     Base class for additional filters on <see cref="EventInfo" />.
	/// </summary>
	public abstract class OnEvent : Filter<EventInfo>, IEventFilterResult
	{
		/// <summary>
		///     The list of predicates.
		/// </summary>
		protected readonly List<Filter<EventInfo>> Predicates = new();

		/// <summary>
		///     Initializes a new instance of <see cref="OnEvent" />.
		/// </summary>
		protected OnEvent(IEventFilter typeFilter)
		{
			typeFilter.Which(this);
			And = typeFilter;
		}

		#region IEventFilterResult Members

		/// <inheritdoc cref="IEventFilterResult.And" />
		public IEventFilter And { get; }

		/// <inheritdoc />
		public Filter<Type> ToTypeFilter()
		{
			return FromPredicate<Type>(
				t => Predicates.Any(p => t.GetEvents().Any(p.Applies)));
		}

		#endregion

		/// <inheritdoc cref="Filter{T}.Applies(T)" />
		public override bool Applies(EventInfo type)
			=> Predicates.Any(p => p.Applies(type));

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> string.Join(" or ", Predicates.Select(x => x.ToString()));
	}

	/// <summary>
	///     Base class for additional filters on <see cref="FieldInfo" />.
	/// </summary>
	public abstract class OnField : Filter<FieldInfo>, IFieldFilterResult
	{
		/// <summary>
		///     The list of predicates.
		/// </summary>
		protected readonly List<Filter<FieldInfo>> Predicates = new();

		/// <summary>
		///     Initializes a new instance of <see cref="OnField" />.
		/// </summary>
		protected OnField(IFieldFilter typeFilter)
		{
			typeFilter.Which(this);
			And = typeFilter;
		}

		#region IFieldFilterResult Members

		/// <inheritdoc cref="IFieldFilterResult.And" />
		public IFieldFilter And { get; }

		/// <inheritdoc />
		public Filter<Type> ToTypeFilter()
		{
			return FromPredicate<Type>(
				t => Predicates.Any(p => t.GetFields().Any(p.Applies)));
		}

		#endregion

		/// <inheritdoc cref="Filter{T}.Applies(T)" />
		public override bool Applies(FieldInfo type)
			=> Predicates.Any(p => p.Applies(type));

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> string.Join(" or ", Predicates.Select(x => x.ToString()));
	}

	/// <summary>
	///     Base class for additional filters on <see cref="MethodInfo" />.
	/// </summary>
	public abstract class OnMethod : Filter<MethodInfo>, IMethodFilterResult
	{
		/// <summary>
		///     The list of predicates.
		/// </summary>
		protected readonly List<Filter<MethodInfo>> Predicates = new();

		/// <summary>
		///     Initializes a new instance of <see cref="OnMethod" />.
		/// </summary>
		protected OnMethod(IMethodFilter typeFilter)
		{
			typeFilter.Which(this);
			And = typeFilter;
		}

		#region IMethodFilterResult Members

		/// <inheritdoc cref="IMethodFilterResult.And" />
		public IMethodFilter And { get; }

		/// <inheritdoc />
		public Filter<Type> ToTypeFilter()
		{
			return FromPredicate<Type>(
				t => Predicates.Any(p => t.GetMethods().Any(p.Applies)),
				ToString());
		}

		#endregion

		/// <inheritdoc cref="Filter{T}.Applies(T)" />
		public override bool Applies(MethodInfo type)
			=> Predicates.Any(p => p.Applies(type));

		/// <inheritdoc />
		public IRequirementResult<MethodInfo> ShouldSatisfy(Requirement<MethodInfo> requirement)
			=> throw new NotImplementedException();

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> string.Join(" or ", Predicates.Select(x => x.ToString()));
	}

	/// <summary>
	///     Base class for additional filters on <see cref="ParameterInfo" />.
	/// </summary>
	public abstract class OnParameter<TResult> : Filter<ParameterInfo>,
		IParameterFilterResult<TResult>
		where TResult : IParameterFilterResult<TResult>
	{
		/// <summary>
		///     The list of predicates.
		/// </summary>
		protected readonly List<Filter<ParameterInfo>> Predicates = new();

		/// <summary>
		///     Initializes a new instance of <see cref="OnParameter{TResult}" />.
		/// </summary>
		protected OnParameter(
			IParameterFilter<TResult> typeFilter)
		{
			And = typeFilter;
		}

		#region IParameterFilterResult<TResult> Members

		/// <inheritdoc cref="IParameterFilterResult{TResult}.And" />
		public IParameterFilter<TResult> And { get; }

		/// <inheritdoc cref="IParameterFilterResult{TResult}.FriendlyName()" />
		public abstract string FriendlyName();

		#endregion

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> string.Join(" or ", Predicates.Select(x => x.ToString()));
	}

	/// <summary>
	///     Base class for additional filters on <see cref="PropertyInfo" />.
	/// </summary>
	public abstract class OnProperty : Filter<PropertyInfo>, IPropertyFilterResult
	{
		/// <summary>
		///     The list of predicates.
		/// </summary>
		protected readonly List<Filter<PropertyInfo>> Predicates = new();

		/// <summary>
		///     Initializes a new instance of <see cref="OnProperty" />.
		/// </summary>
		protected OnProperty(IPropertyFilter typeFilter)
		{
			typeFilter.Which(this);
			And = typeFilter;
		}

		#region IPropertyFilterResult Members

		/// <inheritdoc cref="IPropertyFilterResult.And" />
		public IPropertyFilter And { get; }

		/// <inheritdoc />
		public Filter<Type> ToTypeFilter()
		{
			return FromPredicate<Type>(
				t => Predicates.Any(p => t.GetProperties().Any(p.Applies)));
		}

		#endregion

		/// <inheritdoc cref="Filter{T}.Applies(T)" />
		public override bool Applies(PropertyInfo type)
			=> Predicates.Any(p => p.Applies(type));

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> string.Join(" or ", Predicates.Select(x => x.ToString()));
	}

	/// <summary>
	///     Base class for additional filters on <see cref="Type" />.
	/// </summary>
	public abstract class OnType : Filter<Type>, ITypeFilterResult
	{
		/// <summary>
		///     The list of predicates.
		/// </summary>
		protected readonly List<Func<Type, bool>> Predicates = new();

		private readonly ITypeFilterResult _filtered;

		private readonly ITypeFilter _typeFilter;

		/// <summary>
		///     Initializes a new instance of <see cref="OnType" />.
		/// </summary>
		protected OnType(
			ITypeFilter typeFilter,
			Func<Type, bool> predicate)
		{
			_typeFilter = typeFilter;
			Predicates.Add(predicate);
			_filtered = _typeFilter.Which(this);
		}

		#region ITypeFilterResult Members

		/// <inheritdoc cref="ITypeFilterResult.And" />
		public ITypeFilter And => _typeFilter;

		/// <inheritdoc cref="ITypeFilterResult.Assemblies" />
		public IAssemblyExpectation Assemblies
			=> _filtered.Assemblies;

		/// <inheritdoc cref="IRequirement{Type}.ShouldSatisfy(Requirement{Type})" />
		public IRequirementResult<Type> ShouldSatisfy(Requirement<Type> requirement)
			=> _filtered.ShouldSatisfy(requirement);

		#endregion

		/// <inheritdoc cref="Filter{T}.Applies(T)" />
		public override bool Applies(Type type)
			=> Predicates.Any(p => p(type));
	}
}

/// <summary>
///     Filter for <typeparamref name="TType" />.
/// </summary>
public abstract class Filter<TType>
{
	/// <summary>
	///     Specifies if the filter applies to the given <typeparamref name="TType" />.
	/// </summary>
	public abstract bool Applies(TType type);
}
