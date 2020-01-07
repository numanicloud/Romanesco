using Romanesco.Annotations;
using Romanesco.Common.Model.Basics;
using System.Linq;
using System.Reflection;
using Romanesco.Model.ProjectComponents;

namespace Romanesco.Common.Model.ProjectComponents
{
    public class Project
    {
        public ProjectSettings Settings { get; }
        public StateRoot Root { get; }
        public string DefaultSavePath { get; set; }

        public Project(ProjectSettings settings, StateRoot root)
        {
            Settings = settings;
            Root = root;
        }
    }
}
