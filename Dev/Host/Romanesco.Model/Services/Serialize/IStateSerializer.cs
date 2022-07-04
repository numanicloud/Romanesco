using Newtonsoft.Json.Linq;

namespace Romanesco.Model.Services.Serialize
{
    public interface IStateSerializer
    {
        JObject Serialize(object state);
    }
}
