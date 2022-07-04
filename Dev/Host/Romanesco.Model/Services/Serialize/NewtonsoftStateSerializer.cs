using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Romanesco.Model.Services.Serialize
{
	internal class NewtonsoftStateSerializer : IStateSerializer
    {
        // ここで単にNewtonSoft.Jsonに渡してしまうと、objectにはReactivePropertyの状態で渡ってくるのでエラーになる
        // ValueStorage.currentValueをシリアライズしなければならない
        public JObject Serialize(object state)
        {
            return JObject.FromObject(state, new JsonSerializer()
			{
                TypeNameHandling = TypeNameHandling.All
			});
        }
    }
}
