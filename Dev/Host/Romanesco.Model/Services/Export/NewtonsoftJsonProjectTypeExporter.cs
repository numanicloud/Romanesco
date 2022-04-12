using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Romanesco.Common.Model.Interfaces;

namespace Romanesco.Model.Services.Export
{
	internal sealed class NewtonsoftJsonProjectTypeExporter : IProjectTypeExporter
    {
        public bool DoExportIntoSingleFile => true;

        public async Task ExportAsync(object rootInstance, string exportPath)
        {
            var json = JsonConvert.SerializeObject(rootInstance, Formatting.Indented);
            using (var file = File.Create(exportPath))
            {
                using (var writer = new StreamWriter(file))
                {
                    await writer.WriteAsync(json);
                }
            }
        }
    }
}
