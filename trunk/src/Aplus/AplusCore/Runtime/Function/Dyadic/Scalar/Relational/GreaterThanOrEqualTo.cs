﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AplusCore.Types;

namespace AplusCore.Runtime.Function.Dyadic.Scalar.Relational
{
    class GreaterThanOrEqualTo : DyadicScalar
    {
        [DyadicScalarMethod]
        public AType ExecutePrimitive(AFloat rightArgument, AFloat leftArgument)
        {
            return FloatGTE(rightArgument, leftArgument);
        }

        [DyadicScalarMethod]
        public AType ExecutePrimitive(AInteger rightArgument, AInteger leftArgument)
        {
            int number = (leftArgument.asInteger >= rightArgument.asInteger) ? 1 : 0;
            return AInteger.Create(number);
        }

        [DyadicScalarMethod]
        public AType ExecutePrimitive(AFloat rightArgument, AInteger leftArgument)
        {
            return FloatGTE(rightArgument, leftArgument);
        }

        [DyadicScalarMethod]
        public AType ExecutePrimitive(AInteger rightArgument, AFloat leftArgument)
        {
            return FloatGTE(rightArgument, leftArgument);
        }

        [DyadicScalarMethod]
        public AType ExecutePrimitive(AChar rightArgument, AChar leftArgument)
        {
            int number = (leftArgument.asChar >= rightArgument.asChar) ? 1 : 0;
            return AInteger.Create(number);
        }

        [DyadicScalarMethod]
        public AType ExecutePrimitive(ASymbol rightArgument, ASymbol leftArgument)
        {
            int number = (String.Compare(leftArgument.asString, rightArgument.asString) >= 0) ? 1 : 0;
            return AInteger.Create(number);
        }

        private AType FloatGTE(AType right, AType left)
        {
            int number = (Utils.ComparisonTolerance(left.asFloat, right.asFloat) || left.asFloat > right.asFloat) ? 1 : 0;
            return AInteger.Create(number);
        }
    }
}