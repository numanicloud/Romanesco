using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.Services.Serialize
{
    interface IStateDeserializer
    {
        object Deserialize(string encoded);
    }
}
