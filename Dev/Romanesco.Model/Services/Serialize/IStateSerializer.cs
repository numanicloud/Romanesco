using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.Services.Serialize
{
    public interface IStateSerializer
    {
        string Serialize(object state);
    }
}
