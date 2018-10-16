namespace LispDotNet {
    public class    LispEmptyDataException : LispError {
        public override LispErrorType ErrorType {
            get {
                return LispErrorType.EMPTY_DATA;
            }
        }

        private string name;
        public LispEmptyDataException(string fname) {
            name = fname;
        }

        protected override string ErrorMessage {
            get {
                return $"Function {name} passed empty data list";
            }
        }
    }

    public class LispNotDataListException : LispError {
        public override LispErrorType ErrorType { get; } = LispErrorType.NOT_DATA_LIST;

        private string name;
        public LispNotDataListException(string fname) {
            name = fname;
        }

        protected override string ErrorMessage {
            get {
                return $"Function {name} passed argument which is not a data list";
            }
        }
    }

}
