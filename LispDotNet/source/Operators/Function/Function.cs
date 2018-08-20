using System.Linq;

namespace LispDotNet {

    [SymbolAttribute("\\")]
    public class LispLambdaSymbol : LispSymbol {

        public override string Contents { get; } = "\\";
        public override string Symbol { get; } = "\\";

        public override LispNode Operate(LispEnvironment env, LispNode node) {

            if(node.Nested.Count != 2) {
                return new LispTooManyArgsException(Contents,2,node.Nested.Count);
            }

            var f = node.Pop() as LispDataList;

            if(f == null) return new LispIncorrectArgTypesException(Contents,0,LispNodeType.DATALIST.ToString(),node.Nested[0].NodeType.ToString());

            var b = node.Pop() as LispDataList;

            if(b == null) return new LispIncorrectArgTypesException(Contents,0,LispNodeType.DATALIST.ToString(),node.Nested[1].NodeType.ToString());

            node.Nested.Clear();

            if(!f.Nested.All(n => n is InputSymbol)) {
                return new LispNonSymbolException(Contents);
            }

            return new UserDefinedFunction(f,b);
        }
    }

}
