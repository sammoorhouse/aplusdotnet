using System;

namespace AplusCore.Runtime
{
    #region A+ Error Types
    public enum ErrorType
    {
        Signal = -1,
        Interrupt = 1,
        Wsfull,
        Stack,
        Value,
        Valence,
        Type,
        Rank,
        Length,
        Domain,
        Index,
        Mismatch,
        Nonce,
        MaxRank,
        NonFunction,
        Parse,
        MaxItems,
        Invalid,
        NonData
    }
    #endregion

    public class Error : Exception
    {
        #region Variables

        private ErrorType errortype;

        #endregion

        #region Properties

        public ErrorType ErrorType { get { return this.errortype; } }
        public virtual string ErrorText { get { return this.ErrorType.ToString(); } }

        #endregion

        #region Constructor

        public Error(ErrorType errortype, string message)
            : base(message)
        {
            this.errortype = errortype;
        }

        #endregion

        #region overrides

        public override string ToString()
        {
            return String.Format("//[error] {0}: {1}", this.Message, this.ErrorType.ToString());
        }

        #endregion

        #region SubClasses

        public class Signal : Error
        {
            public override string ErrorText { get { return this.Message; } }

            public Signal(string message)
                : base(ErrorType.Signal, message)
            {
            }
        }

        public class Interrupt : Error
        {
            public Interrupt(string message)
                : base(ErrorType.Interrupt, message)
            {
            }
        }

        public class Wsfull : Error
        {
            public Wsfull(string message)
                : base(ErrorType.Wsfull, message)
            {
            }
        }

        public class Stack : Error
        {
            public Stack(string message)
                : base(ErrorType.Stack, message)
            {
            }
        }

        public class Value : Error
        {
            public Value(string message)
                : base(ErrorType.Value, message)
            {
            }
        }

        public class Valence : Error
        {
            public Valence(string message)
                : base(ErrorType.Valence, message)
            {
            }
        }

        public class Type : Error
        {
            public Type(string message)
                : base(ErrorType.Type, message)
            {
            }
        }

        public class Rank : Error
        {
            public Rank(string message)
                : base(ErrorType.Rank, message)
            {
            }
        }

        public class Length : Error
        {
            public Length(string message)
                : base(ErrorType.Length, message)
            {
            }
        }

        public class Domain : Error
        {
            public Domain(string message)
                : base(ErrorType.Domain, message)
            {
            }
        }

        public class Index : Error
        {
            public Index(string message)
                : base(ErrorType.Index, message)
            {
            }
        }

        public class Mismatch : Error
        {
            public Mismatch(string message)
                : base(ErrorType.Mismatch, message)
            {
            }
        }

        public class Nonce : Error
        {
            public Nonce(string message)
                : base(ErrorType.Nonce, message)
            {
            }
        }

        public class MaxRank : Error
        {
            public MaxRank(string message)
                : base(ErrorType.MaxRank, message)
            {
            }
        }

        public class NonFunction : Error
        {
            public NonFunction(string message)
                : base(ErrorType.NonFunction, message)
            {
            }
        }


        public class Parse : Error
        {
            public Parse(string message)
                : base(ErrorType.Parse, message)
            {
            }
        }

        public class MaxItems : Error
        {
            public MaxItems(string message)
                : base(ErrorType.MaxItems, message)
            {
            }
        }

        public class Invalid : Error
        {
            public Invalid(string message)
                : base(ErrorType.Invalid, message)
            {
            }
        }

        public class NonData : Error
        {
            public NonData(string message)
                : base(ErrorType.NonData, message)
            {
            }
        }

        #endregion
    }

}
