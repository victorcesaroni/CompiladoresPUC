using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Compilador;
using System.IO;

namespace IDE
{
    
    public partial class Form1 : Form
    {
        AnalisadorSintatico analisadorSintatico = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxEditor.Text = File.ReadAllText(openFileDialog1.FileName);
                analisadorSintatico = new AnalisadorSintatico(openFileDialog1.FileName);
            }
        }

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            analisadorSintatico.arquivo.Close();
            File.WriteAllText(analisadorSintatico.caminhoArquivo, textBoxEditor.Text);
            analisadorSintatico = new AnalisadorSintatico(analisadorSintatico.caminhoArquivo);
        }

        private void salvarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void compilarToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void compilarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                while (!analisadorSintatico.lexico.FimDeArquivo())
                {
                    var token = analisadorSintatico.lexico.PegaToken();
                    listBox1.Items.Add(token.linha + " " + token.lexema);
                }
            }
            catch (ExceptionErroLexical ex)
            {
                listViewError.Items.Add(new ListViewItem(new string[] { ex.token.linha.ToString(), ex.Message }));
            }
            catch (Exception ex)
            {

            }
        }
    }
}
