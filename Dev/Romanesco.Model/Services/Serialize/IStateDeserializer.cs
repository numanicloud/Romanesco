using System;

namespace Romanesco.Model.Services.Serialize
{
    public interface IStateDeserializer
    {
        object Deserialize(string encoded, Type type);
    }
}
