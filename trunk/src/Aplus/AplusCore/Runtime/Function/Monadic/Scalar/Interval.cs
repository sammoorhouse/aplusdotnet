using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AplusCore.Types;

namespace AplusCore.Runtime.Function.Monadic.Scalar
{
    class Interval
    {

        private static AArray GenerateRange(int start, int end)
        {
            AArray vector = new AArray(ATypes.AInteger);

            for (int i = start; i < end; i++)
            {
                vector.AddWithNoUpdate(new AInteger(i));
            }
            vector.UpdateInfo();
            return vector;
        }

        private static AArray GenerateVector(int argument)
        {
            return GenerateRange(0, argument);
        }

        private static AType GenerateMultiDimension(AArray arguments)
        {
            int itemCount = 1;
            for (int i = 0; i < arguments.Length; i++)
            {
                itemCount *= arguments[i].asInteger;
            }

            AArray range = GenerateRange(0, itemCount);

            return Function.Dyadic.NonScalar.Reshape.Execute(range, arguments);
        }

        public static AType Execute(AType argument)
        {
            if (argument.Rank > 1)
            {
                throw new Error(ErrorType.Rank, "iota");
            }

            if (argument.IsArray && argument.Type == ATypes.AInteger)
            {
                return GenerateMultiDimension((AArray)argument);
            }
            else if (argument.IsTolerablyInteger)
            {
                return GenerateVector(argument.asInteger);
            }

            throw new Error(ErrorType.Type, "Interval");
        }
    }
}
