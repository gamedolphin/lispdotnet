using System.Collections.Generic;
using System;
using System.Linq;

namespace LispDotNet {

    public static class EnvUtils {

        private static List<LispSymbol> SymbolList = new List<LispSymbol> {
            new LispPrintSymbol(),

            //Math
            new LispAddSymbol(),
            new LispSubtractSymbol(),
            new LispMultiplySymbol(),
            new LispDivideSymbol(),

            //Data
            new LispListSymbol(),
            new LispHeadSymbol(),
            new LispTailSymbol(),
            new LispJoinSymbol(),
            new LispEvalSymbol(),

            //Variables
            new LispDefSymbol(),
            new LispEqSymbol(),

            //Function
            new LispLambdaSymbol()
        };

        private static Dictionary<string,LispNode> GetBuiltInFunctions() {
            var dic = new Dictionary<string,LispNode>();
            SymbolList.ForEach(ty => {
                    try {
                        dic.Add(ty.Symbol,new LispFunction(ty.Operate));
                    }
                    catch(Exception ex) {
                        Console.WriteLine(ex);
                    }
                });
            return dic;
        }

        public static LispEnvironment GetEnvironment() {
            var env = new LispEnvironment(null);
            env.Env = GetBuiltInFunctions();
            return env;
        }

    }

    public class LispEnvironment {

        public LispEnvironment parentEnv = null;

        public Dictionary<string, LispNode> Env = new Dictionary<string, LispNode> ();

        public LispEnvironment(LispEnvironment parent) {
            parentEnv = parent;
        }

        public LispEnvironment GetCopy () {
            var en = new LispEnvironment(parentEnv);


            foreach(KeyValuePair<string, LispNode> entry in Env)
            {
                en.Env.Add(entry.Key,entry.Value.GetNodeCopy());
            }

            return en;
        }

        public void Add(string sym, LispNode node) {
            if(Env.ContainsKey(sym)) {
                Env.Remove(sym);
            }
            Env[sym] = node;
        }

        public void AddGlobal(string sym, LispNode node) {

            var global = this;

            while(global.parentEnv != null) {
                global = global.parentEnv;
            }

            global.Add(sym,node);
        }

        public LispNode Get(string sym) {
            if(Env.ContainsKey(sym)) {
                return Env[sym].GetNodeCopy();
            }

            if(parentEnv != null) {
                return parentEnv.Get(sym);
            }

            return new LispNotSymbolException(sym);
        }
    }
}
