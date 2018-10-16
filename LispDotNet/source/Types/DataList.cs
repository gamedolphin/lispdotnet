using System.Linq;
using System.Collections.Generic;

namespace LispDotNet {
    public class LispDataList : LispNode {

        public override LispNodeType NodeType {
            get {
                return LispNodeType.DATALIST;
            }
        }

        public override string Contents {
            get {
                if(Nested != null && Nested.Count > 0) {
                    var str = Nested.Aggregate("",(a,b) => $"{a} {b.Contents}");
                    return "{"+str+"}";
                }
                return "{}";
            }
        }

        public override LispNode GetNodeCopy() {
            return new LispDataList {
                Nested = this.Nested.Select(x => x.GetNodeCopy()).ToList()
            };
        }
    }
}
