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
        public static List<Token> Tokens_List = new List<Token>();
        // Compiling Function Calling The Scan
        public static void Compile(String SRC)
        {
            // Start Scanning The Source Code For Token_Classes Identification
            SC.Scan(SRC);
        }
        // Pre-Process Source Code For Identifying Lexemes
        public static void Gather_Lexemes(String SRC)
        {
            string[] Lexemes_arr = SRC.Split(' ');
            for (int i = 0; i < Lexemes_arr.Length; i++)
            {
                if (Lexemes_arr[i].Contains("\r\n"))
                {
                    Lexemes_arr[i] = Lexemes_arr[i].Replace("\r\n", string.Empty);
                }
                Lexemes.Add(Lexemes_arr[i]);
            }
        }

    }
}
