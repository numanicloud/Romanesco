using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.ProjectComponents;

namespace Romanesco.Model.Services
{
    public interface IProjectSettingProvider
    {
        ProjectSettings? InputCreateSettings(ProjectSettingsEditor editor);
    }
}
