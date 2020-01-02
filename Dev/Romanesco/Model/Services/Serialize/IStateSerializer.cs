using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.Services.Serialize
{
    interface IStateSerializer
    {
        string Serialize(object state);
    }
}
