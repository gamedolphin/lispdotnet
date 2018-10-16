using System;
using System.Collections.Generic;

namespace LispDotNet {

    public static class LispFuncUtils {
        public static LispEvalSymbol FuncEvaluator = new LispEvalSymbol();
    }

    public delegate LispNode BuiltInFunc(LispEnvironment env,LispNode node);

    public class LispFunction : LispNode {

        public override LispNodeType NodeType { get; } = LispNodeType.FUNCTION;

        public override string Contents { get; } = "<built-in function>";
        public BuiltInFunc Apply;

        public LispFunction(BuiltInFunc fun = null) {
            Apply = fun;
        }

        public virtual LispNode CallFunction(LispEnvironment env, LispNode node) {
            if(Apply != null) {
                return Apply(env,node);
            }
            else {
                return new LispNotSymbolException();
            }
        }

        public override LispNode GetNodeCopy() {
            return new LispFunction(Apply);
        }
    }

    public class UserDefinedFunction : LispFunction {
        public override LispNodeType NodeType { get; } = LispNodeType.USER_FUNCTION;
        public override string Contents {
            get {
                return this.GetType().ToString()+" (\\ "+formals.ToString() + " " + body.ToString() + ")";
            }
        }

        public LispEnvironment FuncEnv;
        private LispNode formals;
        private LispNode body;

        public UserDefinedFunction(LispNode _formals, LispNode _body)  {
            FuncEnv = new LispEnvironment(null);
            formals = _formals;
            body = _body;
        }

        public UserDefinedFunction(UserDefinedFunction fn) {
            FuncEnv = fn.FuncEnv;
            formals = fn.formals;
            body = fn.body;
        }

        public override LispNode GetNodeCopy() {
            var fn =  new UserDefinedFunction(formals.GetNodeCopy(), body.GetNodeCopy());
            fn.FuncEnv = FuncEnv.GetCopy();
            return  fn;
        }

        public override LispNode CallFunction(LispEnvironment env, LispNode node) {

            int given = node.Nested.Count;
            int total = formals.Nested.Count;

            while(node.Nested.Count > 0) {
                if(formals.Nested.Count == 0) {
                    return new LispTooManyArgsException(Contents, total, given);
                }

                var sym = formals.Pop() as InputSymbol;
                var val = node.Pop();

                FuncEnv.Add(sym.Symbol,val);
            }

            node.Nested.Clear();

            if(formals.Nested.Count == 0) {
                FuncEnv.parentEnv = env;
                return Interpreter.Evaluate(FuncEnv,new LispList {
                    Nested = body.GetNodeCopy().Nested
                });
            }
            else {
                return GetNodeCopy();
            }

            // int index = 0;
            // node.Nested.ForEach(n => {
            //         var sym = formals.Nested[index++] as InputSymbol;
            //         FuncEnv.Add(sym.Symbol,n);
            //     });

            // node.Nested.Clear();

            // FuncEnv.parentEnv = env;


        }
    }

}
