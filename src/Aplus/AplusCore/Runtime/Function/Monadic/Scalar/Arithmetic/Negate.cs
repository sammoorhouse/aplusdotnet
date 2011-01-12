using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AplusCore.Types;

namespace AplusCore.Runtime.Function.Monadic.Scalar.Arithmetic
{
    class Negate : MonadicScalar
    {
        public override AType ExecutePrimitive(AInteger argument, AplusEnvironment environment = null)
        {
            return Utils.CreateATypeResult(-argument.asFloat);
        }

        public override AType ExecutePrimitive(AFloat argument, AplusEnvironment environment = null)
        {
            return AFloat.Create(-argument.asFloat);
        }
    }
}
