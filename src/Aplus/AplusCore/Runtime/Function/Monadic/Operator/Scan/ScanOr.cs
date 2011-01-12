using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AplusCore.Types;
using AplusCore.Runtime.Function.Dyadic;

namespace AplusCore.Runtime.Function.Monadic.Operator.Scan
{
    class ScanOr : Scan
    {
        protected override void SetVariables(AType argument)
        {
            this.function = DyadicFunctionInstance.Or;
            this.type = ATypes.AInteger;
        }

        //If argument type is float, we have to convert the first item to integer.
        protected override AType PreProcess(AType argument)
        {
            return ConvertToInt(argument);
        }
    }
}
