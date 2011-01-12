using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AplusCore.Types;
using AplusCore.Runtime.Function.Dyadic;

namespace AplusCore.Runtime.Function.Monadic.Operator.Reduction
{
    class ReduceMin : Reduction
    {
        protected override void SetVariables(ATypes type)
        {
            this.type = ATypes.AFloat;
            this.fillerElement = AFloat.Create(double.PositiveInfinity);
            this.function = DyadicFunctionInstance.Min;
        }
    }
}
