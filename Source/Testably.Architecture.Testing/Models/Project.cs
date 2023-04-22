using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Testably.Architecture.Testing.Models;
public class Project
{
	private readonly Assembly _assembly;

	public Project(Assembly assembly)
	{
		_assembly = assembly;
	}

	public string Name => _assembly.GetName().Name ?? _assembly.ToString();
}
