using System.Linq;

namespace LispDotNet {

    public static class LispNumberUtils {
        public static bool CheckIfAllNumbers(LispNode node, out LispNode err) {
            if(!node.Nested.All(item => item is LispNumber)) {
                node.Nested.Clear();
                err =  new LispNotNumberException();
                return true;
            }
            err = null;
            return false;
        }
    }

    [SymbolAttribute("+")]
    public class LispAddSymbol : LispSymbol {
        public override string Contents { get; } = "+";

        public override LispNode Operate(LispNode node) {

            LispNode err = null;
            if(LispNumberUtils.CheckIfAllNumbers(node, out err)) {
                return err;
            }

            var a = node.Pop() as LispNumber;

            while(node.Nested.Count > 0) {
                var b = node.Pop() as LispNumber;
                a.Number += b.Number;
            }

            return a;
        }
    }

    [SymbolAttribute("-")]
    public class LispSubtractSymbol : LispSymbol {
        public override string Contents { get; } = "-";

        public override LispNode Operate(LispNode node) {

            LispNode err = null;
            if(LispNumberUtils.CheckIfAllNumbers(node, out err)) {
                return err;
            }

            var a = node.Pop() as LispNumber;

            if(node.Nested.Count == 0) {
                a.Number = -a.Number;
            }
            else {
                while(node.Nested.Count > 0) {
                    var b = node.Pop() as LispNumber;
                    a.Number -= b.Number;
                }
            }

            return a;
        }
    }

    [SymbolAttribute("*")]
    public class LispMultiplySymbol : LispSymbol {
        public override string Contents { get; } = "*";

        public override LispNode Operate(LispNode node) {

            LispNode err = null;
            if(LispNumberUtils.CheckIfAllNumbers(node, out err)) {
                return err;
            }

            var a = node.Pop() as LispNumber;

            while(node.Nested.Count > 0) {
                var b = node.Pop() as LispNumber;
                a.Number *= b.Number;
            }

            return a;
        }
    }

    [SymbolAttribute("/")]
    public class LispDivideSymbol : LispSymbol {
        public override string Contents { get; } = "/";

        public override LispNode Operate(LispNode node) {

            LispNode err = null;
            if(LispNumberUtils.CheckIfAllNumbers(node, out err)) {
                return err;
            }

            var a = node.Pop() as LispNumber;

            while(node.Nested.Count > 0) {
                var b = node.Pop() as LispNumber;
                if(b.Number == 0) {
                    return new LispDivideByZeroException();
                }
                a.Number /= b.Number;
            }

            return a;
        }
    }
}
