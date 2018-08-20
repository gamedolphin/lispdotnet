using System.Linq;

namespace LispDotNet {
    public abstract class LispAssignSymbol : LispSymbol {

        public abstract bool GlobalAssign { get; }

        public override LispNode Operate(LispEnvironment env, LispNode node) {

            var dnode = node.Nested[0] as LispDataList;

            var defs = node.Nested.Skip(0).ToList();

            if(dnode == null) return new LispIncorrectArgTypesException(Contents,0,LispNodeType.DATALIST.ToString(),node.NodeType.ToString());

            if(!dnode.Nested.All(n => n is InputSymbol)) {
                return new LispNonSymbolException(Contents);
            }

            if(dnode.Nested.Count != node.Nested.Count - 1) {
                return new LispIncorrectNumberToDefineException(Contents);
            }

            int index = 1;
            dnode.Nested.ForEach((n) => {
                    var sym = n as InputSymbol;
                    if(GlobalAssign) {
                        env.AddGlobal(sym.Symbol, node.Nested[index++]);
                    }
                    else {
                        env.Add(sym.Symbol, node.Nested[index++]);
                    }
                });

            return new LispList();
        }
    }

    [SymbolAttribute("def")]
    public class LispDefSymbol : LispAssignSymbol {
        public override string Contents { get; } = "def";
        public override string Symbol { get; } = "def";
        public override bool GlobalAssign { get; } = true;
    }

    [SymbolAttribute("=")]
    public class LispEqSymbol : LispAssignSymbol {
        public override string Contents { get; } = "=";
        public override string Symbol { get; } = "=";
        public override bool GlobalAssign { get; } = false;
    }
}
