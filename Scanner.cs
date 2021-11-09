using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// All Token Classes As Enumerations
public enum Token_Class
{
   INT,FLOAT,STRING,READ,WRITE,REPEAT,UNTIL,IF,ELSEIF,ELSE,RETURN,ENDL,
    IDENTIFIER,PROGRAM,FUNCTION,PLUSOP,MINUSOP,MULOP,DIVOP,LTOP,MTOP,EQOP,
    NEQOP,COMMA,SEMICOLON,DOT,LBRACES,RBRACES,LPARENT,RPARENT
}
namespace WindowsFormsApp1
{
    // Token Class
    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }
    // Scanner Class 
    public class Scanner
    {
        // Declarations of DSes that will be used through scanning
        public List<Token> Tokens = new List<Token>();
        public static Dictionary<String, Token_Class> res_keys = new Dictionary<string, Token_Class>();
        public static Dictionary<String, Token_Class> ops = new Dictionary<string, Token_Class>();
        
        public Scanner()
        {
            // Associating reserved characters/words with their own Token class
            res_keys.Add("int", Token_Class.INT);
            res_keys.Add("float", Token_Class.FLOAT);
            res_keys.Add("string", Token_Class.STRING);
            res_keys.Add("if", Token_Class.IF);
            res_keys.Add("elseif", Token_Class.ELSEIF);
            res_keys.Add("else", Token_Class.ELSE);
            res_keys.Add("write", Token_Class.WRITE);
            res_keys.Add("read", Token_Class.READ);
            res_keys.Add("repeat", Token_Class.REPEAT);
            res_keys.Add("until", Token_Class.UNTIL);
            res_keys.Add("endl", Token_Class.ENDL);
            res_keys.Add("return", Token_Class.RETURN);

            ops.Add(",", Token_Class.COMMA);
            ops.Add(";", Token_Class.SEMICOLON);
            ops.Add(".", Token_Class.DOT);
            ops.Add("+", Token_Class.PLUSOP);
            ops.Add("-", Token_Class.MINUSOP);
            ops.Add("/", Token_Class.DIVOP);
            ops.Add("*", Token_Class.MULOP);
            ops.Add("(", Token_Class.LPARENT);
            ops.Add(")", Token_Class.RPARENT);
            ops.Add("{", Token_Class.LBRACES);
            ops.Add("}", Token_Class.RBRACES);
        }
        // Scanning function for identifying and attaching lexemes with token_classes
        public static void Scan()
        {

        }
    }
}
