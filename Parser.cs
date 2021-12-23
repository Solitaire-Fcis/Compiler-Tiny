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
            program.Children.Add(Fun_statD());
            program.Children.Add(Main());
            return program;
        }
        Node Fun_statD()
        {
            Node Fun_statDN = new Node("Functions");
            if (Fun_stat() != null)
            {
                Fun_statDN.Children.Add(Fun_stat());
                Fun_statDN.Children.Add(Fun_statD());
            }
            else
                Fun_statDN = null;
            return Fun_statDN;
        }
        Node Main()
        {
            Node main = new Node("Main");
            main.Children.Add(match(Token_Class.DATATYPE_FLOAT));
            main.Children.Add(match(Token_Class.DATATYPE_INT));
            main.Children.Add(match(Token_Class.DATATYPE_STRING));
            main.Children.Add(match(Token_Class.MAIN));
            main.Children.Add(match(Token_Class.LPARENT));
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
            Fun_BodN.Children.Add(match(Token_Class.LBRACES));
            Fun_BodN.Children.Add(Stats_SetD());
            Fun_BodN.Children.Add(match(Token_Class.RETURN));
            Fun_BodN.Children.Add(match(Token_Class.RBRACES));
            return Fun_BodN;
        }

        Node Fun_Decl()
        {
            Node Fun_DeclN = new Node("Function Declaration");
            Fun_DeclN.Children.Add(match(Token_Class.DATATYPE_FLOAT));
            Fun_DeclN.Children.Add(match(Token_Class.DATATYPE_INT));
            Fun_DeclN.Children.Add(match(Token_Class.DATATYPE_STRING));
            Fun_DeclN.Children.Add(match(Token_Class.IDENTIFIER));
            Fun_DeclN.Children.Add(match(Token_Class.LPARENT));
            Fun_DeclN.Children.Add(Para());
            Fun_DeclN.Children.Add(ParaD());
            Fun_DeclN.Children.Add(match(Token_Class.RPARENT));
            return Fun_DeclN;
        }

        Node Stats_SetD()
        {
            Node Stats_SetDN = new Node("Statements");
            if (Stats_Set() != null)
            {
                Stats_SetDN.Children.Add(Stats_Set());
                Stats_SetDN.Children.Add(Stats_SetD());
            }
            else
                Stats_SetDN = null;
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
            else if (TokenStream[InputPointer].token_type == Token_Class.DATATYPE_FLOAT || TokenStream[InputPointer].token_type == Token_Class.DATATYPE_STRING || TokenStream[InputPointer].token_type == Token_Class.DATATYPE_INT)
                Stats_SetN.Children.Add(Decl_Stat());
            return Stats_SetN;
        }

        Node Write()
        {
            Node WriteN = new Node("Write Statement");
            WriteN.Children.Add(match(Token_Class.WRITE));
            if (TokenStream[InputPointer].token_type == Token_Class.ENDL)
            {
                WriteN.Children.Add(match(Token_Class.ENDL));
                WriteN.Children.Add(match(Token_Class.SEMICOLON));
            }
            else
            {
                WriteN.Children.Add(Exp());
                WriteN.Children.Add(match(Token_Class.SEMICOLON));
            }
            return WriteN;
        }
        Node Return()
        {
            Node ReturnN = new Node("Return Statement");
            ReturnN.Children.Add(match(Token_Class.RETURN));
            ReturnN.Children.Add(Exp());
            ReturnN.Children.Add(match(Token_Class.SEMICOLON));
            return ReturnN;
        }
        Node Read()
        {
            Node ReadN = new Node("Read Statement");
            ReadN.Children.Add(match(Token_Class.READ));
            ReadN.Children.Add(match(Token_Class.IDENTIFIER));
            ReadN.Children.Add(match(Token_Class.SEMICOLON));
            return ReadN;
        }
        Node Assign_Stat()
        {
            Node Assign_StatN = new Node("Assignment Statement");
            Assign_StatN.Children.Add(match(Token_Class.IDENTIFIER));
            Assign_StatN.Children.Add(match(Token_Class.ASSIGN));
            Assign_StatN.Children.Add(Exp());
            Assign_StatN.Children.Add(match(Token_Class.SEMICOLON));
            return Assign_StatN;
        }
        Node Exp()
        {

        }
        Node Decl_Stat()
        {
            Node Decl_StatN = new Node("Declarations Statement");
            Decl_StatN.Children.Add(match(Token_Class.DATATYPE_FLOAT));
            Decl_StatN.Children.Add(match(Token_Class.DATATYPE_STRING));
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
                Decl_StatDDN.Children.Add(match(Token_Class.IDENTIFIER));
                if(TokenStream[InputPointer].token_type == Token_Class.COMMA)
                    Decl_StatDDN.Children.Add(Decl_StatDD());
                else if(TokenStream[InputPointer].token_type == Token_Class.COMMA)
                {
                    Decl_StatDDN.Children.Add(match(Token_Class.ASSIGN));
                    Decl_StatDDN.Children.Add(Exp());
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
            if (TokenStream[InputPointer].token_type == Token_Class.IDENTIFIER)
                Decl_StatDN.Children.Add(match(Token_Class.DATATYPE_INT));
            else
                Decl_StatDN = Assign_Stat();
            return Decl_StatDN;
        }
        Node Para()
        {
            Node ParaN = new Node("Parameter");
            ParaN.Children.Add(match(Token_Class.DATATYPE_FLOAT));
            ParaN.Children.Add(match(Token_Class.DATATYPE_INT));
            ParaN.Children.Add(match(Token_Class.DATATYPE_STRING));
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
