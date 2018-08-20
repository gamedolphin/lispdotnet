using System;
using Pidgin;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
            Parser<char,char> Arithmetic = OneOf(Char('+'),Char('-'),Char('*'),Char('/'));
            Parser<char,char> OtherChars = OneOf(Char('\\'),Char('='),Char('<'),Char('>'),Char('!'),Char('&'));

            Parser<char,LispNode> Num = Digit.AtLeastOnceString().Select<LispNode>(ex => new LispNumber (ex));

            Parser<char,LispNode> Symbol = OneOf(Letter,
                                                 Arithmetic,
                                                 OtherChars)
                .Then((OneOf(LetterOrDigit,Arithmetic,OtherChars)).ManyString(),
                      (h,t) => h + t)
                .Select<LispNode>(ex => new InputSymbol(ex));

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
    }
}
