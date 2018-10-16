using System;
using Pidgin;

namespace LispDotNet {

    using static Interpreter;
    using static GrammarUtil;
    using static EnvUtils;

    class Program {

        static void Main (string[] args) {
            Console.WriteLine ("LispDotNet Version 0.0.0.1");
            Console.WriteLine ("Press Ctrl+c to Exit\n");

            string input;
            // List<string> history = new List<string>();

            var env = GetEnvironment();
            var p = GetParser ();

            while(true) {
                Console.Write("LispDotNet>");

                input = Console.ReadLine();

                // history.Add(input);

                try {
                     var c = p.ParseOrThrow(input);
                     Console.WriteLine(Evaluate(env,c));
                }
                catch(Exception ex) {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
