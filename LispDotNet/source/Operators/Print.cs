using System;

namespace LispDotNet {
    [SymbolAttribute("print")]
    public class LispPrintSymbol : LispSymbol {

        public override string Symbol { get; } = "print";

        public override string Contents { get; } = "print";

        public override LispNode Operate(LispEnvironment env,LispNode a) {
            Console.WriteLine(a);
            return a;
        }
    }
}
