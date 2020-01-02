using Romanesco.Common.Entities;
using Romanesco.Model.Services.Serialize;
using System.Linq;

namespace Romanesco.Model.ProjectComponents
{
    class Project
    {
        public ProjectSettings Settings { get; }
        public StateRoot Root { get; }

        public Project(ProjectSettings settings, StateRoot root)
        {
            Settings = settings;
            Root = root;
        }

        public ProjectData ToData(IStateSerializer serializer)
        {
            var jsonRoots = Root.States.Select(s => s.Settability.GetValue()).ToArray();
            return new ProjectData
            {
                AssemblyPath = Settings.Assembly.Location,
                ProjectTypeQualifier = Settings.ProjectType.FullName,
                EncodedMaster = serializer.Serialize(jsonRoots),
            };
        }
    }
}
