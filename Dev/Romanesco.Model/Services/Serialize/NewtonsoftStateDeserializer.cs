using Newtonsoft.Json;
using System;

namespace Romanesco.Model.Services.Serialize
{
	internal class NewtonsoftStateDeserializer : IStateDeserializer
    {
        public object? Deserialize(string encoded, Type type)
        {
            return JsonConvert.DeserializeObject(encoded, type, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }
    }
}
