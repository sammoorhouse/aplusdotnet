using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AplusCore.Types;
using AplusCore.Runtime.Function.Dyadic;

namespace AplusCore.Runtime.Function.Monadic.NonScalar.Structural
{
    class Transpose : AbstractMonadicFunction
    {
        #region Entry point

        /// <summary>
        /// Transpose reduction to Transpose Axes.
        /// As follows: (rot iota rho rho x) flip x
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        public override AType Execute(AType argument, AplusEnvironment environment)
        {
            if (argument.IsArray)
            {
                AType transposeVector = Enumerable.Range(0, argument.Rank).Reverse().ToAArray();
                return DyadicFunctionInstance.TransposeAxis.Execute(argument, transposeVector);
            }
            else
            {
                return argument.Clone();
            }
        }

        #endregion
    }
}
