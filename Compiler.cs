using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    //Compiler For Tiny Programming Language [TPL]
    public static class Compiler
    {
        // Declarations for Compiling and Pre-Processing
        public static List<String> Syntax_Errors = new List<String>();
        public static Scanner SC = new Scanner();
        public static List<String> Lexemes = new List<String>();
        public static List<Token> TokenStream = new List<Token>();
        // Compiling Function Calling The Scan
        public static void Compile()
        {

        }
        // Pre-Process Source Code For Identifying Lexemes
        public static void Gather_Lexemes()
        {

        }

    }
}
