using System;
using System.Collections.Generic;
using Pidgin;
using static Pidgin.Parser;
using System.Linq;

namespace LispDotNet {

    class Program {

        static void Main (string[] args) {
            Console.WriteLine ("LispDotNet Version 0.0.0.1");
            Console.WriteLine ("Press Ctrl+c to Exit\n");

            string input;
            // List<string> history = new List<string>();

            var p = GrammarUtil.GetParser ();

            while(true) {
                Console.Write("LispDotNet>");

                input = Console.ReadLine();

                // history.Add(input);

                try {
                     var c = p.ParseOrThrow(input);
                     Console.WriteLine(GrammarUtil.Evaluate(c));
                }
                catch(Exception ex) {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
