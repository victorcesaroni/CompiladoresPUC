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
        VMCore vm;

        public Form1()
        {
            InitializeComponent();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                vm = new VMCore(2000, 200);

                vm.ParseFromFile(openFileDialog1.FileName);

                listViewMemoryProgram.Items.Clear();
                for (int i = 0; i < vm.instructionCount; i++)
                {
                    var ins = vm.instructions[i];
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
                listViewMemoryProgram.Items.Add(new ListViewItem(new string[] { "FIM", "FIM", "FIM", "FIM", "FIM", "FIM", "FIM", }));

                ProccessIO();
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
            for (int i = 0; i < vm.M.Count(); i++)
            {
                var m = vm.M[i];
                listViewMemoryData.Items.Add(new ListViewItem(new string[] { i.ToString(), m.ToString(), }));
            }
            listViewMemoryData.Items.Add(new ListViewItem(new string[] { "FIM", "FIM", }));

            labelI.Text = "I: " + vm.I.ToString();
            labelS.Text = "S: " + vm.S.ToString();

            listViewMemoryProgram.SelectedItems.Clear();
            listViewMemoryProgram.Items[vm.I].Selected = true;
            //listViewMemoryProgram.Items[vm.I].BackColor = Color.Green;
            listViewMemoryProgram.EnsureVisible(vm.I);
            listViewMemoryProgram.Select();

            if (vm.S >= 0)
            {
                //listViewMemoryData.SelectedItems.Clear();
                //listViewMemoryData.Items[vm.S].Selected = true;
                listViewMemoryData.Items[vm.S].BackColor = Color.Green;
                //listViewMemoryData.EnsureVisible(vm.S);
                //listViewMemoryData.Select();
            }
        }

        private void ProccessIO()
        {
            txtInput.Enabled = vm.io.waitingInput;
            txtOutput.Enabled = vm.io.waitingOutput;

            int parseResult = 0;

            if (vm.io.waitingOutput)
            {
                int ioReadData = vm.IORead();

                txtOutput.Text += ioReadData.ToString() + "\r\n";
            }

            if (vm.io.waitingInput && int.TryParse(txtInput.Text, out parseResult))
            {
                int ioWriteData = int.Parse(txtInput.Text);
                vm.IOWrite(ioWriteData);

                txtInput.Text = "";
            }

            txtInput.Enabled = vm.io.waitingInput;
            txtOutput.Enabled = vm.io.waitingOutput;
        }

        private void continuarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProccessIO();
            vm.Run(true, true);
            ProccessIO();
            UpdateInterface();
        }

        private void executarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //vm.Run(false, false);

            while (true)
            {
                //txtOutput.Text += vm.memory.M[vm.registers.I].ToString() + "\r\n";                

                ProccessIO();
                bool continueExec = vm.SingleStep(false);
                ProccessIO();

                if (!continueExec)
                    break;
            }

            UpdateInterface();
        }
    }
}
