using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AplusCore.Types;
using AplusCore.Runtime.Function.Dyadic;
using AplusCore.Runtime.Function.Monadic;

namespace AplusCore.Runtime.Function.Dyadic.Product
{
    /// <summary>
    /// Performs inner product  +.*  operator
    /// </summary>
    class IPAddMultiply : InnerProduct
    {
        protected override AType Calculate(AType left, AType right, AplusEnvironment env)
        {
            AType inner = DyadicFunctionInstance.Multiply.Execute(right, left, env);
            AType result = MonadicFunctionInstance.ReduceAdd.Execute(inner, env);
            return result;
        }
    }
}
