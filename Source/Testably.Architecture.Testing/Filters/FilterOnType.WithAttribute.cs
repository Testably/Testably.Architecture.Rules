using System;

namespace Testably.Architecture.Testing;

public abstract partial class FilterOnType
{
	/// <summary>
	///     Add additional filters on a <see cref="Type" /> which has an attribute.
	/// </summary>
	public class WithAttribute : FilterOnType
	{
		internal WithAttribute(
			IFilter<Type> expectationFilter,
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
		public WithAttribute OrAttribute<TAttribute>(
			Func<TAttribute, bool>? predicate = null,
			bool inherit = true) where TAttribute : Attribute
		{
			Predicates.Add(type => type.HasAttribute(predicate, inherit));
			return this;
		}
	}
}
