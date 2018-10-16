using System;

namespace LispDotNet {

    public class SymbolAttribute : Attribute {
        public string SymbolType;

        public SymbolAttribute(string type) {
            this.SymbolType = type;
        }
    }

    public abstract class LispSymbol : LispNode   {
        public override LispNodeType NodeType { get; } = LispNodeType.SYMBOL;

        public abstract string Symbol { get; }

        public abstract LispNode Operate(LispEnvironment env, LispNode a);

        public override LispNode GetNodeCopy() {
            return this;
        }
    }

    public class InputSymbol : LispNode {

        public override LispNodeType NodeType { get; } = LispNodeType.INPUT_SYMBOL;

        public string Symbol;

        public override string Contents {
            get {
                return Symbol;
            }
        }

        public InputSymbol (string _symbol) {
            Symbol = _symbol;
        }

        public override LispNode GetNodeCopy() {
            return new InputSymbol(Symbol);
        }
    }
}
