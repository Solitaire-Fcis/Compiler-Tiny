using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiny_Compiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;
        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }
        Node Program()
        {
            Node program = new Node("Program");
            if (!((TokenStream[InputPointer].token_type == Token_Class.DATATYPE_FLOAT ||
                TokenStream[InputPointer].token_type == Token_Class.DATATYPE_INT ||
                TokenStream[InputPointer].token_type == Token_Class.DATATYPE_STRING) && 
                TokenStream[InputPointer+1].token_type == Token_Class.MAIN))
                program.Children.Add(Fun_statD());
            program.Children.Add(Main());
            return program;
        }
        Node Fun_statD()
        {
            Node Fun_statDN = new Node("Functions");
            Fun_statDN.Children.Add(Fun_stat());
            Fun_statDN.Children.Add(Fun_statD());
            return Fun_statDN;
        }
        Node Main()
        {
            Node main = new Node("Main");
            if(TokenStream[InputPointer].token_type == Token_Class.DATATYPE_FLOAT)
                main.Children.Add(match(Token_Class.DATATYPE_FLOAT));
            else if(TokenStream[InputPointer].token_type == Token_Class.DATATYPE_INT)
                main.Children.Add(match(Token_Class.DATATYPE_INT));
            else if (TokenStream[InputPointer].token_type == Token_Class.DATATYPE_STRING)
                main.Children.Add(match(Token_Class.DATATYPE_STRING));
            if(TokenStream[InputPointer].token_type == Token_Class.MAIN)
                main.Children.Add(match(Token_Class.MAIN));
            if(TokenStream[InputPointer].token_type == Token_Class.LPARENT)
                main.Children.Add(match(Token_Class.LPARENT));
            if(TokenStream[InputPointer].token_type == Token_Class.RPARENT)
                main.Children.Add(match(Token_Class.RPARENT));
            main.Children.Add(Fun_Bod());
            return main;
        }
        Node Fun_stat()
        {
            Node Fun_statN = new Node("Function");
            Fun_statN.Children.Add(Fun_Decl());
            Fun_statN.Children.Add(Fun_Bod());
            return Fun_statN;
        }

        Node Fun_Bod()
        {
            Node Fun_BodN = new Node("Function Body");
            if(TokenStream[InputPointer].token_type == Token_Class.LBRACES)
                Fun_BodN.Children.Add(match(Token_Class.LBRACES));
            Fun_BodN.Children.Add(Stats_SetD());
            if(TokenStream[InputPointer].token_type == Token_Class.RETURN)
                Fun_BodN.Children.Add(match(Token_Class.RETURN));
            if(TokenStream[InputPointer].token_type == Token_Class.RBRACES)
                Fun_BodN.Children.Add(match(Token_Class.RBRACES));
            return Fun_BodN;
        }

        Node Fun_Decl()
        {
            Node Fun_DeclN = new Node("Function Declaration");
            if(TokenStream[InputPointer].token_type == Token_Class.DATATYPE_FLOAT)
                Fun_DeclN.Children.Add(match(Token_Class.DATATYPE_FLOAT));
            if (TokenStream[InputPointer].token_type == Token_Class.DATATYPE_INT)
                Fun_DeclN.Children.Add(match(Token_Class.DATATYPE_INT));
            if (TokenStream[InputPointer].token_type == Token_Class.DATATYPE_STRING)
                Fun_DeclN.Children.Add(match(Token_Class.DATATYPE_STRING));
            if (TokenStream[InputPointer].token_type == Token_Class.IDENTIFIER)
                Fun_DeclN.Children.Add(match(Token_Class.IDENTIFIER));
            if (TokenStream[InputPointer].token_type == Token_Class.LPARENT)
                Fun_DeclN.Children.Add(match(Token_Class.LPARENT));
            Fun_DeclN.Children.Add(Para());
            Fun_DeclN.Children.Add(ParaD());
            if (TokenStream[InputPointer].token_type == Token_Class.RPARENT)
                Fun_DeclN.Children.Add(match(Token_Class.RPARENT));
            return Fun_DeclN;
        }

        Node Stats_SetD()
        {
            Node Stats_SetDN = new Node("Statements");
            Node tmpStats = Stats_Set();
            Stats_SetDN.Children.Add(tmpStats);
            if(!(tmpStats == null))
                Stats_SetDN.Children.Add(Stats_SetD());
            return Stats_SetDN;
        }
        Node Stats_Set()
        {
            Node Stats_SetN = new Node("Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.IDENTIFIER)
                Stats_SetN.Children.Add(Assign_Stat());
            else if (TokenStream[InputPointer].token_type == Token_Class.WRITE)
                Stats_SetN.Children.Add(Write());
            else if (TokenStream[InputPointer].token_type == Token_Class.READ)
                Stats_SetN.Children.Add(Read());
            else if (TokenStream[InputPointer].token_type == Token_Class.RETURN)
                Stats_SetN.Children.Add(Return());
            else if (TokenStream[InputPointer].token_type == Token_Class.IF)
                Stats_SetN.Children.Add(If_Stat());
            else if (TokenStream[InputPointer].token_type == Token_Class.ELSEIF)
                Stats_SetN.Children.Add(ElseIF_Stat());
            else if (TokenStream[InputPointer].token_type == Token_Class.ELSE)
                Stats_SetN.Children.Add(Else_StatD());
            else if (TokenStream[InputPointer].token_type == Token_Class.DATATYPE_FLOAT || TokenStream[InputPointer].token_type == Token_Class.DATATYPE_STRING || TokenStream[InputPointer].token_type == Token_Class.DATATYPE_INT)
                Stats_SetN.Children.Add(Decl_Stat());
            else
                Stats_SetN = null;
            return Stats_SetN;
        }

        Node Write()
        {
            Node WriteN = new Node("Write Statement");
            WriteN.Children.Add(match(Token_Class.WRITE));
            if (TokenStream[InputPointer].token_type == Token_Class.ENDL)
            {
                WriteN.Children.Add(match(Token_Class.ENDL));
                if (TokenStream[InputPointer].token_type == Token_Class.SEMICOLON)
                    WriteN.Children.Add(match(Token_Class.SEMICOLON));
            }
            else
            {
                WriteN.Children.Add(Exp());
                if (TokenStream[InputPointer].token_type == Token_Class.SEMICOLON)
                    WriteN.Children.Add(match(Token_Class.SEMICOLON));
            }
            return WriteN;
        }
        Node Return()
        {
            Node ReturnN = new Node("Return Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.RETURN)
                ReturnN.Children.Add(match(Token_Class.RETURN));
            ReturnN.Children.Add(Exp());
            if (TokenStream[InputPointer].token_type == Token_Class.SEMICOLON)
                ReturnN.Children.Add(match(Token_Class.SEMICOLON));
            return ReturnN;
        }
        Node Read()
        {
            Node ReadN = new Node("Read Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.READ)
                ReadN.Children.Add(match(Token_Class.READ));
            if (TokenStream[InputPointer].token_type == Token_Class.IDENTIFIER)
                ReadN.Children.Add(match(Token_Class.IDENTIFIER));
            if (TokenStream[InputPointer].token_type == Token_Class.SEMICOLON)
                ReadN.Children.Add(match(Token_Class.SEMICOLON));
            return ReadN;
        }
        Node Assign_Stat()
        {
            Node Assign_StatN = new Node("Assignment Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.IDENTIFIER)
                Assign_StatN.Children.Add(match(Token_Class.IDENTIFIER));
            if (TokenStream[InputPointer].token_type == Token_Class.ASSIGN)
                Assign_StatN.Children.Add(match(Token_Class.ASSIGN));
            Assign_StatN.Children.Add(Exp());
            if (TokenStream[InputPointer].token_type == Token_Class.SEMICOLON)
                Assign_StatN.Children.Add(match(Token_Class.SEMICOLON));
            return Assign_StatN;
        }
        Node Exp()
        {
            Node exp = new Node("Expression");
            if (TokenStream[InputPointer].token_type == Token_Class.STRING)
                exp.Children.Add(match(Token_Class.STRING));
            else if (TokenStream[InputPointer].token_type == Token_Class.NUMBER || TokenStream[InputPointer].token_type == Token_Class.IDENTIFIER)
                exp.Children.Add(Term());
            else
                exp.Children.Add(Eq());
            return exp;
        }

        Node Eq()
        {
            Node eq = new Node("Equation");

            if (TokenStream[InputPointer].token_type == Token_Class.NUMBER || TokenStream[InputPointer].token_type == Token_Class.IDENTIFIER)
                eq.Children.Add(Term());
            else if (TokenStream[InputPointer].token_type == Token_Class.LPARENT)
            {
                eq.Children.Add(match(Token_Class.LPARENT));
                eq.Children.Add(Eq());
                eq.Children.Add(match(Token_Class.RPARENT));
                eq.Children.Add(EqD());
            }
            else
            {
                eq.Children.Add(Eq());
                eq.Children.Add(A_Operator());
                eq.Children.Add(Eq());
            }

            return eq;
        }

        Node A_Operator()
        {
            Node a_operator = new Node("Arithmetic Operator");
            if (TokenStream[InputPointer].token_type == Token_Class.PLUSOP)
                a_operator.Children.Add(match(Token_Class.PLUSOP));
            else if (TokenStream[InputPointer].token_type == Token_Class.MINUSOP)
                a_operator.Children.Add(match(Token_Class.MINUSOP));
            else if (TokenStream[InputPointer].token_type == Token_Class.DIVOP)
                a_operator.Children.Add(match(Token_Class.DIVOP));
            else
                a_operator.Children.Add(match(Token_Class.MULOP));
            return a_operator;
        }

        Node EqD()
        {
            Node eqd = new Node("Equations");
            eqd.Children.Add(A_Operator());
            eqd.Children.Add(match(Token_Class.LPARENT));
            eqd.Children.Add(Eq());
            eqd.Children.Add(match(Token_Class.RPARENT));
            return eqd;
        }
        Node Term()
        {
            Node term = new Node("Term");
            if (TokenStream[InputPointer].token_type == Token_Class.NUMBER)
            {
                term.Children.Add(match(Token_Class.NUMBER));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.IDENTIFIER)
            {
                term.Children.Add(match(Token_Class.IDENTIFIER));
            }
            else
                term.Children.Add(Fun_Call());
            return term;
        }

        Node Fun_Call()
        {
            Node fun_call = new Node("Function Call");
            
            fun_call.Children.Add(match(Token_Class.IDENTIFIER));
            fun_call.Children.Add(match(Token_Class.LPARENT));
            fun_call.Children.Add(match(Token_Class.IDENTIFIER));
            fun_call.Children.Add(Id_Arg());
            fun_call.Children.Add(match(Token_Class.RPARENT));
            fun_call.Children.Add(match(Token_Class.SEMICOLON));

            return fun_call;
        }

        Node Id_Arg()
        {
            Node id_arg = new Node("Argument");

            if (TokenStream[InputPointer+1].token_type != Token_Class.COMMA)
            {
                id_arg.Children.Add(Id_Arg());
                id_arg.Children.Add(match(Token_Class.COMMA));
                id_arg.Children.Add(match(Token_Class.IDENTIFIER));
            }
            else
                id_arg = null;
            return id_arg;
        }


        Node Decl_Stat()
        {
            Node Decl_StatN = new Node("Declarations Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.DATATYPE_FLOAT)
                Decl_StatN.Children.Add(match(Token_Class.DATATYPE_FLOAT));
            if (TokenStream[InputPointer].token_type == Token_Class.DATATYPE_STRING)
                Decl_StatN.Children.Add(match(Token_Class.DATATYPE_STRING));
            if (TokenStream[InputPointer].token_type == Token_Class.DATATYPE_INT)
                Decl_StatN.Children.Add(match(Token_Class.DATATYPE_INT));
            Decl_StatN.Children.Add(Decl_StatD());
            Decl_StatN.Children.Add(Decl_StatDD());
            return Decl_StatN;
        }
        Node Decl_StatDD()
        {
            Node Decl_StatDDN = new Node("Declaration Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.COMMA)
            { 
                Decl_StatDDN.Children.Add(match(Token_Class.COMMA));
                if (TokenStream[InputPointer].token_type == Token_Class.IDENTIFIER)
                    Decl_StatDDN.Children.Add(match(Token_Class.IDENTIFIER));
                if(TokenStream[InputPointer].token_type == Token_Class.COMMA)
                    Decl_StatDDN.Children.Add(Decl_StatDD());
                else if(TokenStream[InputPointer].token_type == Token_Class.COMMA)
                {
                    if (TokenStream[InputPointer].token_type == Token_Class.ASSIGN)
                        Decl_StatDDN.Children.Add(match(Token_Class.ASSIGN));
                    Decl_StatDDN.Children.Add(Exp());
                    if (TokenStream[InputPointer].token_type == Token_Class.SEMICOLON)
                        Decl_StatDDN.Children.Add(match(Token_Class.SEMICOLON));
                    Decl_StatDDN.Children.Add(Decl_StatDD());
                }
            }
            else
                Decl_StatDDN = null;
                return Decl_StatDDN;
        }
        Node Decl_StatD()
        {
            Node Decl_StatDN = new Node("Declaration Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.IDENTIFIER &&
                !(TokenStream[InputPointer+1].token_type == Token_Class.ASSIGN))
                Decl_StatDN.Children.Add(match(Token_Class.IDENTIFIER));
            else
                Decl_StatDN = Assign_Stat();
            return Decl_StatDN;
        }
        Node Para()
        {
            Node ParaN = new Node("Parameter");
            if (TokenStream[InputPointer].token_type == Token_Class.DATATYPE_FLOAT)
                ParaN.Children.Add(match(Token_Class.DATATYPE_FLOAT));
            if (TokenStream[InputPointer].token_type == Token_Class.DATATYPE_INT)
                ParaN.Children.Add(match(Token_Class.DATATYPE_INT));
            if (TokenStream[InputPointer].token_type == Token_Class.DATATYPE_STRING)
                ParaN.Children.Add(match(Token_Class.DATATYPE_STRING));
            if (TokenStream[InputPointer].token_type == Token_Class.IDENTIFIER)
                ParaN.Children.Add(match(Token_Class.IDENTIFIER));
            return ParaN;
        }
        Node ParaD()
        {
            Node ParaDN = new Node("Parameter");
            if (TokenStream[InputPointer].token_type == Token_Class.COMMA)
            {
                ParaDN.Children.Add(match(Token_Class.COMMA));
                ParaDN.Children.Add(Para());
                ParaDN.Children.Add(ParaD());
            }
            else
                ParaDN = null;
            return ParaDN;
        }

        Node Comment()
        {
            Node comment = new Node("Comment");
            comment.Children.Add(match(Token_Class.COMMENT));
            comment.Children.Add(match(Token_Class.STRING));
            comment.Children.Add(match(Token_Class.COMMENT));
            return comment;
        }

        Node CO()
        {
            Node node = new Node("Condition Operator");

            if (TokenStream[InputPointer].token_type == Token_Class.LTOP)
                node.Children.Add(match(Token_Class.LTOP));
            else if (TokenStream[InputPointer].token_type == Token_Class.MTOP)
                node.Children.Add(match(Token_Class.MTOP));
            else if (TokenStream[InputPointer].token_type == Token_Class.EQOP)
                node.Children.Add(match(Token_Class.EQOP));
            else
            {
                node.Children.Add(match(Token_Class.LTOP));
                node.Children.Add(match(Token_Class.MTOP));
            }

            return node;
        }

        Node Cond()
        {
            Node node = new Node("Condition");

            node.Children.Add(match(Token_Class.IDENTIFIER));
            node.Children.Add(CO());
            node.Children.Add(Term());

            return node;
        }

        Node Cond_Stat()
        {
            Node node = new Node("Condition Statement");

            node.Children.Add(Cond());

            if (TokenStream[InputPointer].token_type == Token_Class.AND
                || TokenStream[InputPointer].token_type == Token_Class.OR)
            {
                node.Children.Add(BO());
                node.Children.Add(Cond());
                node.Children.Add(Cond_Stat());
            }
            else
                node.Children.Add(null);

            return node;
        }

        Node If_Stat()
        {
            Node node = new Node("If Statement");

            node.Children.Add(match(Token_Class.IF));
            node.Children.Add(Cond_Stat());

            // Token class : THEN (NOT FOUND) DONE
            node.Children.Add(match(Token_Class.THEN));

            node.Children.Add(Stats_SetD());
            node.Children.Add(ElseIF_StatD());
            node.Children.Add(Else_StatD());

            // Token Class : END for IF, ElseIF, and Else (NOT FOUND) DONE
            node.Children.Add(match(Token_Class.END));

            return node;
        }

        Node ElseIF_StatD()
        {
            Node node = new Node("Elseif Statements");

            node.Children.Add(ElseIF_StatD());
            node.Children.Add(ElseIF_Stat());

            return node;
        }

        Node ElseIF_Stat()
        {
            Node node = new Node("Elseif Statement");

            node.Children.Add(match(Token_Class.ELSEIF));
            node.Children.Add(Cond());

            // Token class : THEN (NOT FOUND)
            node.Children.Add(match(Token_Class.THEN));

            node.Children.Add(Stats_SetD());
            node.Children.Add(Stats_SetD());
            node.Children.Add(Else_StatD());

            // Token Class : END for IF, ElseIF, and Else (NOT FOUND)
            node.Children.Add(match(Token_Class.END));

            return node;
        }

        Node Else_StatD()
        {
            Node node = new Node("Else Statements");

            if (TokenStream[InputPointer].token_type == Token_Class.ELSE)
            {
                node.Children.Add(match(Token_Class.ELSE));
                node.Children.Add(Stats_SetD());

                // Token Class : END for IF, ElseIF, and Else (NOT FOUND)
                node.Children.Add(match(Token_Class.END));

            }
            else
                node = null;

            return node;
        }

        Node Rep_Stat()
        {
            Node node = new Node("Repeat Statement");

            node.Children.Add(match(Token_Class.REPEAT));
            node.Children.Add(Stats_SetD());
            node.Children.Add(match(Token_Class.UNTIL));
            node.Children.Add(Cond_Stat());

            return node;
        }


        Node BO()
        {
            Node node = new Node("Boolean Operator");

            if (TokenStream[InputPointer].token_type == Token_Class.AND)
                node.Children.Add(match(Token_Class.AND));
            else if (TokenStream[InputPointer].token_type == Token_Class.OR)
                node.Children.Add(match(Token_Class.OR));

            return node;
        }

        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }
                else
                {
                    Compiler.Syntax_Errors.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Compiler.Syntax_Errors.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
