﻿using AplusCore.Types;

namespace AplusCore.Runtime.Function.Dyadic.Product
{
    class OPNotEqual : OuterProduct
    {
        protected override AType Calculate(AType left, AType right, Aplus env)
        {
            return DyadicFunctionInstance.NotEqualTo.Execute(right, left, env);
        }
    }
}
