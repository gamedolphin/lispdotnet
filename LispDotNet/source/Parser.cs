using System;
using Pidgin;
using static Pidgin.Parser;
using System.Collections.Generic;
using System.Linq;

namespace LispDotNet {

    public static class GrammarUtil {

        private static List<Type> SymbolList = new List<Type> {
            typeof(LispPrintSymbol),

            //Math
            typeof(LispAddSymbol),
            typeof(LispSubtractSymbol),
            typeof(LispMultiplySymbol),
            typeof(LispDivideSymbol),

            //Data
            typeof(LispListSymbol),
            typeof(LispHeadSymbol),
            typeof(LispTailSymbol),
            typeof(LispJoinSymbol),
            typeof(LispEvalSymbol),
        };

        private static Dictionary<string,Type> GetDictionary() {
            var dic = new Dictionary<string,Type>();
            SymbolList.ForEach(ty => {
                    var attr = ty.GetCustomAttributes(typeof(SymbolAttribute), true) as SymbolAttribute[];
                    if(attr.Length > 0) {
                        dic.Add(attr[0].SymbolType, ty);
                    }
                });
            return dic;
        }

        private static Dictionary<string,Type> SymbolDictionary = GetDictionary();

        private static LispNode GetLispSymbol (string symbolType) {
            return System.Activator.CreateInstance(SymbolDictionary[symbolType]) as LispNode;
        }

        public static Parser<char,LispNode> GetParser() {
            Parser<char,char> LPar = Char('(');
            Parser<char,char> RPar = Char(')');
            Parser<char,char> LBrace = Char('{');
            Parser<char,char> RBrace = Char('}');
            Parser<char,LispNode> Symbol = OneOf(SymbolDictionary.Keys
                                                 .Select(ex => String(ex)))
                .Select<LispNode>(ex =>  GetLispSymbol(ex));
            Parser<char,LispNode> Num = Digit.AtLeastOnceString().Select<LispNode>(ex => new LispNumber (ex));
            Parser<char,LispNode> List = null;
            Parser<char,LispNode> Data = null;
            Parser<char,LispNode> Atom = OneOf(Symbol,Num,Rec(() => List), Rec(() => Data));

            List = Atom.Between(SkipWhitespaces).Separated(Whitespace.Many()).Between(LPar,RPar).Select<LispNode>(ex => {
                    return new LispList {
                        Nested = new List<LispNode>(ex)
                    };
                });

            Data = Atom.Between(SkipWhitespaces).Separated(Whitespace.Many()).Between(LBrace,RBrace).Select<LispNode>(ex => {
                    return new LispDataList {
                        Nested = new List<LispNode>(ex)
                    };
                });

            Parser<char,LispNode> Expr = OneOf(Num,List,Data,Symbol);

            return Expr;
        }

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

        private static LispNode EvaluateExpr (LispNode node) {

            node.Nested = node.Nested.ConvertAll(item => Evaluate(item));

            if(node.Nested.Any(item => item is LispError)) {
                var item = node.Nested.First(err => err is LispError);
                node.Nested.Clear();
                return item;
            }

            // Empty Expression
            if(node.Nested.Count == 0) return node;

            // Single value expression
            if(node.Nested.Count == 1) return node.Pop();


            var op = node.Pop() as LispSymbol;

            if(op == null) {
                node.Nested.Clear();
                return new LispNotSymbolException();
            }

            return op.Operate(node);
        }

        public static LispNode Evaluate (LispNode node) {

            if(node is LispList) {
                return EvaluateExpr(node);
            }

            return node;
        }
    }
}
