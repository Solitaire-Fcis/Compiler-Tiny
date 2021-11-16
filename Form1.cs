﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
        // Clear GridView
        private void button_WOC3_Click(object sender, EventArgs e)
        {
            Compiler.Syntax_Errors.Clear();
            Compiler.Tokens_List.Clear();
            dataGridView1.Rows.Clear();
            textBox2.Clear();
        }
        // Source Code
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        // Compile 
        private void button_WOC1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            string SRC = textBox1.Text;
            Compiler.Compile(SRC);
            Tokens_Output();
            Errors_Output();
        }
        // Tokens Filtered
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }
        // Print Tokens Identified in Source Code
        private void Tokens_Output()
        {
            for (int i = 0; i < Compiler.Tokens_List.Count; i++)
            {
                dataGridView1.Rows.Add(Compiler.Tokens_List.ElementAt(i).lex, Compiler.Tokens_List.ElementAt(i).token_type);
            }
        }
        // Print Errors in Source Code
        private void Errors_Output()
        {
            for (int i = 0; i < Compiler.Syntax_Errors.Count; i++)
            {
                textBox2.Text += Compiler.Syntax_Errors[i];
                textBox2.Text += "\r\n";
            }
        }
    }
}
