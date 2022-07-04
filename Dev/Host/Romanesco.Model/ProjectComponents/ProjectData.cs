using Newtonsoft.Json.Linq;

namespace Romanesco.Model.ProjectComponents
{
	internal sealed class ProjectData
	{
		// シリアライズ用クラスなのでnullを無視
#nullable disable
		public string AssemblyPath { get; set; }
		public string ProjectTypeQualifier { get; set; }
		public string ProjectTypeExporterQualifier { get; set; }
		public JObject EncodedMaster { get; set; }
		public string[] DependencyProjects { get; set; }
#nullable restore
	}
}
