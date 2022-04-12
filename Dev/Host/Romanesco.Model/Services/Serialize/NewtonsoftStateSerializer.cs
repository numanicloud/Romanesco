using Newtonsoft.Json;

namespace Romanesco.Model.Services.Serialize
{
	internal class NewtonsoftStateSerializer : IStateSerializer
    {
        // ここで単にNewtonSoft.Jsonに渡してしまうと、objectにはReactivePropertyの状態で渡ってくるのでエラーになる
        // ValueStorage.currentValueをシリアライズしなければならない
        public string Serialize(object state)
        {
            return JsonConvert.SerializeObject(state, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }
    }
}
