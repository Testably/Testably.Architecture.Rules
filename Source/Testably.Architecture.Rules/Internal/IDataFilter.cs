using System.Collections.Generic;

namespace Testably.Architecture.Rules.Internal;

internal interface IDataFilter<TData>
{
	IEnumerable<TData> Filter(IEnumerable<TData> source);
}
