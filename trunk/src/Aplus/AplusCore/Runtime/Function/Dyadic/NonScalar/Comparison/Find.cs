using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AplusCore.Types;

namespace AplusCore.Runtime.Function.Dyadic.NonScalar.Comparison
{
    class Find : AbstractDyadicFunction
    {
        #region Variables

        private AType y;
        private List<int> cellShape;

        #endregion

        #region Entry point

        public override AType Execute(AType right, AType left, AplusEnvironment environment = null)
        {
            PrepareVariables(left, right);
            return MultipleItmesWalking(right);
        }

        #endregion

        #region Preparation

        /// <summary>
        /// Prepare the left side and determine the cellshape.
        /// </summary>
        /// <param name="left"></param>
        private void PrepareVariables(AType left, AType right)
        {
            if (!Utils.IsSameGeneralType(left, right) && !Util.TypeCorrect(right.Type, left.Type, "N?", "?N"))
            {
                throw new Error.Type(TypeErrorText);
            }

            this.y = left;
            this.cellShape = (this.y.Rank > 1 && this.y.Length > 0) ? this.y[0].Shape : new List<int>();

            if (right.Rank < this.cellShape.Count)
            {
                throw new Error.Rank(RankErrorText);
            }
        }

        #endregion

        #region Computation

        /// <summary>
        /// If the cell rank > 1, first step is check the shape of actual cell equal to determined cell shape.
        /// If different we throw Length error. The second step is classify the cell.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private AType MultipleItmesWalking(AType cell)
        {
            if (this.cellShape.Count == cell.Shape.Count)
            {
                if (!this.cellShape.SequenceEqual(cell.Shape))
                {
                    throw new Error.Length(LengthErrorText);
                }
                return Classify(cell);
            }
            else
            {
                AType result = AArray.Create(ATypes.AInteger);

                foreach (AType item in cell)
                {
                    result.AddWithNoUpdate(MultipleItmesWalking(item));
                }
                result.UpdateInfo();

                return result;
            }
        }

        /// <summary>
        /// The value of that element is the index of the first item of y to which the cell is identical.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private AType Classify(AType element)
        {
            int index = this.y.Length;

            if (this.y.IsArray)
            {
                // TODO: refactor remove 'i', use 'index' instead
                for (int i = 0; i < this.y.Length; i++)
                {
                    if (this.y[i].ComparisonToleranceCompareTo(element))
                    {
                        index = i;
                        break;
                    }
                }
            }
            else
            {
                if (this.y.ComparisonToleranceCompareTo(element))
                {
                    index = 0;
                }
            }

            return AInteger.Create(index);
        }

        #endregion
    }
}
