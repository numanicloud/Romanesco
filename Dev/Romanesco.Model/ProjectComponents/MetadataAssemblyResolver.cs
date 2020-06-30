using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Romanesco.Model.ProjectComponents
{
	public class MetadataAssemblyResolver : System.Reflection.MetadataAssemblyResolver
	{
		public override Assembly Resolve(MetadataLoadContext context, AssemblyName assemblyName)
		{
			throw new NotImplementedException();
		}
	}
}
