using System.Collections.Generic;

namespace LispDotNet {
    public enum LispNodeType {
        SYMBOL, NUMBER, ERROR, LIST, DATALIST
    }

    public abstract class LispNode {
        public abstract LispNodeType NodeType { get; }
        public abstract string Contents { get; }
        public List<LispNode> Nested = new List<LispNode>();

        public override string ToString() {
            return Contents;
        }
    }
}
