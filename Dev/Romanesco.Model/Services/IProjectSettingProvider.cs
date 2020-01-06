using Romanesco.Model.ProjectComponents;

namespace Romanesco.Model.Services
{
    public interface IProjectSettingProvider
    {
        ProjectSettings GetSettings();
    }
}
