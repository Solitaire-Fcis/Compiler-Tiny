using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// All Token Classes As Enumerations
public enum Token_Class
{
   DATATYPE_INT,DATATYPE_FLOAT,DATATYPE_STRING,NUMBER,STRING,READ,WRITE,REPEAT,UNTIL,IF,ELSEIF,ELSE,RETURN,ENDL,
    IDENTIFIER,PROGRAM,FUNCTION,PLUSOP,MINUSOP,MULOP,DIVOP,LTOP,MTOP,EQOP,
    NEQOP,COMMA,SEMICOLON,DOT,LBRACES,RBRACES,LPARENT,RPARENT,ASSIGN,COMMENT,NA
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
        public static List<Token> Tokens_List = new List<Token>();
        public static Dictionary<String, Token_Class> Reserved_Keys_List = new Dictionary<string, Token_Class>();
        public static Dictionary<String, Token_Class> Operators_List = new Dictionary<string, Token_Class>();
        
        public Scanner()
        {
            // Associating reserved characters/words with their own Token class
            Reserved_Keys_List.Add("int", Token_Class.DATATYPE_INT);
            Reserved_Keys_List.Add("float", Token_Class.DATATYPE_FLOAT);
            Reserved_Keys_List.Add("string", Token_Class.DATATYPE_STRING);
            Reserved_Keys_List.Add("if", Token_Class.IF);
            Reserved_Keys_List.Add("elseif", Token_Class.ELSEIF);
            Reserved_Keys_List.Add("else", Token_Class.ELSE);
            Reserved_Keys_List.Add("write", Token_Class.WRITE);
            Reserved_Keys_List.Add("read", Token_Class.READ);
            Reserved_Keys_List.Add("repeat", Token_Class.REPEAT);
            Reserved_Keys_List.Add("until", Token_Class.UNTIL);
            Reserved_Keys_List.Add("endl", Token_Class.ENDL);
            Reserved_Keys_List.Add("return", Token_Class.RETURN);

            Operators_List.Add(",", Token_Class.COMMA);
            Operators_List.Add(";", Token_Class.SEMICOLON);
            Operators_List.Add(".", Token_Class.DOT);
            Operators_List.Add("+", Token_Class.PLUSOP);
            Operators_List.Add("-", Token_Class.MINUSOP);
            Operators_List.Add("/", Token_Class.DIVOP);
            Operators_List.Add("*", Token_Class.MULOP);
            Operators_List.Add("=", Token_Class.EQOP);
            Operators_List.Add(":=", Token_Class.ASSIGN);
            Operators_List.Add("(", Token_Class.LPARENT);
            Operators_List.Add(")", Token_Class.RPARENT);
            Operators_List.Add("{", Token_Class.LBRACES);
            Operators_List.Add("}", Token_Class.RBRACES);
        }
        // Scanning function for identifying and attaching lexemes with token_classes
        public void Scan(String SRC)
        {
            // int main;
            for(int i = 0; i < SRC.Length; i++)
            {
                int j = i;
                char Present_Character = SRC[i];
                String Lex = Present_Character.ToString();
                if (Present_Character == ' ' || Present_Character == '\r' || Present_Character == '\n')
                    continue;
                else if (char.IsLetter(Present_Character))
                {
                    j++;
                    while (char.IsLetterOrDigit(SRC[j]))
                    {
                        Lex += SRC[j].ToString();
                        j++;
                    }
                    j--;
                }
                else if (char.IsDigit(Present_Character))
                {
                    j++;
                    while (char.IsDigit(SRC[j]))
                    {
                        Lex += SRC[j].ToString();
                        j++;
                    }
                    j--;
                }
                else if(Present_Character == '/' && SRC[j + 1] == '*')
                {
                    j++;
                    while (SRC.Length != j)
                    {
                        Lex += SRC[j].ToString();
                        if (SRC[j] == '/')
                            break;
                        j++;
                    }
                }
                else if (Present_Character == '\"')
                {
                    j += 1;
                    while (SRC.Length != j)
                    {
                        Lex += SRC[j].ToString();
                        if (SRC[j] == '\"')
                            break;
                        j++;
                    }
                }
                else if(Present_Character == ':' && SRC[++j] == '=')
                    Lex += SRC[j].ToString();
                i = j;
                FindTokenClass(Lex);
            }
            Compiler.Tokens_List = Tokens_List;
        }
        public static void FindTokenClass(String Lex)
        {
            bool NA = false;
            Token token = new Token();
            token.lex = Lex;
            // Checking If Reserved Keyword
            if (isReserved_Keyword(Lex))
                token.token_type = Reserved_Keys_List[Lex];
            // Checking If Identifier
            else if (isIdentifier(Lex))
                token.token_type = Token_Class.IDENTIFIER;
            // Checking If Constant
            else if (isConstant(Lex))
                token.token_type = Token_Class.NUMBER;
            // Checking If Operator
            else if (isOperator(Lex))
                token.token_type = Operators_List[Lex];
            // Checking If Comment
            else if (isComment(Lex))
                token.token_type = Token_Class.COMMENT;
            // Checking If String Value
            else if (isString(Lex))
                token.token_type = Token_Class.STRING;
            // Unidentfied Lexeme
            else
            {
                token.token_type = Token_Class.NA;
                Compiler.Syntax_Errors.Add(Lex);
                NA = true;
            }
            if(!NA)
                Tokens_List.Add(token);
        }
        public static bool isIdentifier(String Lex)
        {
            bool Valid = true;
            if (char.IsLetter(Lex[0]))
            {
                int i = 0;
                while (Lex.Length > i)
                {
                    if (!char.IsLetterOrDigit(Lex[i]))
                        Valid = false;
                    i++;
                }
            }
            else
                Valid = false;
            return Valid;
        }
        public static bool isOperator(String Lex)
        {
            if (Operators_List.ContainsKey(Lex))
                return true;
            return false;
        }
        public static bool isConstant(String Lex)
        {
            bool Valid = true;
            int i = 0;
            while (Lex.Length > i)
            {
                if (!char.IsDigit(Lex[i]))
                    Valid = false;
                i++;
            }
            return Valid;
        }
        public static bool isReserved_Keyword(String Lex)
        {
            if (Reserved_Keys_List.ContainsKey(Lex))
                return true;
            return false;
        }
        public static bool isComment(String Lex)
        {
            if (Lex[0] == '/' && Lex[1] == '*' && Lex[Lex.Length - 2] == '*' && Lex[Lex.Length - 1] == '/')
                return true;
            return false;

        }
        public static bool isString(String Lex)
        {
            if (Lex[0] == '\"' && Lex[Lex.Length-1] == '\"')
                return true;
            return false;
        }
    }
}
