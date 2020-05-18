namespace Romanesco.Model.ProjectComponents
{
    public class ProjectData
    {
        // シリアライズ用クラスなのでnullを無視
#nullable disable
        public string AssemblyPath { get; set; }
        public string ProjectTypeQualifier { get; set; }
        public string ProjectTypeExporterQualifier { get; set; }
        public string EncodedMaster { get; set; }
        public string[] DependencyProjects { get; set; }
#nullable restore
    }
}
