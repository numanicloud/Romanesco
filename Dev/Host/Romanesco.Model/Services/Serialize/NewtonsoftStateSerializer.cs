﻿using Newtonsoft.Json;

namespace Romanesco.Model.Services.Serialize
{
	internal class NewtonsoftStateSerializer : IStateSerializer
    {
        public string Serialize(object state)
        {
            return JsonConvert.SerializeObject(state, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }
    }
}