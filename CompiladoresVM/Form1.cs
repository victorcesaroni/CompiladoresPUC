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
        VMCore vm = new VMCore(200, 50);

        public Form1()
        {
            InitializeComponent();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                vm.ParseFromFile(openFileDialog1.FileName);

                listViewMemoryProgram.Items.Clear();
                for (int i = 0; i < vm.memory.maxInstructions; i++)
                {
                    var ins = vm.memory.Instructions[i];
                    listViewMemoryProgram.Items.Add(new ListViewItem(new string[] { i.ToString(), ins.label, ins.opcode, ins.arg1, ins.arg2, ins.comment, ins.useLabel.ToString(), }));

                    if (ins.useLabel)
                    {
                        listViewMemoryProgram.Items[i].ForeColor = Color.Blue;
                    }
                    else
                    {
                        listViewMemoryProgram.Items[i].ForeColor = Color.Black;
                    }

                    if (ins.isLabel)
                    {
                        listViewMemoryProgram.Items[i].ForeColor = Color.DarkGray;
                    }
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
            if (listViewMemoryProgram.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in listViewMemoryProgram.SelectedItems)
                {
                    item.BackColor = Color.White;
                }
            }
        }

        private void UpdateInterface()
        {
            listViewMemoryData.Items.Clear();
            for (int i = 0; i < vm.memory.M.Count(); i++)
            {
                var m = vm.memory.M[i];
                listViewMemoryData.Items.Add(new ListViewItem(new string[] { i.ToString(), m.ToString(), }));
            }

            labelI.Text = "I: " + vm.registers.I.ToString();
            labelS.Text = "S: " + vm.registers.S.ToString();

            listViewMemoryProgram.SelectedItems.Clear();
            listViewMemoryProgram.Items[vm.registers.I].Selected = true;
            //listViewMemoryProgram.Items[vm.registers.I].BackColor = Color.Green;
            listViewMemoryProgram.EnsureVisible(vm.registers.I);
            listViewMemoryProgram.Select();
        }

        private void execuçãoDiretaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //vm.Run(false, false);

            while (true)
            {
                //txtOutput.Text += vm.memory.M[vm.registers.I].ToString() + "\r\n";                

                bool continueExec = vm.SingleStep(false);

                if (!continueExec)
                    break;
            }

            UpdateInterface();
        }

        private void passoAPassoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vm.Run(true, true);
            UpdateInterface();
        }

        private void continuarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vm.Run(true, true);
            UpdateInterface();
        }
    }
}
