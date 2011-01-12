using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AplusCore.Types;

namespace AplusCore.Runtime.Function.Dyadic.NonScalar.Selection
{
    class Left : AbstractDyadicFunction
    {
        public override AType Execute(AType right, AType left, AplusEnvironment environment = null)
        {
            return left;
        }
    }
}
