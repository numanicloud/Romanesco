using Romanesco.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Romanesco.Common.Entities
{
    public class StateRoot
    {
        public IFieldState[] States { get; set; }
    }
}
