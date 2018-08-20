using System.Collections.Generic;
using System.Linq;

namespace LispDotNet {
    public enum LispNodeType {
        SYMBOL, NUMBER, ERROR, LIST, DATALIST, FUNCTION, INPUT_SYMBOL, USER_FUNCTION,NODE
    }

    public abstract class LispNode {
        public abstract LispNodeType NodeType { get; }
        public abstract string Contents { get; }

        public List<LispNode> Nested = new List<LispNode>();

        public override string ToString() {
            return Contents;
        }

        public abstract LispNode GetNodeCopy();

    }

    public static class LispNodeUtils {

        public static long GetTotalNodes (LispNode node) {
            if(node.Nested == null || node.Nested.Count == 0)  return 1;

            if(node.Nested?.Count >= 1) {
                long total = 1;

                node.Nested.ToList().ForEach(childNode => {
                        total += GetTotalNodes(childNode);
                    });

                return total;
            }

            return 0;
        }

        public static LispNode Pop(this LispNode node) {
            var p = node.Nested.First();
            node.Nested.RemoveAt(0);
            return p;
        }

        public static LispNode Join(LispNode x, LispNode y) {

            while(y.Nested.Count > 0) {
                x.Nested.Add(y.Pop());
            }

            y.Nested.Clear();

            return x;
        }

        public static LispNode Take(this LispNode node, int index) {
            var p = node.Nested[index];
            node.Nested.Clear();
            return p;
        }
    }
}
