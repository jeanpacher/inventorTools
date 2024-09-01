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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("$");
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportFiles));
            this.label1 = new System.Windows.Forms.Label();
            this.txtZipFileName = new System.Windows.Forms.TextBox();
            this.btnSearchZip = new System.Windows.Forms.Button();
            this.btnExtrair = new System.Windows.Forms.Button();
            this.subMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CheckAll = new System.Windows.Forms.ToolStripMenuItem();
            this.UncheckAll = new System.Windows.Forms.ToolStripMenuItem();
            this.TreeBosch = new System.Windows.Forms.TreeView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabTabela = new System.Windows.Forms.TabPage();
            this.tabelaItens = new System.Windows.Forms.DataGridView();
            this.ColunaTipo = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.VaultExistColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColunaExtensaoArquivo = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColunaPasta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaRenomear = new System.Windows.Forms.DataGridViewButtonColumn();
            this.originalNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabDesconhecidos = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TabelaDesconhecidos = new System.Windows.Forms.DataGridView();
            this.ColunaCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnMover = new System.Windows.Forms.Button();
            this.txtFolderToMove = new System.Windows.Forms.ComboBox();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.tabATMOLIB = new System.Windows.Forms.TabPage();
            this.tabelaATMOLIB = new System.Windows.Forms.DataGridView();
            this.ColunaIsVault = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colunaFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colunaArquivo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colunaRBGBDETAILS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaRBGBPRODUCERNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaRBGBPRODUCERORDERNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.LIstaIcones = new System.Windows.Forms.ImageList(this.components);
            this.normaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.subMenu.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabTabela.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabelaItens)).BeginInit();
            this.tabDesconhecidos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TabelaDesconhecidos)).BeginInit();
            this.tabATMOLIB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabelaATMOLIB)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.normaBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 30);
            this.label1.TabIndex = 5;
            this.label1.Text = "Arquivo a ser importado";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtZipFileName
            // 
            this.txtZipFileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtZipFileName.Location = new System.Drawing.Point(224, 3);
            this.txtZipFileName.Name = "txtZipFileName";
            this.txtZipFileName.Size = new System.Drawing.Size(1203, 20);
            this.txtZipFileName.TabIndex = 4;
            this.txtZipFileName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnSearchZip
            // 
            this.btnSearchZip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSearchZip.Location = new System.Drawing.Point(1433, 3);
            this.btnSearchZip.Name = "btnSearchZip";
            this.btnSearchZip.Size = new System.Drawing.Size(35, 24);
            this.btnSearchZip.TabIndex = 3;
            this.btnSearchZip.Text = "...";
            this.btnSearchZip.UseVisualStyleBackColor = true;
            this.btnSearchZip.Click += new System.EventHandler(this.btnSearchZip_Click);
            // 
            // btnExtrair
            // 
            this.btnExtrair.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExtrair.Location = new System.Drawing.Point(1474, 3);
            this.btnExtrair.Name = "btnExtrair";
            this.btnExtrair.Size = new System.Drawing.Size(112, 24);
            this.btnExtrair.TabIndex = 7;
            this.btnExtrair.Text = "Extrair";
            this.btnExtrair.UseVisualStyleBackColor = true;
            this.btnExtrair.Click += new System.EventHandler(this.btnExtrair_Click);
            // 
            // subMenu
            // 
            this.subMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.subMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CheckAll,
            this.UncheckAll});
            this.subMenu.Name = "subMenu";
            this.subMenu.Size = new System.Drawing.Size(138, 48);
            // 
            // CheckAll
            // 
            this.CheckAll.Name = "CheckAll";
            this.CheckAll.Size = new System.Drawing.Size(137, 22);
            this.CheckAll.Text = "Check All";
            this.CheckAll.Click += new System.EventHandler(this.CheckAll_Click);
            // 
            // UncheckAll
            // 
            this.UncheckAll.Name = "UncheckAll";
            this.UncheckAll.Size = new System.Drawing.Size(137, 22);
            this.UncheckAll.Text = "Uncheck All";
            this.UncheckAll.Click += new System.EventHandler(this.UncheckAll_Click);
            // 
            // TreeBosch
            // 
            this.TreeBosch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TreeBosch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeBosch.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TreeBosch.FullRowSelect = true;
            this.TreeBosch.ItemHeight = 20;
            this.TreeBosch.Location = new System.Drawing.Point(0, 0);
            this.TreeBosch.Name = "TreeBosch";
            treeNode1.Name = "Node0";
            treeNode1.Tag = "root";
            treeNode1.Text = "$";
            this.TreeBosch.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.TreeBosch.Size = new System.Drawing.Size(314, 725);
            this.TreeBosch.TabIndex = 14;
            this.TreeBosch.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewNorma_AfterSelect);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTabela);
            this.tabControl1.Controls.Add(this.tabDesconhecidos);
            this.tabControl1.Controls.Add(this.tabATMOLIB);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1384, 725);
            this.tabControl1.TabIndex = 16;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabTabela
            // 
            this.tabTabela.Controls.Add(this.tabelaItens);
            this.tabTabela.Location = new System.Drawing.Point(4, 22);
            this.tabTabela.Name = "tabTabela";
            this.tabTabela.Padding = new System.Windows.Forms.Padding(3);
            this.tabTabela.Size = new System.Drawing.Size(1376, 699);
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
            this.ColunaExtensaoArquivo,
            this.ColunaPasta,
            this.ColunaName,
            this.ColunaRenomear,
            this.originalNameColumn});
            this.tabelaItens.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabelaItens.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.tabelaItens.Location = new System.Drawing.Point(3, 3);
            this.tabelaItens.Name = "tabelaItens";
            this.tabelaItens.RowHeadersWidth = 51;
            this.tabelaItens.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tabelaItens.ShowEditingIcon = false;
            this.tabelaItens.Size = new System.Drawing.Size(1370, 693);
            this.tabelaItens.StandardTab = true;
            this.tabelaItens.TabIndex = 0;
            this.tabelaItens.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tabelaItens_CellClick);
            this.tabelaItens.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tabelaItens_CellContentClick);
            this.tabelaItens.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.tabelaItens_CellValueChanged);
            // 
            // ColunaTipo
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColunaTipo.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColunaTipo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.ColunaTipo.HeaderText = "CATEGORIA";
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
            this.ColunaTipo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColunaTipo.Width = 150;
            // 
            // VaultExistColumn
            // 
            this.VaultExistColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VaultExistColumn.HeaderText = "VAULT";
            this.VaultExistColumn.MinimumWidth = 6;
            this.VaultExistColumn.Name = "VaultExistColumn";
            this.VaultExistColumn.ReadOnly = true;
            this.VaultExistColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.VaultExistColumn.Width = 60;
            // 
            // ColunaExtensaoArquivo
            // 
            this.ColunaExtensaoArquivo.HeaderText = "TIPO";
            this.ColunaExtensaoArquivo.Name = "ColunaExtensaoArquivo";
            this.ColunaExtensaoArquivo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColunaExtensaoArquivo.Width = 60;
            // 
            // ColunaPasta
            // 
            this.ColunaPasta.HeaderText = "PASTA";
            this.ColunaPasta.Name = "ColunaPasta";
            this.ColunaPasta.Width = 200;
            // 
            // ColunaName
            // 
            this.ColunaName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColunaName.HeaderText = "NAME";
            this.ColunaName.MinimumWidth = 6;
            this.ColunaName.Name = "ColunaName";
            // 
            // ColunaRenomear
            // 
            this.ColunaRenomear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ColunaRenomear.HeaderText = "AÇÃO";
            this.ColunaRenomear.MinimumWidth = 6;
            this.ColunaRenomear.Name = "ColunaRenomear";
            this.ColunaRenomear.Text = "Renomear";
            this.ColunaRenomear.Width = 125;
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
            this.tabDesconhecidos.Controls.Add(this.label3);
            this.tabDesconhecidos.Controls.Add(this.label2);
            this.tabDesconhecidos.Controls.Add(this.TabelaDesconhecidos);
            this.tabDesconhecidos.Controls.Add(this.btnMover);
            this.tabDesconhecidos.Controls.Add(this.txtFolderToMove);
            this.tabDesconhecidos.Controls.Add(this.cbLocation);
            this.tabDesconhecidos.Location = new System.Drawing.Point(4, 22);
            this.tabDesconhecidos.Name = "tabDesconhecidos";
            this.tabDesconhecidos.Padding = new System.Windows.Forms.Padding(3);
            this.tabDesconhecidos.Size = new System.Drawing.Size(1376, 699);
            this.tabDesconhecidos.TabIndex = 1;
            this.tabDesconhecidos.Text = "DESCONHECIDOS";
            this.tabDesconhecidos.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(261, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "PASTA";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "CATEGORIA";
            // 
            // TabelaDesconhecidos
            // 
            this.TabelaDesconhecidos.AllowUserToAddRows = false;
            this.TabelaDesconhecidos.AllowUserToDeleteRows = false;
            this.TabelaDesconhecidos.AllowUserToResizeRows = false;
            this.TabelaDesconhecidos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TabelaDesconhecidos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColunaCheck,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.TabelaDesconhecidos.ContextMenuStrip = this.subMenu;
            this.TabelaDesconhecidos.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TabelaDesconhecidos.Location = new System.Drawing.Point(3, 71);
            this.TabelaDesconhecidos.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.TabelaDesconhecidos.Name = "TabelaDesconhecidos";
            this.TabelaDesconhecidos.RowHeadersWidth = 51;
            this.TabelaDesconhecidos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.TabelaDesconhecidos.ShowEditingIcon = false;
            this.TabelaDesconhecidos.Size = new System.Drawing.Size(1370, 625);
            this.TabelaDesconhecidos.StandardTab = true;
            this.TabelaDesconhecidos.TabIndex = 16;
            // 
            // ColunaCheck
            // 
            this.ColunaCheck.HeaderText = "ITEM";
            this.ColunaCheck.MinimumWidth = 6;
            this.ColunaCheck.Name = "ColunaCheck";
            this.ColunaCheck.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColunaCheck.Width = 60;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "NAME";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 350;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "NOME ORIGINAL";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // btnMover
            // 
            this.btnMover.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnMover.Location = new System.Drawing.Point(566, 13);
            this.btnMover.Name = "btnMover";
            this.btnMover.Size = new System.Drawing.Size(135, 42);
            this.btnMover.TabIndex = 17;
            this.btnMover.Text = "MOVER SELECIONADOS";
            this.btnMover.UseVisualStyleBackColor = true;
            this.btnMover.Click += new System.EventHandler(this.btnMover_Click);
            // 
            // txtFolderToMove
            // 
            this.txtFolderToMove.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtFolderToMove.FormattingEnabled = true;
            this.txtFolderToMove.Location = new System.Drawing.Point(261, 33);
            this.txtFolderToMove.Name = "txtFolderToMove";
            this.txtFolderToMove.Size = new System.Drawing.Size(287, 21);
            this.txtFolderToMove.TabIndex = 19;
            // 
            // cbLocation
            // 
            this.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocation.FormattingEnabled = true;
            this.cbLocation.Items.AddRange(new object[] {
            "$\\ATMOLIB\\Library\\Catalog",
            "$\\ATMOLIB\\Produtos Bosch",
            "$\\Sites\\CtP_TEF\\project",
            "$\\ContentCenter\\en-US",
            "$\\ContentCenter\\pt-BR",
            "$\\ContentCenter\\pt-PT"});
            this.cbLocation.Location = new System.Drawing.Point(6, 33);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(211, 21);
            this.cbLocation.TabIndex = 20;
            this.cbLocation.SelectedIndexChanged += new System.EventHandler(this.cbLocation_SelectedIndexChanged);
            // 
            // tabATMOLIB
            // 
            this.tabATMOLIB.Controls.Add(this.tabelaATMOLIB);
            this.tabATMOLIB.Location = new System.Drawing.Point(4, 22);
            this.tabATMOLIB.Name = "tabATMOLIB";
            this.tabATMOLIB.Padding = new System.Windows.Forms.Padding(3);
            this.tabATMOLIB.Size = new System.Drawing.Size(1376, 699);
            this.tabATMOLIB.TabIndex = 2;
            this.tabATMOLIB.Text = "ATMOLIB";
            this.tabATMOLIB.UseVisualStyleBackColor = true;
            // 
            // tabelaATMOLIB
            // 
            this.tabelaATMOLIB.AllowUserToAddRows = false;
            this.tabelaATMOLIB.AllowUserToDeleteRows = false;
            this.tabelaATMOLIB.AllowUserToResizeRows = false;
            this.tabelaATMOLIB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tabelaATMOLIB.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColunaIsVault,
            this.colunaFolder,
            this.colunaArquivo,
            this.colunaRBGBDETAILS,
            this.ColunaRBGBPRODUCERNAME,
            this.ColunaRBGBPRODUCERORDERNO});
            this.tabelaATMOLIB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabelaATMOLIB.Location = new System.Drawing.Point(3, 3);
            this.tabelaATMOLIB.Name = "tabelaATMOLIB";
            this.tabelaATMOLIB.RowHeadersWidth = 51;
            this.tabelaATMOLIB.Size = new System.Drawing.Size(1370, 693);
            this.tabelaATMOLIB.StandardTab = true;
            this.tabelaATMOLIB.TabIndex = 1;
            // 
            // ColunaIsVault
            // 
            this.ColunaIsVault.HeaderText = "VAULT";
            this.ColunaIsVault.Name = "ColunaIsVault";
            this.ColunaIsVault.Width = 60;
            // 
            // colunaFolder
            // 
            this.colunaFolder.HeaderText = "PASTA";
            this.colunaFolder.Name = "colunaFolder";
            this.colunaFolder.Width = 200;
            // 
            // colunaArquivo
            // 
            this.colunaArquivo.HeaderText = "ARQUIVO";
            this.colunaArquivo.Name = "colunaArquivo";
            this.colunaArquivo.Width = 300;
            // 
            // colunaRBGBDETAILS
            // 
            this.colunaRBGBDETAILS.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colunaRBGBDETAILS.HeaderText = "RBGBDETAILS";
            this.colunaRBGBDETAILS.Name = "colunaRBGBDETAILS";
            // 
            // ColunaRBGBPRODUCERNAME
            // 
            this.ColunaRBGBPRODUCERNAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColunaRBGBPRODUCERNAME.HeaderText = "RBGBPRODUCERNAME";
            this.ColunaRBGBPRODUCERNAME.Name = "ColunaRBGBPRODUCERNAME";
            // 
            // ColunaRBGBPRODUCERORDERNO
            // 
            this.ColunaRBGBPRODUCERORDERNO.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColunaRBGBPRODUCERORDERNO.HeaderText = "RBGBPRODUCERORDERNO";
            this.ColunaRBGBPRODUCERORDERNO.Name = "ColunaRBGBPRODUCERORDERNO";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 221F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 118F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtZipFileName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSearchZip, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnExtrair, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.button1, 4, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(24, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1695, 30);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1592, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 24);
            this.button1.TabIndex = 8;
            this.button1.Text = " CHECK IN";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(24, 64);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.TreeBosch);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1702, 725);
            this.splitContainer1.SplitterDistance = 314;
            this.splitContainer1.TabIndex = 18;
            // 
            // LIstaIcones
            // 
            this.LIstaIcones.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("LIstaIcones.ImageStream")));
            this.LIstaIcones.TransparentColor = System.Drawing.Color.Transparent;
            this.LIstaIcones.Images.SetKeyName(0, ".ipt");
            this.LIstaIcones.Images.SetKeyName(1, ".iam");
            this.LIstaIcones.Images.SetKeyName(2, ".dwg");
            this.LIstaIcones.Images.SetKeyName(3, ".idw");
            this.LIstaIcones.Images.SetKeyName(4, ".ipn");
            this.LIstaIcones.Images.SetKeyName(5, "outros");
            // 
            // normaBindingSource
            // 
            this.normaBindingSource.DataSource = typeof(Bosch_ImportData.Norma);
            // 
            // ImportFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1744, 808);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
            this.tabDesconhecidos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TabelaDesconhecidos)).EndInit();
            this.tabATMOLIB.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabelaATMOLIB)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.normaBindingSource)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtZipFileName;
        private System.Windows.Forms.Button btnSearchZip;
        private System.Windows.Forms.Button btnExtrair;
        private System.Windows.Forms.ContextMenuStrip subMenu;
        public System.Windows.Forms.TreeView TreeBosch;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTabela;
        private System.Windows.Forms.TabPage tabDesconhecidos;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.DataGridView tabelaItens;
        public System.Windows.Forms.DataGridView TabelaDesconhecidos;
        private System.Windows.Forms.Button btnMover;
        private System.Windows.Forms.ComboBox txtFolderToMove;
        private System.Windows.Forms.ComboBox cbLocation;
        private System.Windows.Forms.BindingSource normaBindingSource;
        public System.Windows.Forms.ImageList LIstaIcones;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem CheckAll;
        private System.Windows.Forms.ToolStripMenuItem UncheckAll;
        private System.Windows.Forms.TabPage tabATMOLIB;
        public System.Windows.Forms.DataGridView tabelaATMOLIB;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColunaCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColunaIsVault;
        private System.Windows.Forms.DataGridViewTextBoxColumn colunaFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colunaArquivo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colunaRBGBDETAILS;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaRBGBPRODUCERNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaRBGBPRODUCERORDERNO;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColunaTipo;
        private System.Windows.Forms.DataGridViewCheckBoxColumn VaultExistColumn;
        private System.Windows.Forms.DataGridViewImageColumn ColunaExtensaoArquivo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaPasta;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaName;
        private System.Windows.Forms.DataGridViewButtonColumn ColunaRenomear;
        private System.Windows.Forms.DataGridViewTextBoxColumn originalNameColumn;
    }
}