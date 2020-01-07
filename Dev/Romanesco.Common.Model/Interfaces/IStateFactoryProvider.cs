using System.Collections.Generic;
using Romanesco.Common.Model.Basics;
using Romanesco.Model.ProjectComponents;

namespace Romanesco.Common.Model.Interfaces
{
    public interface IStateFactoryProvider
    {
        IEnumerable<IStateFactory> GetStateFactories(ProjectContextCrawler context);
    }
}
