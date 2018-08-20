using System.Linq;

namespace LispDotNet {
    public class LispList : LispNode {

        public override LispNodeType NodeType {
            get {
                return LispNodeType.LIST;
            }
        }

        public override string Contents {
            get {
                if(Nested != null && Nested.Count > 0) {
                    var str = Nested.Aggregate("",(a,b) => $"{a} {b.Contents}");
                    return "("+str+")";
                }
                return "()";
            }
        }

        public override LispNode GetNodeCopy() {
            return new LispList {
                Nested = this.Nested.Select(x => x.GetNodeCopy()).ToList()
            };
        }
    }
}
