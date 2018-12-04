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
        Style blueStyle = new TextStyle(Brushes.Blue, Brushes.White, FontStyle.Bold);
        Style negritoStyle = new TextStyle(Brushes.Black, Brushes.White, FontStyle.Bold);
        Style salmonStyle = new TextStyle(Brushes.Salmon, Brushes.White, FontStyle.Regular);
        Style grayStyle = new TextStyle(Brushes.DarkGray, Brushes.White, FontStyle.Bold);
        Style blueNormalStyle = new TextStyle(Brushes.Blue, Brushes.White, FontStyle.Regular);
        Style salmonItallicStyle = new TextStyle(Brushes.Salmon, Brushes.White, FontStyle.Regular | FontStyle.Italic);
        Style blackItallicStyle = new TextStyle(Brushes.Black, Brushes.White, FontStyle.Regular | FontStyle.Italic);
        
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
            Colorir();

            listViewError.Items.Clear();

            Save();

            analisadorSintatico = new AnalisadorSintatico(path);
            //analisadorSintatico.Iniciar();
            try
            {
                analisadorSintatico.Iniciar();

                listViewError.Items.Add(new ListViewItem(new string[] { "0", "0", "Compilacao terminou com sucesso" }));
            }
            
            catch (ExceptionErroLexical ex)
            {
                Range rng = new Range(textBoxEditor, (int)ex.coluna, (int)ex.linha, (int)ex.coluna + 1, (int)ex.linha);
                ClearStyles(rng);
                rng.SetStyle(redStyle);

                listViewError.Items.Add(new ListViewItem(new string[] { ex.linha.ToString(), ex.coluna.ToString(), "Erro lexical: " + ex.ToString() }));
            }
            catch (ExceptionSimboloEsperado ex)
            {
                Range rng = new Range(textBoxEditor, (int)ex.token.coluna - ex.token.lexema.Length - 1, (int)ex.token.linha, (int)ex.token.coluna - 1, (int)ex.token.linha);
                ClearStyles(rng);
                rng.SetStyle(redStyle);

                listViewError.Items.Add(new ListViewItem(new string[] { ex.token.linha.ToString(), ex.token.coluna.ToString(), "Erro sintatico ExceptionSimboloEsperado: " + ex.ToString() }));
            }
            catch (ExceptionSimboloInesperado ex)
            {
                Range rng = new Range(textBoxEditor, (int)ex.token.coluna - ex.token.lexema.Length - 1, (int)ex.token.linha, (int)ex.token.coluna - 1, (int)ex.token.linha);
                ClearStyles(rng);
                rng.SetStyle(redStyle);

                listViewError.Items.Add(new ListViewItem(new string[] { ex.token.linha.ToString(), ex.token.coluna.ToString(), "Erro sintatico ExceptionSimboloInesperado: " + ex.ToString() }));

            }
            catch (ExceptionVariavelNaoDeclarada ex)
            {
                Range rng = new Range(textBoxEditor, (int)ex.token.coluna - ex.token.lexema.Length - 1, (int)ex.token.linha, (int)ex.token.coluna - 1, (int)ex.token.linha);
                ClearStyles(rng);
                rng.SetStyle(redStyle);

                listViewError.Items.Add(new ListViewItem(new string[] { ex.token.linha.ToString(), ex.token.coluna.ToString(), "Erro semantico ExceptionVariavelNaoDeclarada: " + ex.ToString() }));

            }
            catch (ExceptionVariavelDuplicada ex)
            {
                Range rng = new Range(textBoxEditor, (int)ex.token.coluna - ex.token.lexema.Length - 1, (int)ex.token.linha, (int)ex.token.coluna - 1, (int)ex.token.linha);
                ClearStyles(rng);
                rng.SetStyle(redStyle);

                listViewError.Items.Add(new ListViewItem(new string[] { ex.token.linha.ToString(), ex.token.coluna.ToString(), "Erro semantico ExceptionVariavelDuplicada: " + ex.ToString() }));
            }
            catch(ExceptionTipoInvalido ex)
            {
                Range rng = new Range(textBoxEditor, (int)ex.token.coluna - ex.token.lexema.Length - 1, (int)ex.token.linha, (int)ex.token.coluna - 1, (int)ex.token.linha);
                ClearStyles(rng);
                rng.SetStyle(redStyle);

                listViewError.Items.Add(new ListViewItem(new string[] { ex.token.linha.ToString(), ex.token.coluna.ToString(), "Erro semantico ExceptionTipoInvalido: " + ex.ToString() }));
            }
            catch (ExceptionRetornoDeFuncaoInesperado ex)
            {
                Range rng = new Range(textBoxEditor, (int)ex.token.coluna - ex.token.lexema.Length - 1, (int)ex.token.linha, (int)ex.token.coluna - 1, (int)ex.token.linha);
                ClearStyles(rng);
                rng.SetStyle(redStyle);

                listViewError.Items.Add(new ListViewItem(new string[] { ex.token.linha.ToString(), ex.token.coluna.ToString(), "Erro semantico ExceptionRetornoDeFuncaoInesperado: " + ex.ToString() }));
            }
            catch (Exception ex)
            {
                listViewError.Items.Add(new ListViewItem(new string[] { "0", "0", ex.Message }));
            }

            analisadorSintatico.Finalizar();
        }

        void ClearStyles(Range rng)
        {
            rng.ClearStyle(redStyle);
            rng.ClearStyle(blueStyle);
            rng.ClearStyle(negritoStyle);
            rng.ClearStyle(salmonStyle);
            rng.ClearStyle(grayStyle);
            rng.ClearStyle(blueNormalStyle);
            rng.ClearStyle(salmonItallicStyle);
            rng.ClearStyle(blackItallicStyle);
        }

        void Colorir()
        {
            if (path == null)
                return;

            ClearStyles(new Range(textBoxEditor, 0, 0, textBoxEditor.Lines[textBoxEditor.Lines.Count - 1].Length, textBoxEditor.Lines.Count - 1));

            string pathTmp = path + ".tmp";

            if (!File.Exists(pathTmp))
                File.Create(pathTmp);

            File.WriteAllText(pathTmp, textBoxEditor.Text);

            using (StreamReader arquivo = new StreamReader(new FileStream(pathTmp, FileMode.Open)))
            {                
                AnalisadorLexico lexico = new AnalisadorLexico(arquivo);

                try
                {
                    //listBox1.Items.Clear();

                    while (!lexico.FimDeArquivo())
                    {
                        var token = lexico.PegaToken();
                        //listBox1.Items.Add(token.linha + " " + token.lexema);

                        Range rng = new Range(textBoxEditor, (int)token.coluna - token.lexema.Length - 1, (int)token.linha, (int)token.coluna - 1, (int)token.linha);

                        if (token.simbolo == Simbolo.S_PROCEDIMENTO ||
                            token.simbolo == Simbolo.S_PROGRAMA ||
                            token.simbolo == Simbolo.S_FACA ||
                            token.simbolo == Simbolo.S_SE ||
                            token.simbolo == Simbolo.S_SENAO ||
                            token.simbolo == Simbolo.S_ENTAO ||
                            token.simbolo == Simbolo.S_VAR ||
                            token.simbolo == Simbolo.S_ENQUANTO ||
                            token.simbolo == Simbolo.S_FUNCAO)
                        {
                            rng.SetStyle(blueStyle);
                        }

                        if (token.simbolo == Simbolo.S_INICIO ||
                            token.simbolo == Simbolo.S_FIM)
                        {
                            rng.SetStyle(negritoStyle);
                        }

                        if (token.simbolo == Simbolo.S_NUMERO)
                        {
                            rng.SetStyle(salmonStyle);
                        }
                                         
                        if (token.simbolo == Simbolo.S_VERDADEIRO ||
                           token.simbolo == Simbolo.S_FALSO)
                        {
                            rng.SetStyle(salmonStyle);
                        }

                        if (token.simbolo == Simbolo.S_BOOLEANO ||
                            token.simbolo == Simbolo.S_INTEIRO ||
                            token.simbolo == Simbolo.S_E ||
                            token.simbolo == Simbolo.S_OU ||
                            token.simbolo == Simbolo.S_NAO ||
                            token.simbolo == Simbolo.S_DIV)
                        {
                            rng.SetStyle(blackItallicStyle);
                        }
                    }
                }
                catch { }

                arquivo.Close();
            }

            using (StreamReader arquivo = new StreamReader(new FileStream(pathTmp, FileMode.Open)))
            {                
                AnalisadorLexico lexico = new AnalisadorLexico(arquivo);

                while (!lexico.FimDeArquivo())
                {
                    if (lexico.c == '{')
                    {
                        int startL = (int)lexico.linha;
                        int startC = (int)lexico.coluna - 1;
                        while (lexico.c != '}' && !lexico.FimDeArquivo())
                        {
                            lexico.c = lexico.Ler();
                        }
                        int endL = (int)lexico.linha;
                        int endC = (int)lexico.coluna;

                        Range rng = new Range(textBoxEditor, startC, startL, endC, endL);
                        ClearStyles(rng);
                        rng.SetStyle(grayStyle);
                    }

                    lexico.c = lexico.Ler();
                }

                arquivo.Close();
            }
        }

        private void textBoxEditor_TextChangedDelayed(object sender, TextChangedEventArgs e)
        {
            Colorir();
        }

        private void textBoxEditor_Load(object sender, EventArgs e)
        {

        }
    }
}
