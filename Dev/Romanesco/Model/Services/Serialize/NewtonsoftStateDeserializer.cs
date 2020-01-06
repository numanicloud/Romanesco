using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.Services.Serialize
{
    class NewtonsoftStateDeserializer : IStateDeserializer
    {
        public object Deserialize(string encoded, Type type)
        {
            return JsonConvert.DeserializeObject(encoded, type);
        }
    }
}
