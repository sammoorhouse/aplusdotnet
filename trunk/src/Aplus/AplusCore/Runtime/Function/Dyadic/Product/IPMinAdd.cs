using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AplusCore.Types;
using AplusCore.Runtime.Function.Monadic;

namespace AplusCore.Runtime.Function.Dyadic.Product
{
    /// <summary>
    /// Performs inner product  min.+  operator
    /// </summary>
    class IPMinAdd : InnerProduct
    {
        protected override AType Calculate(AType left, AType right, AplusEnvironment env)
        {
            AType inner = DyadicFunctionInstance.Add.Execute(right, left, env);
            AType result = MonadicFunctionInstance.ReduceMin.Execute(inner, env);
            return result;
        }
    }
}
