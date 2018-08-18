namespace LispDotNet {
    public class LispNumber : LispNode {

        public override LispNodeType NodeType {
            get {
                return LispNodeType.NUMBER;
            }
        }

        public override string Contents {
            get {
                return Number.ToString();
            }
        }

        public double Number;

        public LispNumber(string num) {
            Number = System.Convert.ToDouble(num);
        }

        public LispNumber(double num) {
            Number = num;
        }
    }
}
