using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AplusCore.Types;
using AplusCore.Runtime.Function.Dyadic;

namespace AplusCore.Runtime.Function.Monadic.Operator.Scan
{
    class ScanMax : Scan
    {
        protected override void SetVariables(AType argument)
        {
            this.function = DyadicFunctionInstance.Max;

            if (argument.Length == 0)
            {
                this.type = argument.Type == ATypes.AFloat ? ATypes.AFloat : ATypes.AInteger;
            }
            else
            {
                this.type = argument.Type;
            }
        }
    }
}
