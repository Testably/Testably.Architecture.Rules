using System;
using System.Linq;

namespace Testably.Architecture.Rules.Internal;

internal class TypeRule : Rule<Type>, ITypeExpectation
{
	/// <inheritdoc cref="IRule.Check" />
	public override IRuleCheck Check
		=> new RuleCheck<Type>(Filters, Requirements, Exemptions,
			_ => _.SelectMany(a => a.GetTypes()));
}
