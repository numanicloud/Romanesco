using Newtonsoft.Json;

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
