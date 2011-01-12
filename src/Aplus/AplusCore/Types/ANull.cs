using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AplusCore.Types
{
    /// <summary>
    /// Represents the null element in the A+ type system
    /// </summary>
    public class ANull : AArray
    {
        #region Constructor
        private ANull() : base(ATypes.ANull)
        {
            //this.length = 0;
            //this.shape = new List<int>() { 0 };
            //this.rank = 1;

            //this.type = ATypes.ANull;
        }

        public static AType Create()
        {
            return new AReference(new ANull());
        }

        #endregion

        #region Properties

        //public override bool IsPrimitive { get { return true; } }

        #endregion

        #region Overrides


        public override AType Clone()
        {
            return Types.ANull.Create();
        }

        /// <summary>
        /// ANull is not equal to anything!
        /// </summary>
        /// <param name="obj">this parameter is ignored</param>
        /// <returns>false</returns>
        public override bool Equals(object obj)
        {
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ 0xBEAF;
        }

        public override string ToString()
        {
            return "";
        }

        #endregion
    }
}
