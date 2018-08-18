using System;

namespace LispDotNet {
    [SymbolAttribute("print")]
    public class LispPrintSymbol : LispSymbol {

        public override string Contents { get; } = "print";

        public override LispNode Operate(LispNode a) {
            Console.WriteLine(a);
            return a;
        }
    }
}
