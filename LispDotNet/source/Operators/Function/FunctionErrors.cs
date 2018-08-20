namespace LispDotNet {
    public class LispNonSymbolException : LispError {
        public override LispErrorType ErrorType {
            get {
                return LispErrorType.DEFINE_NON_SYMBOL;
            }
        }

        private string name;
        public LispNonSymbolException(string fname) {
            name = fname;
        }

        protected override string ErrorMessage {
            get {
                return $"Function {name} cannot define non-symbol type";
            }
        }
    }

    public class LispIncorrectNumberToDefineException : LispError {
        public override LispErrorType ErrorType {
            get {
                return LispErrorType.INCORRECT_ARGS_FOR_DEFINE;
            }
        }

        private string name;
        public LispIncorrectNumberToDefineException(string fname) {
            name = fname;
        }

        protected override string ErrorMessage {
            get {
                return $"Function {name} cannot define incorrect values to symbols";
            }
        }
    }
}
