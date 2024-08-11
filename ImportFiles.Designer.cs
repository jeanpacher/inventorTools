namespace Bosch_ImportData
{
    partial class ImportFiles
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("$");
            this.label1 = new System.Windows.Forms.Label();
            this.txtZipFileName = new System.Windows.Forms.TextBox();
            this.btnSearchZip = new System.Windows.Forms.Button();
            this.btnExtrair = new System.Windows.Forms.Button();
            this.BtnTeste = new System.Windows.Forms.Button();
            this.btnCloseInventor = new System.Windows.Forms.Button();
            this.subMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.moverParaCCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moverParaATMOLIBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moverParaNormaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TreeBosch = new System.Windows.Forms.TreeView();
            this.ListaLV = new System.Windows.Forms.ListView();
            this.DirectoryCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FileNameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StatusCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabelaItens = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ColunaTipo = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColunaName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaFind = new System.Windows.Forms.DataGridViewButtonColumn();
            this.originalNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subMenu.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabelaItens)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 30);
            this.label1.TabIndex = 5;
            this.label1.Text = "Arquivo a ser importado";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtZipFileName
            // 
            this.txtZipFileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtZipFileName.Location = new System.Drawing.Point(143, 3);
            this.txtZipFileName.Name = "txtZipFileName";
            this.txtZipFileName.Size = new System.Drawing.Size(946, 20);
            this.txtZipFileName.TabIndex = 4;
            this.txtZipFileName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnSearchZip
            // 
            this.btnSearchZip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSearchZip.Location = new System.Drawing.Point(1095, 3);
            this.btnSearchZip.Name = "btnSearchZip";
            this.btnSearchZip.Size = new System.Drawing.Size(34, 24);
            this.btnSearchZip.TabIndex = 3;
            this.btnSearchZip.Text = "...";
            this.btnSearchZip.UseVisualStyleBackColor = true;
            this.btnSearchZip.Click += new System.EventHandler(this.btnSearchZip_Click);
            // 
            // btnExtrair
            // 
            this.btnExtrair.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExtrair.Location = new System.Drawing.Point(1135, 3);
            this.btnExtrair.Name = "btnExtrair";
            this.btnExtrair.Size = new System.Drawing.Size(74, 24);
            this.btnExtrair.TabIndex = 7;
            this.btnExtrair.Text = "Extrair";
            this.btnExtrair.UseVisualStyleBackColor = true;
            this.btnExtrair.Click += new System.EventHandler(this.btnExtrair_Click);
            // 
            // BtnTeste
            // 
            this.BtnTeste.Location = new System.Drawing.Point(1197, 1);
            this.BtnTeste.Name = "BtnTeste";
            this.BtnTeste.Size = new System.Drawing.Size(18, 23);
            this.BtnTeste.TabIndex = 9;
            this.BtnTeste.Text = "Criar Instancia";
            this.BtnTeste.UseVisualStyleBackColor = true;
            this.BtnTeste.Visible = false;
            this.BtnTeste.Click += new System.EventHandler(this.BtnTeste_Click);
            // 
            // btnCloseInventor
            // 
            this.btnCloseInventor.Location = new System.Drawing.Point(1221, 5);
            this.btnCloseInventor.Name = "btnCloseInventor";
            this.btnCloseInventor.Size = new System.Drawing.Size(26, 19);
            this.btnCloseInventor.TabIndex = 10;
            this.btnCloseInventor.Text = "Fechar Inventor";
            this.btnCloseInventor.UseVisualStyleBackColor = true;
            this.btnCloseInventor.Visible = false;
            this.btnCloseInventor.Click += new System.EventHandler(this.btnCloseInventor_Click);
            // 
            // subMenu
            // 
            this.subMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.subMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moverParaCCToolStripMenuItem,
            this.moverParaATMOLIBToolStripMenuItem,
            this.moverParaNormaToolStripMenuItem});
            this.subMenu.Name = "subMenu";
            this.subMenu.Size = new System.Drawing.Size(219, 70);
            this.subMenu.Opening += new System.ComponentModel.CancelEventHandler(this.subMenu_Opening);
            // 
            // moverParaCCToolStripMenuItem
            // 
            this.moverParaCCToolStripMenuItem.Name = "moverParaCCToolStripMenuItem";
            this.moverParaCCToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.moverParaCCToolStripMenuItem.Text = "Mover para Content Center";
            // 
            // moverParaATMOLIBToolStripMenuItem
            // 
            this.moverParaATMOLIBToolStripMenuItem.Name = "moverParaATMOLIBToolStripMenuItem";
            this.moverParaATMOLIBToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.moverParaATMOLIBToolStripMenuItem.Text = "Mover para ATMO LIB";
            // 
            // moverParaNormaToolStripMenuItem
            // 
            this.moverParaNormaToolStripMenuItem.Name = "moverParaNormaToolStripMenuItem";
            this.moverParaNormaToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.moverParaNormaToolStripMenuItem.Text = "Mover para Norma";
            // 
            // TreeBosch
            // 
            this.TreeBosch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TreeBosch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeBosch.Font = new System.Drawing.Font("Microsoft New Tai Lue", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TreeBosch.FullRowSelect = true;
            this.TreeBosch.ItemHeight = 24;
            this.TreeBosch.Location = new System.Drawing.Point(0, 0);
            this.TreeBosch.Name = "TreeBosch";
            treeNode3.Name = "Node0";
            treeNode3.Tag = "root";
            treeNode3.Text = "$";
            this.TreeBosch.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3});
            this.TreeBosch.Size = new System.Drawing.Size(391, 715);
            this.TreeBosch.TabIndex = 14;
            this.TreeBosch.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewNorma_AfterSelect);
            // 
            // ListaLV
            // 
            this.ListaLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.DirectoryCol,
            this.FileNameCol,
            this.StatusCol});
            this.ListaLV.HideSelection = false;
            this.ListaLV.Location = new System.Drawing.Point(0, 0);
            this.ListaLV.Name = "ListaLV";
            this.ListaLV.Size = new System.Drawing.Size(807, 689);
            this.ListaLV.TabIndex = 15;
            this.ListaLV.UseCompatibleStateImageBehavior = false;
            this.ListaLV.View = System.Windows.Forms.View.Details;
            // 
            // DirectoryCol
            // 
            this.DirectoryCol.Text = "Diretório";
            this.DirectoryCol.Width = 200;
            // 
            // FileNameCol
            // 
            this.FileNameCol.Text = "Filename";
            this.FileNameCol.Width = 400;
            // 
            // StatusCol
            // 
            this.StatusCol.Text = "Status";
            this.StatusCol.Width = 100;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(805, 715);
            this.tabControl1.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tabelaItens);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(797, 689);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "TABELA";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabelaItens
            // 
            this.tabelaItens.AllowUserToAddRows = false;
            this.tabelaItens.AllowUserToDeleteRows = false;
            this.tabelaItens.AllowUserToResizeRows = false;
            this.tabelaItens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tabelaItens.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColunaTipo,
            this.ColunaName,
            this.ColunaFind,
            this.originalNameColumn});
            this.tabelaItens.ContextMenuStrip = this.subMenu;
            this.tabelaItens.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabelaItens.Location = new System.Drawing.Point(3, 3);
            this.tabelaItens.Name = "tabelaItens";
            this.tabelaItens.Size = new System.Drawing.Size(791, 683);
            this.tabelaItens.TabIndex = 0;
            this.tabelaItens.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tabelaItens_CellClick);
            this.tabelaItens.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.tabelaItens_CellValueChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ListaLV);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(797, 689);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "LISTA";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtZipFileName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSearchZip, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnExtrair, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(24, 22);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1200, 30);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(24, 74);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.TreeBosch);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1200, 715);
            this.splitContainer1.SplitterDistance = 391;
            this.splitContainer1.TabIndex = 18;
            // 
            // ColunaTipo
            // 
            this.ColunaTipo.HeaderText = "TIPO";
            this.ColunaTipo.Items.AddRange(new object[] {
            "Norma",
            "ATMOLIB_Library",
            "ATMOLIB_ProdutosBosch",
            "NormaAuxiliar",
            "ContentCenter",
            "Desconhecido"});
            this.ColunaTipo.Name = "ColunaTipo";
            this.ColunaTipo.ReadOnly = true;
            this.ColunaTipo.Width = 200;
            // 
            // ColunaName
            // 
            this.ColunaName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColunaName.HeaderText = "NAME";
            this.ColunaName.Name = "ColunaName";
            // 
            // ColunaFind
            // 
            this.ColunaFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ColunaFind.HeaderText = "PROCURAR";
            this.ColunaFind.Name = "ColunaFind";
            this.ColunaFind.Text = "Search";
            // 
            // originalNameColumn
            // 
            this.originalNameColumn.HeaderText = "NomeOriginal";
            this.originalNameColumn.Name = "originalNameColumn";
            this.originalNameColumn.Visible = false;
            // 
            // ImportFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1242, 808);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnCloseInventor);
            this.Controls.Add(this.BtnTeste);
            this.Name = "ImportFiles";
            this.Text = "BOSCH";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImportFiles_FormClosing);
            this.Load += new System.EventHandler(this.ImportFiles_Load);
            this.subMenu.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabelaItens)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtZipFileName;
        private System.Windows.Forms.Button btnSearchZip;
        private System.Windows.Forms.Button btnExtrair;
        private System.Windows.Forms.Button BtnTeste;
        private System.Windows.Forms.Button btnCloseInventor;
        private System.Windows.Forms.ContextMenuStrip subMenu;
        private System.Windows.Forms.ToolStripMenuItem moverParaCCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moverParaATMOLIBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moverParaNormaToolStripMenuItem;
        private System.Windows.Forms.ListView ListaLV;
        private System.Windows.Forms.ColumnHeader DirectoryCol;
        private System.Windows.Forms.ColumnHeader FileNameCol;
        private System.Windows.Forms.ColumnHeader StatusCol;
        public System.Windows.Forms.TreeView TreeBosch;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView tabelaItens;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColunaTipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaName;
        private System.Windows.Forms.DataGridViewButtonColumn ColunaFind;
        private System.Windows.Forms.DataGridViewTextBoxColumn originalNameColumn;
    }
}