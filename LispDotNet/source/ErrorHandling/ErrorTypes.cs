namespace LispDotNet {
    public enum LispErrorType {
        DIV_BY_ZERO, NOT_NUMBER, NOT_SYMBOL, EMPTY_LIST,
        NOT_DATA_LIST, TOO_MANY_ARGS, INCORRECT_ARG_TYPES,
        EMPTY_DATA, DEFINE_NON_SYMBOL, INCORRECT_ARGS_FOR_DEFINE
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

        public override LispNode GetNodeCopy() {
            return this;
        }
    }

    public class LispTooManyArgsException : LispError {
        public override LispErrorType ErrorType { get; } = LispErrorType.TOO_MANY_ARGS;

        private string name;
        private int expected;
        private int got;

        public LispTooManyArgsException(string fName, int _expected, int _got) {
            name = fName;
            expected = _expected;
            got = _got;
        }

        protected override string ErrorMessage {
            get {
                return $"Function {name} passed incorrect number of arguments. Expected {expected}, got {got}";
            }
        }
    }

    public class LispIncorrectArgTypesException : LispError {
        public override LispErrorType ErrorType { get; } = LispErrorType.INCORRECT_ARG_TYPES;

        private string name;
        private int argumentIndex;
        private string expectedArgument;
        private string gotArgument;

        public LispIncorrectArgTypesException(string fName,int index, string _expected, string _got) {
            name = fName;
            argumentIndex = index;
            expectedArgument = _expected;
            gotArgument = _got;
        }

        protected override string ErrorMessage {
            get {
                return $"Function {name} passed incorrect argument type for {argumentIndex}. Expected {expectedArgument}, got {gotArgument}";
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

        private string sym;

        public LispNotSymbolException(string s = "") {
            sym = s;
        }

        protected override string ErrorMessage {
            get {
                return $"First atom {sym} in list is not a symbol, cannot evaluate";
            }
        }
    }
}
