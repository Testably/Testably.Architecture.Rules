using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Testably.Architecture.Rules.Internal;

internal class TypeRule : Rule<Type>, ITypeExpectation, ITypeFilterResult
{
	/// <inheritdoc cref="IRule.Check" />
	public override IRuleCheck Check
		=> new RuleCheck<Type>(Filters, Requirements, Exemptions,
			_ => _.SelectMany(a => a.GetTypes()));

	public TypeRule(params Filter<Type>[] filters)
	{
		Filters.AddRange(filters);
	}

	#region ITypeExpectation Members

	/// <inheritdoc cref="ITypeFilter.Which(Filter{Type})" />
	public ITypeFilterResult Which(Filter<Type> filter)
	{
		Filters.Add(filter);
		return this;
	}

	#endregion

	#region ITypeFilterResult Members

	/// <inheritdoc cref="ITypeFilterResult.And" />
	public ITypeFilter And => this;

	/// <inheritdoc />
	public IAssemblyExpectation Assemblies
		=> new AssemblyRule(new TypeAssemblyFilter(Filters));

	/// <inheritdoc />
	public IConstructorExpectation Constructors
		=> new ConstructorRule(new TypeConstructorFilter(Filters));

	/// <inheritdoc />
	public IEventExpectation Events
		=> new EventRule(new TypeEventFilter(Filters));

	/// <inheritdoc />
	public IFieldExpectation Fields
		=> new FieldRule(new TypeFieldFilter(Filters));

	/// <inheritdoc />
	public IMethodExpectation Methods
		=> new MethodRule(new TypeMethodFilter(Filters));

	/// <inheritdoc />
	public IPropertyExpectation Properties
		=> new PropertyRule(new TypePropertyFilter(Filters));

	#endregion

	private sealed class TypeAssemblyFilter : Filter<Assembly>
	{
		private readonly List<Filter<Type>> _typeFilters;

		public TypeAssemblyFilter(List<Filter<Type>> typeFilters)
		{
			_typeFilters = typeFilters;
		}

		/// <inheritdoc cref="Filter{Assembly}.Applies(Assembly)" />
		public override bool Applies(Assembly assembly)
		{
			return assembly.GetTypes().Any(
				type => _typeFilters.All(
					filter => filter.Applies(type)));
		}

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> $"The assembly of the type must match the filters: {string.Join(", ", _typeFilters)}";
	}

	private sealed class TypeConstructorFilter : Filter<ConstructorInfo>
	{
		private readonly List<Filter<Type>> _typeFilters;

		public TypeConstructorFilter(List<Filter<Type>> typeFilters)
		{
			_typeFilters = typeFilters;
		}

		/// <inheritdoc cref="Filter{ConstructorInfo}.Applies(ConstructorInfo)" />
		public override bool Applies(ConstructorInfo constructorInfo)
		{
			Type? type = constructorInfo.DeclaringType;
			return type != null &&
			       _typeFilters.All(filter => filter.Applies(type));
		}

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> $"The declaring type of the constructor must match the filters: {string.Join(", ", _typeFilters)}";
	}

	private sealed class TypeEventFilter : Filter<EventInfo>
	{
		private readonly List<Filter<Type>> _typeFilters;

		public TypeEventFilter(List<Filter<Type>> typeFilters)
		{
			_typeFilters = typeFilters;
		}

		/// <inheritdoc cref="Filter{EventInfo}.Applies(EventInfo)" />
		public override bool Applies(EventInfo eventInfo)
		{
			Type? type = eventInfo.DeclaringType;
			return type != null &&
			       _typeFilters.All(filter => filter.Applies(type));
		}

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> $"The declaring type of the event must match the filters: {string.Join(", ", _typeFilters)}";
	}

	private sealed class TypeFieldFilter : Filter<FieldInfo>
	{
		private readonly List<Filter<Type>> _typeFilters;

		public TypeFieldFilter(List<Filter<Type>> typeFilters)
		{
			_typeFilters = typeFilters;
		}

		/// <inheritdoc cref="Filter{FieldInfo}.Applies(FieldInfo)" />
		public override bool Applies(FieldInfo fieldInfo)
		{
			Type? type = fieldInfo.DeclaringType;
			return type != null &&
			       _typeFilters.All(filter => filter.Applies(type));
		}

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> $"The declaring type of the field must match the filters: {string.Join(", ", _typeFilters)}";
	}

	private sealed class TypeMethodFilter : Filter<MethodInfo>
	{
		private readonly List<Filter<Type>> _typeFilters;

		public TypeMethodFilter(List<Filter<Type>> typeFilters)
		{
			_typeFilters = typeFilters;
		}

		/// <inheritdoc cref="Filter{MethodInfo}.Applies(MethodInfo)" />
		public override bool Applies(MethodInfo methodInfo)
		{
			Type? type = methodInfo.DeclaringType;
			return type != null &&
			       _typeFilters.All(filter => filter.Applies(type));
		}

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> $"The declaring type of the method must match the filters: {string.Join(", ", _typeFilters)}";
	}

	private sealed class TypePropertyFilter : Filter<PropertyInfo>
	{
		private readonly List<Filter<Type>> _typeFilters;

		public TypePropertyFilter(List<Filter<Type>> typeFilters)
		{
			_typeFilters = typeFilters;
		}

		/// <inheritdoc cref="Filter{PropertyInfo}.Applies(PropertyInfo)" />
		public override bool Applies(PropertyInfo propertyInfo)
		{
			Type? type = propertyInfo.DeclaringType;
			return type != null &&
			       _typeFilters.All(filter => filter.Applies(type));
		}

		/// <inheritdoc cref="object.ToString()" />
		public override string ToString()
			=> $"The declaring type of the property must match the filters: {string.Join(", ", _typeFilters)}";
	}
}
