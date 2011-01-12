using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AplusCore.Types;
using AplusCore.Runtime.Function.Dyadic.Scalar;

namespace AplusCore.Runtime.Function
{
    class DyadicScalarFunction
    {
        public static AType Execute(IDyadicScalarFunction func, AType right, AType left)
        {
            if (left.IsArray && right.IsArray)
            {
                if (left.Length == right.Length)
                {
                    AArray l = (AArray)left;
                    AArray r = (AArray)right;
                    AArray result = new AArray(ATypes.AArray);
                    for (int i = 0; i < l.Length; i++)
                    {
                        result.Add(Execute(func, r[i], l[i]));
                    }
                    return result;
                }
                else
                {
                    throw new Error(ErrorType.Length, "Length error!");
                }
            }
            else if (left.IsArray && right.IsPrimitive)
            {
                AArray l = (AArray)left;
                AArray result = new AArray(ATypes.AArray);
                for (int i = 0; i < l.Length; i++)
                {
                    result.Add(Execute(func, right, l[i]));
                }
                return result;
            }
            else if (left.IsPrimitive && right.IsArray)
            {
                AArray r = (AArray)right;
                AArray result = new AArray(ATypes.AArray);
                for (int i = 0; i < r.Length; i++)
                {
                    result.Add(Execute(func, r[i], left));
                }
                return result;
            }
            else
            {
                return func.Execute(right,left);
            }
        }
    }
}
