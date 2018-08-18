namespace LispDotNet {
    public enum LispErrorType {
        DIV_BY_ZERO, NOT_NUMBER, NOT_SYMBOL, EMPTY_LIST,
        NOT_DATA_LIST, TOO_MANY_ARGS, INCORRECT_ARG_TYPES,
        EMPTY_DATA
    }

    public abstract class LispError : LispNode {

        public override LispNodeType NodeType {
            get {
                return LispNodeType.ERROR;
            }
        }

        public abstract LispErrorType ErrorType { get; }

        protected abstract string ErrorMessage { get; }

        public override string Contents {
            get {
                return $"Error: {ErrorMessage}";
            }
        }
    }

    public class LispTooManyArgsException : LispError {
        public override LispErrorType ErrorType { get; } = LispErrorType.TOO_MANY_ARGS;

        private string name;

        public LispTooManyArgsException(string fName) {
            name = fName;
        }

        protected override string ErrorMessage {
            get {
                return $"Function {name} passed too many arguments";
            }
        }
    }

    public class LispIncorrectArgTypesException : LispError {
        public override LispErrorType ErrorType { get; } = LispErrorType.INCORRECT_ARG_TYPES;

        private string name;

        public LispIncorrectArgTypesException(string fName) {
            name = fName;
        }

        protected override string ErrorMessage {
            get {
                return $"Function {name} passed incorrect argument types";
            }
        }
    }

    public class LispDivideByZeroException : LispError {
        public override LispErrorType ErrorType {
            get {
                return LispErrorType.DIV_BY_ZERO;
            }
        }

        protected override string ErrorMessage {
            get {
                return "Error: Attempt to divide by zero";
            }
        }
    }

    public class LispNotNumberException : LispError {
        public override LispErrorType ErrorType {
            get {
                return LispErrorType.NOT_NUMBER;
            }
        }

        protected override string ErrorMessage {
            get {
                return "Attempt to apply number operator on non-number";
            }
        }
    }

    public class LispEmptyListException : LispError {
        public override LispErrorType ErrorType {
            get {
                return LispErrorType.EMPTY_LIST;
            }
        }

        protected override string ErrorMessage {
            get {
                return "Expression has no elements to evaluate";
            }
        }
    }

    public class LispNotSymbolException : LispError {
        public override LispErrorType ErrorType {
            get {
                return LispErrorType.NOT_SYMBOL;
            }
        }

        protected override string ErrorMessage {
            get {
                return "First atom in list is not a symbol, cannot evaluate";
            }
        }
    }
}
