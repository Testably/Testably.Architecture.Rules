using System;
using System.Reflection;

namespace Testably.Architecture.Testing.Filters;

public abstract partial class FilterOnType
{
	/// <summary>
	///     Add additional filters on a <see cref="Type" /> which has a method with an attribute.
	/// </summary>
	public class WithMethodAttribute : FilterOnType
	{
		internal WithMethodAttribute(
			IExpectationFilter<Type> expectationFilter,
			Func<Type, bool> predicate) : base(expectationFilter, predicate)
		{
		}

		/// <summary>
		///     Adds another filter <see cref="Type" />s for an attribute of type <typeparamref name="TAttribute" />.
		/// </summary>
		/// <param name="predicate">
		///     (optional) A predicate to check the attribute values.
		///     <para />
		///     If not set (<see langword="null" />), will only check if the attribute is present.
		/// </param>
		/// <param name="inherit">
		///     <see langword="true" /> to search the inheritance chain to find the attributes; otherwise,
		///     <see langword="false" />.<br />
		///     Defaults to <see langword="true" />
		/// </param>
		public WithMethodAttribute OrAttribute<TAttribute>(
			Func<TAttribute, MethodInfo, bool>? predicate = null,
			bool inherit = true) where TAttribute : Attribute
		{
			Predicates.Add(type => type.HasMethodWithAttribute(predicate, inherit));
			return this;
		}
	}
}
