using System.Collections.Generic;
using Romanesco.Common.Model.Basics;
using Romanesco.Model.ProjectComponents;

namespace Romanesco.Common.View.Interfaces
{
    public interface IViewFactoryProvider
    {
        IEnumerable<IViewFactory> GetViewFactories();
    }
}
