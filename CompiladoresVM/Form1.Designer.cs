namespace CompiladoresVM
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtInput = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.arquivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.continuarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listViewMemoryProgram = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStripBreakpoint = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.adicionarBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removerBreakpointToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxInput = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtOutput = new System.Windows.Forms.RichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.listViewMemoryData = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.labelI = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelS = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStripBreakpoint.SuspendLayout();
            this.groupBoxInput.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            this.txtInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInput.Location = new System.Drawing.Point(3, 16);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(194, 101);
            this.txtInput.TabIndex = 2;
            this.txtInput.Text = "";
            this.txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtInput_KeyDown);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.arquivoToolStripMenuItem,
            this.executarToolStripMenuItem,
            this.continuarToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(778, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // arquivoToolStripMenuItem
            // 
            this.arquivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.abrirToolStripMenuItem,
            this.sairToolStripMenuItem});
            this.arquivoToolStripMenuItem.Name = "arquivoToolStripMenuItem";
            this.arquivoToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.arquivoToolStripMenuItem.Text = "Arquivo";
            // 
            // abrirToolStripMenuItem
            // 
            this.abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            this.abrirToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.abrirToolStripMenuItem.Text = "Abrir";
            this.abrirToolStripMenuItem.Click += new System.EventHandler(this.abrirToolStripMenuItem_Click);
            // 
            // sairToolStripMenuItem
            // 
            this.sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            this.sairToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.sairToolStripMenuItem.Text = "Sair";
            // 
            // executarToolStripMenuItem
            // 
            this.executarToolStripMenuItem.Name = "executarToolStripMenuItem";
            this.executarToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.executarToolStripMenuItem.Text = "Executar";
            this.executarToolStripMenuItem.Click += new System.EventHandler(this.executarToolStripMenuItem_Click);
            // 
            // continuarToolStripMenuItem
            // 
            this.continuarToolStripMenuItem.Name = "continuarToolStripMenuItem";
            this.continuarToolStripMenuItem.Size = new System.Drawing.Size(91, 20);
            this.continuarToolStripMenuItem.Text = "Passo a passo";
            this.continuarToolStripMenuItem.Click += new System.EventHandler(this.continuarToolStripMenuItem_Click);
            // 
            // listViewMemoryProgram
            // 
            this.listViewMemoryProgram.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.labelHeader,
            this.columnHeader2,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader11,
            this.columnHeader10});
            this.listViewMemoryProgram.ContextMenuStrip = this.contextMenuStripBreakpoint;
            this.listViewMemoryProgram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewMemoryProgram.FullRowSelect = true;
            this.listViewMemoryProgram.Location = new System.Drawing.Point(3, 16);
            this.listViewMemoryProgram.Name = "listViewMemoryProgram";
            this.listViewMemoryProgram.Size = new System.Drawing.Size(560, 207);
            this.listViewMemoryProgram.TabIndex = 5;
            this.listViewMemoryProgram.UseCompatibleStateImageBehavior = false;
            this.listViewMemoryProgram.View = System.Windows.Forms.View.Details;
            this.listViewMemoryProgram.SelectedIndexChanged += new System.EventHandler(this.listViewMemoryProgram_SelectedIndexChanged);
            this.listViewMemoryProgram.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewMemoryProgram_KeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "i";
            this.columnHeader1.Width = 25;
            // 
            // labelHeader
            // 
            this.labelHeader.Text = "Label";
            this.labelHeader.Width = 40;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Instrução";
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "arg1";
            this.columnHeader8.Width = 35;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "arg2";
            this.columnHeader9.Width = 35;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Comentário";
            this.columnHeader11.Width = 150;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "useLabel";
            this.columnHeader10.Width = 40;
            // 
            // contextMenuStripBreakpoint
            // 
            this.contextMenuStripBreakpoint.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.adicionarBreakpointToolStripMenuItem,
            this.removerBreakpointToolStripMenuItem1});
            this.contextMenuStripBreakpoint.Name = "contextMenuStripBreakpoint";
            this.contextMenuStripBreakpoint.Size = new System.Drawing.Size(186, 48);
            // 
            // adicionarBreakpointToolStripMenuItem
            // 
            this.adicionarBreakpointToolStripMenuItem.Name = "adicionarBreakpointToolStripMenuItem";
            this.adicionarBreakpointToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.adicionarBreakpointToolStripMenuItem.Text = "Adicionar breakpoint";
            this.adicionarBreakpointToolStripMenuItem.Click += new System.EventHandler(this.adicionarBreakpointToolStripMenuItem_Click);
            // 
            // removerBreakpointToolStripMenuItem1
            // 
            this.removerBreakpointToolStripMenuItem1.Name = "removerBreakpointToolStripMenuItem1";
            this.removerBreakpointToolStripMenuItem1.Size = new System.Drawing.Size(185, 22);
            this.removerBreakpointToolStripMenuItem1.Text = "Remover breakpoint";
            this.removerBreakpointToolStripMenuItem1.Click += new System.EventHandler(this.removerBreakpointToolStripMenuItem1_Click);
            // 
            // groupBoxInput
            // 
            this.groupBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxInput.Controls.Add(this.txtInput);
            this.groupBoxInput.Location = new System.Drawing.Point(12, 259);
            this.groupBoxInput.Name = "groupBoxInput";
            this.groupBoxInput.Size = new System.Drawing.Size(200, 120);
            this.groupBoxInput.TabIndex = 10;
            this.groupBoxInput.TabStop = false;
            this.groupBoxInput.Text = "Input";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.txtOutput);
            this.groupBox3.Location = new System.Drawing.Point(215, 259);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(360, 120);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Output";
            // 
            // txtOutput
            // 
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutput.Location = new System.Drawing.Point(3, 16);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(354, 101);
            this.txtOutput.TabIndex = 2;
            this.txtOutput.Text = "";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.listViewMemoryProgram);
            this.groupBox4.Location = new System.Drawing.Point(12, 27);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(566, 226);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Memória de programa";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.listViewMemoryData);
            this.groupBox5.Location = new System.Drawing.Point(584, 27);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(182, 352);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Memória de dados";
            // 
            // listViewMemoryData
            // 
            this.listViewMemoryData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7});
            this.listViewMemoryData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewMemoryData.FullRowSelect = true;
            this.listViewMemoryData.Location = new System.Drawing.Point(3, 16);
            this.listViewMemoryData.Name = "listViewMemoryData";
            this.listViewMemoryData.Size = new System.Drawing.Size(176, 333);
            this.listViewMemoryData.TabIndex = 5;
            this.listViewMemoryData.UseCompatibleStateImageBehavior = false;
            this.listViewMemoryData.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Endereço";
            this.columnHeader6.Width = 67;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Conteúdo";
            this.columnHeader7.Width = 96;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelI,
            this.labelS});
            this.statusStrip1.Location = new System.Drawing.Point(0, 390);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(778, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // labelI
            // 
            this.labelI.Name = "labelI";
            this.labelI.Size = new System.Drawing.Size(22, 17);
            this.labelI.Text = "I: 0";
            // 
            // labelS
            // 
            this.labelS.Name = "labelS";
            this.labelS.Size = new System.Drawing.Size(25, 17);
            this.labelS.Text = "S: 0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 412);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBoxInput);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStripBreakpoint.ResumeLayout(false);
            this.groupBoxInput.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox txtInput;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem arquivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem executarToolStripMenuItem;
        private System.Windows.Forms.ListView listViewMemoryProgram;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripBreakpoint;
        private System.Windows.Forms.ToolStripMenuItem adicionarBreakpointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem continuarToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBoxInput;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox txtOutput;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ListView listViewMemoryData;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem removerBreakpointToolStripMenuItem1;
        private System.Windows.Forms.ColumnHeader labelHeader;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel labelI;
        private System.Windows.Forms.ToolStripStatusLabel labelS;
    }
}

