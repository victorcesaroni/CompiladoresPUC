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
using FastColoredTextBoxNS;

namespace IDE
{
    
    public partial class Form1 : Form
    {
        Style redStyle = new TextStyle(Brushes.Black, Brushes.LightSalmon, FontStyle.Regular);

        AnalisadorSintatico analisadorSintatico = null;
        string path = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog1.FileName;
                textBoxEditor.Text = File.ReadAllText(path);
            }
        }

        private void Save()
        {
            if (path == null)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    path = saveFileDialog1.FileName;
                }
            }

            if (path != null)
                File.WriteAllText(path, textBoxEditor.Text);
        }

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
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
            {
                Range rng = new Range(textBoxEditor, 0, 0, textBoxEditor.Lines[textBoxEditor.Lines.Count - 1].Length, textBoxEditor.Lines.Count - 1);
                rng.ClearStyle(redStyle);
            }

            listViewError.Items.Clear();
            listBox1.Items.Clear();

            Save();

            analisadorSintatico = new AnalisadorSintatico(path);

            try
            {
                /*while (!analisadorSintatico.lexico.FimDeArquivo())
                {
                    var token = analisadorSintatico.lexico.PegaToken();
                    listBox1.Items.Add(token.linha + " " + token.lexema);
                }*/

                analisadorSintatico.Iniciar();

                listViewError.Items.Add(new ListViewItem(new string[] { "0", "0", "Compilacao terminou com sucesso" }));
            }
            catch (ExceptionErroLexical ex)
            {
                Range rng = new Range(textBoxEditor, (int)ex.coluna, (int)ex.linha, (int)ex.coluna + 1, (int)ex.linha);
                rng.SetStyle(redStyle);

                listViewError.Items.Add(new ListViewItem(new string[] { ex.linha.ToString(), ex.coluna.ToString(), "Erro lexical: " + ex.ToString() }));
            }
            catch (ExceptionSimboloEsperado ex)
            {
                Range rng = new Range(textBoxEditor, (int)ex.token.coluna - ex.token.lexema.Length, (int)ex.token.linha, (int)ex.token.coluna, (int)ex.token.linha);
                rng.SetStyle(redStyle);

                listViewError.Items.Add(new ListViewItem(new string[] { ex.token.linha.ToString(), ex.token.coluna.ToString(), "Erro sintatico ExceptionSimboloEsperado: " + ex.ToString() }));
            }
            catch (ExceptionSimboloInesperado ex)
            {
                Range rng = new Range(textBoxEditor, (int)ex.token.coluna - ex.token.lexema.Length, (int)ex.token.linha , (int)ex.token.coluna, (int)ex.token.linha);
                rng.SetStyle(redStyle);

                listViewError.Items.Add(new ListViewItem(new string[] { ex.token.linha.ToString(), ex.token.coluna.ToString(), "Erro sintatico ExceptionSimboloInesperado: " + ex.ToString() }));

            }
            catch (Exception ex)
            {
                listViewError.Items.Add(new ListViewItem(new string[] { "0", "0", ex.Message }));
            }

            analisadorSintatico.arquivo.Close();
        }
    }
}
