using System.Collections.Generic;
using System.Linq;
using System;

namespace LispDotNet {

    [SymbolAttribute("list")]
    public class LispListSymbol : LispSymbol {
        public override string Contents { get; } = "list";
        public override string Symbol { get; } = "list";

        public override LispNode Operate(LispEnvironment env,LispNode node) {

            var dnode = node as LispList;

            if(dnode == null) return new LispIncorrectArgTypesException(Contents,0,LispNodeType.LIST.ToString(),node.NodeType.ToString());

            return new LispDataList {
                Nested = dnode.Nested
            };
        }
    }

    [SymbolAttribute("head")]
    public class LispHeadSymbol : LispSymbol {
        public override string Contents { get; } = "head";
        public override string Symbol { get; } = "head";

        public override LispNode Operate(LispEnvironment env,LispNode node) {

            if(node.Nested.Count != 1) {
                return new LispTooManyArgsException(Contents,1,node.Nested.Count);
            }

            var dnode = node.Nested[0] as LispDataList;

            if(dnode == null) return new LispIncorrectArgTypesException(Contents,0,LispNodeType.DATALIST.ToString(),node.NodeType.ToString());

            if(dnode.Nested.Count == 0) {
                return new LispEmptyDataException(Contents);
            }

            var result = dnode;

            if(result.Nested.Count > 1) {
                result.Nested.RemoveRange(1, result.Nested.Count - 1);
            }

            return result;
        }
    }

    [SymbolAttribute("tail")]
    public class LispTailSymbol : LispSymbol {
        public override string Contents { get; } = "tail";
        public override string Symbol { get; } = "tail";

        public override LispNode Operate(LispEnvironment env,LispNode node) {

            if(node.Nested.Count != 1) {
                return new LispTooManyArgsException(Contents,1,node.Nested.Count);
            }

            var dnode = node.Nested[0] as LispDataList;

            if(dnode == null) return new LispIncorrectArgTypesException(Contents,0,LispNodeType.DATALIST.ToString(),node.NodeType.ToString());

            if(dnode.Nested.Count == 0) {
                return new LispEmptyDataException(Contents);
            }

            var result = dnode;

            result.Pop();

            return result;
        }
    }



    [SymbolAttribute("join")]
    public class LispJoinSymbol : LispSymbol {
        public override string Contents { get; } = "join";
        public override string Symbol { get; } = "join";

        public override LispNode Operate(LispEnvironment env,LispNode node) {

            if(!node.Nested.All(item => item is LispDataList)) {
                node.Nested.Clear();
                return new LispNotDataListException(Contents);
            }

            var x = node.Pop();

            while(node.Nested.Count > 0) {
                x = LispNodeUtils.Join(x,node.Pop());
            }

            return x;
        }
    }

    [SymbolAttribute("eval")]
    public class LispEvalSymbol : LispSymbol {
        public override string Contents { get; } = "eval";
        public override string Symbol { get; } = "eval";

        public override LispNode Operate(LispEnvironment env,LispNode node) {

            if(node.Nested.Count != 1) {
                return new LispTooManyArgsException(Contents,1,node.Nested.Count);
            }

            var dnode = node.Take(0) as LispDataList;

            if(dnode == null) return new LispIncorrectArgTypesException(Contents,0,LispNodeType.DATALIST.ToString(),node.NodeType.ToString());


            var x = new LispList {
                Nested = dnode.GetNodeCopy().Nested
            };

            return Interpreter.Evaluate(env,x);
        }
    }
}
