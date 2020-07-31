using System;
using System.Collections.Generic;
using System.Text;
using Deptorygen.Annotations;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Reflections;

namespace Romanesco.Common.Model.Interfaces
{
	[Factory]
	public interface IApiFactory
	{
		IDataAssemblyRepository ResolveDataAssemblyRepository();
		CommandHistory ResolveCommandHistory();
		IObjectInterpreter ResolveObjectInterpreter();
	}
}
