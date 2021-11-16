using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiny_Compiler
{
    //Compiler For Tiny Programming Language [TPL]
    public static class Compiler
    {
        // Declarations for Compiling and Pre-Processing
        public static List<String> Syntax_Errors = new List<String>();
        public static Scanner SC = new Scanner();
        public static List<Token> Tokens_List = new List<Token>();
        // Compiling Function Calling The Scan
        public static void Compile(String SRC)
        {
            // Start Scanning The Source Code For Token_Classes Identification
            SC.Scan(SRC);
        }
    }
}
