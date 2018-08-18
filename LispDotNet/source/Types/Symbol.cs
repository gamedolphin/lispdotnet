using System;

namespace LispDotNet {

    public class SymbolAttribute : Attribute {
        public string SymbolType;

        public SymbolAttribute(string type) {
            this.SymbolType = type;
        }
    }

    public abstract class LispSymbol : LispNode   {
        public override LispNodeType NodeType {
            get {
                return LispNodeType.SYMBOL;
            }
        }

        public abstract LispNode Operate(LispNode a);
    }
}
