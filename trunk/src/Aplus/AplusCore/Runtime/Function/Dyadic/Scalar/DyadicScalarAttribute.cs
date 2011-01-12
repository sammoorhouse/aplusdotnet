using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AplusCore.Runtime.Function.Dyadic.Scalar
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    sealed class DyadicScalarMethodAttribute : Attribute
    {

        public DyadicScalarMethodAttribute()
        {
        }

    }
}
