using System.Linq;

namespace LispDotNet {

    public static class Interpreter {

        private static LispNode EvaluateExpr (LispEnvironment env, LispNode node) {

            node.Nested = node
                .Nested
                .ConvertAll(item => Evaluate(env,item));

            if(node.Nested.Any(item => item is LispError)) {
                var item = node.Nested.First(err => err is LispError);
                node.Nested.Clear();
                return item;
            }

            // Empty Expression
            if(node.Nested.Count == 0) return node;

            // Single value expression
            if(node.Nested.Count == 1) return node.Take(0);


            var op = node.Pop() as LispFunction;

            if(op == null) {
                node.Nested.Clear();
                return new LispNotSymbolException();
            }

            return op.CallFunction(env,node);
        }

        public static LispNode Evaluate (LispEnvironment env, LispNode node) {

            if(node is InputSymbol) {
                return env.Get(((InputSymbol)node).Symbol);
            }

            if(node is LispList) {
                return EvaluateExpr(env,node);
            }

            return node;
        }

    }

}
