using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.Services.Serialize
{
    class NewtonsoftStateSerializer : IStateSerializer
    {
        public string Serialize(object state)
        {
            return JsonConvert.SerializeObject(state);
        }
    }
}
