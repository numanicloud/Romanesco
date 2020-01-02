using Romanesco.Model.ProjectComponents;

namespace Romanesco.Model.Services
{
    interface IProjectSettingProvider
    {
        ProjectSettings GetSettings();
    }
}
