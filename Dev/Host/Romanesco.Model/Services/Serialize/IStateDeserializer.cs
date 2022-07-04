using System;
using Newtonsoft.Json.Linq;

namespace Romanesco.Model.Services.Serialize
{
    public interface IStateDeserializer
    {
        object? Deserialize(JObject encoded, Type type);
    }
}
