using System;
using System.Reflection;

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
