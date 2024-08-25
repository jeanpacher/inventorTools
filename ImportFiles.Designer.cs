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
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("$");
            this.label1 = new System.Windows.Forms.Label();
            this.txtZipFileName = new System.Windows.Forms.TextBox();
            this.btnSearchZip = new System.Windows.Forms.Button();
            this.btnExtrair = new System.Windows.Forms.Button();
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
            this.tabTabela = new System.Windows.Forms.TabPage();
            this.tabelaItens = new System.Windows.Forms.DataGridView();
            this.ColunaTipo = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.VaultExistColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColunaName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaFind = new System.Windows.Forms.DataGridViewButtonColumn();
            this.originalNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabDesconhecidos = new System.Windows.Forms.TabPage();
            this.TabelaDesconhecidos = new System.Windows.Forms.DataGridView();
            this.ColunaCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnMover = new System.Windows.Forms.Button();
            this.txtFolderToMove = new System.Windows.Forms.ComboBox();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.subMenu.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabTabela.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabelaItens)).BeginInit();
            this.tabDesconhecidos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TabelaDesconhecidos)).BeginInit();
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
            // subMenu
            // 
            this.subMenu.Enabled = false;
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
            this.moverParaCCToolStripMenuItem.Enabled = false;
            this.moverParaCCToolStripMenuItem.Name = "moverParaCCToolStripMenuItem";
            this.moverParaCCToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.moverParaCCToolStripMenuItem.Text = "Mover para Content Center";
            // 
            // moverParaATMOLIBToolStripMenuItem
            // 
            this.moverParaATMOLIBToolStripMenuItem.Enabled = false;
            this.moverParaATMOLIBToolStripMenuItem.Name = "moverParaATMOLIBToolStripMenuItem";
            this.moverParaATMOLIBToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.moverParaATMOLIBToolStripMenuItem.Text = "Mover para ATMO LIB";
            // 
            // moverParaNormaToolStripMenuItem
            // 
            this.moverParaNormaToolStripMenuItem.Enabled = false;
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
            treeNode2.Name = "Node0";
            treeNode2.Tag = "root";
            treeNode2.Text = "$";
            this.TreeBosch.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.TreeBosch.Size = new System.Drawing.Size(393, 715);
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
            this.tabControl1.Controls.Add(this.tabTabela);
            this.tabControl1.Controls.Add(this.tabDesconhecidos);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(814, 715);
            this.tabControl1.TabIndex = 16;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabTabela
            // 
            this.tabTabela.Controls.Add(this.tabelaItens);
            this.tabTabela.Location = new System.Drawing.Point(4, 22);
            this.tabTabela.Name = "tabTabela";
            this.tabTabela.Padding = new System.Windows.Forms.Padding(3);
            this.tabTabela.Size = new System.Drawing.Size(806, 689);
            this.tabTabela.TabIndex = 0;
            this.tabTabela.Text = "TABELA";
            this.tabTabela.UseVisualStyleBackColor = true;
            // 
            // tabelaItens
            // 
            this.tabelaItens.AllowUserToAddRows = false;
            this.tabelaItens.AllowUserToDeleteRows = false;
            this.tabelaItens.AllowUserToResizeRows = false;
            this.tabelaItens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tabelaItens.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColunaTipo,
            this.VaultExistColumn,
            this.ColunaName,
            this.ColunaFind,
            this.originalNameColumn});
            this.tabelaItens.ContextMenuStrip = this.subMenu;
            this.tabelaItens.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabelaItens.Location = new System.Drawing.Point(3, 3);
            this.tabelaItens.Name = "tabelaItens";
            this.tabelaItens.RowHeadersWidth = 51;
            this.tabelaItens.Size = new System.Drawing.Size(800, 683);
            this.tabelaItens.StandardTab = true;
            this.tabelaItens.TabIndex = 0;
            this.tabelaItens.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tabelaItens_CellClick);
            this.tabelaItens.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.tabelaItens_CellValueChanged);
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
            this.ColunaTipo.MinimumWidth = 6;
            this.ColunaTipo.Name = "ColunaTipo";
            this.ColunaTipo.ReadOnly = true;
            this.ColunaTipo.Width = 150;
            // 
            // VaultExistColumn
            // 
            this.VaultExistColumn.HeaderText = "Vault Exist";
            this.VaultExistColumn.MinimumWidth = 6;
            this.VaultExistColumn.Name = "VaultExistColumn";
            this.VaultExistColumn.ReadOnly = true;
            this.VaultExistColumn.Width = 80;
            // 
            // ColunaName
            // 
            this.ColunaName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColunaName.HeaderText = "NAME";
            this.ColunaName.MinimumWidth = 6;
            this.ColunaName.Name = "ColunaName";
            // 
            // ColunaFind
            // 
            this.ColunaFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ColunaFind.HeaderText = "PROCURAR";
            this.ColunaFind.MinimumWidth = 6;
            this.ColunaFind.Name = "ColunaFind";
            this.ColunaFind.Text = "Search";
            this.ColunaFind.Width = 125;
            // 
            // originalNameColumn
            // 
            this.originalNameColumn.HeaderText = "NomeOriginal";
            this.originalNameColumn.MinimumWidth = 6;
            this.originalNameColumn.Name = "originalNameColumn";
            this.originalNameColumn.Visible = false;
            this.originalNameColumn.Width = 125;
            // 
            // tabDesconhecidos
            // 
            this.tabDesconhecidos.Controls.Add(this.TabelaDesconhecidos);
            this.tabDesconhecidos.Controls.Add(this.ListaLV);
            this.tabDesconhecidos.Location = new System.Drawing.Point(4, 22);
            this.tabDesconhecidos.Name = "tabDesconhecidos";
            this.tabDesconhecidos.Padding = new System.Windows.Forms.Padding(3);
            this.tabDesconhecidos.Size = new System.Drawing.Size(806, 689);
            this.tabDesconhecidos.TabIndex = 1;
            this.tabDesconhecidos.Text = "DESCONHECIDOS";
            this.tabDesconhecidos.UseVisualStyleBackColor = true;
            // 
            // TabelaDesconhecidos
            // 
            this.TabelaDesconhecidos.AllowUserToAddRows = false;
            this.TabelaDesconhecidos.AllowUserToDeleteRows = false;
            this.TabelaDesconhecidos.AllowUserToResizeRows = false;
            this.TabelaDesconhecidos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TabelaDesconhecidos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColunaCheck,
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.TabelaDesconhecidos.ContextMenuStrip = this.subMenu;
            this.TabelaDesconhecidos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabelaDesconhecidos.Location = new System.Drawing.Point(3, 3);
            this.TabelaDesconhecidos.Name = "TabelaDesconhecidos";
            this.TabelaDesconhecidos.RowHeadersWidth = 51;
            this.TabelaDesconhecidos.Size = new System.Drawing.Size(800, 683);
            this.TabelaDesconhecidos.StandardTab = true;
            this.TabelaDesconhecidos.TabIndex = 16;
            // 
            // ColunaCheck
            // 
            this.ColunaCheck.HeaderText = "ITEM";
            this.ColunaCheck.MinimumWidth = 6;
            this.ColunaCheck.Name = "ColunaCheck";
            this.ColunaCheck.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColunaCheck.Width = 80;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "Vault Exist";
            this.dataGridViewCheckBoxColumn1.MinimumWidth = 6;
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ReadOnly = true;
            this.dataGridViewCheckBoxColumn1.Width = 80;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "NAME";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "NomeOriginal";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Visible = false;
            this.dataGridViewTextBoxColumn2.Width = 125;
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
            this.tableLayoutPanel1.Location = new System.Drawing.Point(24, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1211, 30);
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
            this.splitContainer1.Size = new System.Drawing.Size(1211, 715);
            this.splitContainer1.SplitterDistance = 393;
            this.splitContainer1.TabIndex = 18;
            // 
            // btnMover
            // 
            this.btnMover.Location = new System.Drawing.Point(1093, 48);
            this.btnMover.Name = "btnMover";
            this.btnMover.Size = new System.Drawing.Size(142, 23);
            this.btnMover.TabIndex = 17;
            this.btnMover.Text = "Mover Selecionados";
            this.btnMover.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMover.UseVisualStyleBackColor = true;
            this.btnMover.Visible = false;
            // 
            // txtFolderToMove
            // 
            this.txtFolderToMove.FormattingEnabled = true;
            this.txtFolderToMove.Location = new System.Drawing.Point(800, 48);
            this.txtFolderToMove.Name = "txtFolderToMove";
            this.txtFolderToMove.Size = new System.Drawing.Size(287, 21);
            this.txtFolderToMove.TabIndex = 19;
            this.txtFolderToMove.Visible = false;
            // 
            // cbLocation
            // 
            this.cbLocation.FormattingEnabled = true;
            this.cbLocation.Items.AddRange(new object[] {
            "$/ATMOLIB/Library/Catalog",
            "$/ATMOLIB/Produtos Bosch",
            "$/Sites/CtP_TEF/project",
            "$/ContentCenter/en-US",
            "$/ContentCenter/pt-BR",
            "$/ContentCenter/pt-PT"});
            this.cbLocation.Location = new System.Drawing.Point(543, 48);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(251, 21);
            this.cbLocation.TabIndex = 20;
            this.cbLocation.Visible = false;
            this.cbLocation.SelectedIndexChanged += new System.EventHandler(this.cbLocation_SelectedIndexChanged);
            // 
            // ImportFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1253, 808);
            this.Controls.Add(this.cbLocation);
            this.Controls.Add(this.txtFolderToMove);
            this.Controls.Add(this.btnMover);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ImportFiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BOSCH - NÃO CONECTADO";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImportFiles_FormClosing);
            this.Load += new System.EventHandler(this.ImportFiles_Load);
            this.subMenu.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabTabela.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabelaItens)).EndInit();
            this.tabDesconhecidos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TabelaDesconhecidos)).EndInit();
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
        private System.Windows.Forms.TabPage tabTabela;
        private System.Windows.Forms.TabPage tabDesconhecidos;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColunaTipo;
        private System.Windows.Forms.DataGridViewCheckBoxColumn VaultExistColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaName;
        private System.Windows.Forms.DataGridViewButtonColumn ColunaFind;
        private System.Windows.Forms.DataGridViewTextBoxColumn originalNameColumn;
        public System.Windows.Forms.DataGridView tabelaItens;
        public System.Windows.Forms.DataGridView TabelaDesconhecidos;
        private System.Windows.Forms.Button btnMover;
        private System.Windows.Forms.ComboBox txtFolderToMove;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColunaCheck;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.ComboBox cbLocation;
    }
}