using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompiladoresVM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] lines = File.ReadAllLines(openFileDialog1.FileName);
                int i = 0;
                listViewMemoryProgram.Items.Clear();
                foreach (var l in lines)
                {
                    listViewMemoryProgram.Items.Add(new ListViewItem(new string[] { i.ToString(), l }));
                    i++;
                }
            }
        }

        private void adicionarBreakpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewMemoryProgram.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listViewMemoryProgram.SelectedItems)
                {
                    item.BackColor = Color.Red;
                }
            }
        }

        private void removerBreakpointToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
    }
}
