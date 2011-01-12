using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AplusCore.Runtime.Function.Operator.Dyadic
{
    class DyadicOperatorInstance
    {
        internal static readonly Each Each = new Each();
        internal static readonly Apply Apply = new Apply();
        internal static readonly Rank Rank = new Rank();
    }
}
