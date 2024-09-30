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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.subMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CheckAll = new System.Windows.Forms.ToolStripMenuItem();
            this.UncheckAll = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.txtZipFileName = new System.Windows.Forms.TextBox();
            this.btnSearchZip = new System.Windows.Forms.Button();
            this.btnExtrair = new System.Windows.Forms.Button();
            this.TreeBosch = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCheckInNorma = new System.Windows.Forms.Button();
            this.BtnCheckIn = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabTabela = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnMoveFiles = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFolderToMove = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.tabelaItens = new System.Windows.Forms.DataGridView();
            this.ColunaItem = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isVault = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IsInventor = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColunaCategoria = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaTipoArquivo = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColunaPasta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaRenomear = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColunaProcurar = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColunaDeletar = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColunaMover = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColunaPreview = new System.Windows.Forms.DataGridViewImageColumn();
            this.tabATMOLIB = new System.Windows.Forms.TabPage();
            this.tabelaATMOLIB = new System.Windows.Forms.DataGridView();
            this.ColunaIsVault = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colunaFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colunaArquivo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colunaRBGBDETAILS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaRBGBPRODUCERNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaRBGBPRODUCERORDERNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InventorIconImageList = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.subMenu.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabTabela.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabelaItens)).BeginInit();
            this.tabATMOLIB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabelaATMOLIB)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // subMenu
            // 
            this.subMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.subMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CheckAll,
            this.UncheckAll});
            this.subMenu.Name = "subMenu";
            this.subMenu.Size = new System.Drawing.Size(156, 52);
            // 
            // CheckAll
            // 
            this.CheckAll.Name = "CheckAll";
            this.CheckAll.Size = new System.Drawing.Size(155, 24);
            this.CheckAll.Text = "Check All";
            this.CheckAll.Click += new System.EventHandler(this.CheckAll_Click);
            // 
            // UncheckAll
            // 
            this.UncheckAll.Name = "UncheckAll";
            this.UncheckAll.Size = new System.Drawing.Size(155, 24);
            this.UncheckAll.Text = "Uncheck All";
            this.UncheckAll.Click += new System.EventHandler(this.UncheckAll_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 30);
            this.label1.TabIndex = 5;
            this.label1.Text = " ARQUIVO";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtZipFileName
            // 
            this.txtZipFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtZipFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtZipFileName.Location = new System.Drawing.Point(123, 13);
            this.txtZipFileName.Name = "txtZipFileName";
            this.txtZipFileName.ReadOnly = true;
            this.txtZipFileName.Size = new System.Drawing.Size(914, 21);
            this.txtZipFileName.TabIndex = 4;
            this.txtZipFileName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnSearchZip
            // 
            this.btnSearchZip.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchZip.Location = new System.Drawing.Point(1043, 13);
            this.btnSearchZip.Name = "btnSearchZip";
            this.btnSearchZip.Size = new System.Drawing.Size(48, 24);
            this.btnSearchZip.TabIndex = 3;
            this.btnSearchZip.Text = "...";
            this.btnSearchZip.UseVisualStyleBackColor = true;
            this.btnSearchZip.Click += new System.EventHandler(this.btnSearchZip_Click);
            // 
            // btnExtrair
            // 
            this.btnExtrair.AutoSize = true;
            this.btnExtrair.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExtrair.Location = new System.Drawing.Point(1097, 13);
            this.btnExtrair.MinimumSize = new System.Drawing.Size(70, 0);
            this.btnExtrair.Name = "btnExtrair";
            this.btnExtrair.Size = new System.Drawing.Size(93, 24);
            this.btnExtrair.TabIndex = 7;
            this.btnExtrair.Text = "ABRIR";
            this.toolTip1.SetToolTip(this.btnExtrair, "LEGAL. VAMOS LA");
            this.btnExtrair.UseVisualStyleBackColor = true;
            this.btnExtrair.Click += new System.EventHandler(this.btnExtrair_Click);
            // 
            // TreeBosch
            // 
            this.TreeBosch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TreeBosch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeBosch.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TreeBosch.FullRowSelect = true;
            this.TreeBosch.ItemHeight = 20;
            this.TreeBosch.Location = new System.Drawing.Point(0, 10);
            this.TreeBosch.Name = "TreeBosch";
            treeNode1.Name = "Node0";
            treeNode1.Tag = "root";
            treeNode1.Text = "$";
            this.TreeBosch.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.TreeBosch.Size = new System.Drawing.Size(266, 763);
            this.TreeBosch.TabIndex = 14;
            this.TreeBosch.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewNorma_AfterSelect);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 128F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 123F));
            this.tableLayoutPanel1.Controls.Add(this.txtZipFileName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSearchZip, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnExtrair, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCheckInNorma, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.BtnCheckIn, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 30, 3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 10, 10, 0);
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1454, 40);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // btnCheckInNorma
            // 
            this.btnCheckInNorma.Enabled = false;
            this.btnCheckInNorma.Location = new System.Drawing.Point(1324, 13);
            this.btnCheckInNorma.Name = "btnCheckInNorma";
            this.btnCheckInNorma.Size = new System.Drawing.Size(117, 24);
            this.btnCheckInNorma.TabIndex = 9;
            this.btnCheckInNorma.Text = "NORMA CHECK IN";
            this.btnCheckInNorma.UseVisualStyleBackColor = true;
            // 
            // BtnCheckIn
            // 
            this.BtnCheckIn.AutoSize = true;
            this.BtnCheckIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnCheckIn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BtnCheckIn.Enabled = false;
            this.BtnCheckIn.Location = new System.Drawing.Point(1196, 14);
            this.BtnCheckIn.MaximumSize = new System.Drawing.Size(120, 0);
            this.BtnCheckIn.MinimumSize = new System.Drawing.Size(120, 0);
            this.BtnCheckIn.Name = "BtnCheckIn";
            this.BtnCheckIn.Size = new System.Drawing.Size(120, 23);
            this.BtnCheckIn.TabIndex = 8;
            this.BtnCheckIn.Text = "ATMOLIBN CHECK IN";
            this.toolTip1.SetToolTip(this.BtnCheckIn, "CLIQUE PARA FAZER O CHECJIN");
            this.BtnCheckIn.UseVisualStyleBackColor = true;
            this.BtnCheckIn.Click += new System.EventHandler(this.button1_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 40);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.TreeBosch);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.splitContainer1.Panel1MinSize = 60;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.splitContainer1.Size = new System.Drawing.Size(1454, 773);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 18;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTabela);
            this.tabControl1.Controls.Add(this.tabATMOLIB);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 10);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1184, 763);
            this.tabControl1.TabIndex = 16;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabTabela
            // 
            this.tabTabela.AccessibleName = "";
            this.tabTabela.BackColor = System.Drawing.Color.Transparent;
            this.tabTabela.Controls.Add(this.splitContainer2);
            this.tabTabela.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabTabela.Location = new System.Drawing.Point(4, 22);
            this.tabTabela.Name = "tabTabela";
            this.tabTabela.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.tabTabela.Size = new System.Drawing.Size(1176, 737);
            this.tabTabela.TabIndex = 0;
            this.tabTabela.Text = "TABELA";
            this.tabTabela.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 10);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 30, 3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabelaItens);
            this.splitContainer2.Size = new System.Drawing.Size(1170, 724);
            this.splitContainer2.SplitterDistance = 59;
            this.splitContainer2.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnMoveFiles);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtFolderToMove);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbLocation);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(780, 59);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MOVER ARQUIVOS EM LOTE";
            // 
            // BtnMoveFiles
            // 
            this.BtnMoveFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnMoveFiles.Location = new System.Drawing.Point(575, 21);
            this.BtnMoveFiles.Name = "BtnMoveFiles";
            this.BtnMoveFiles.Size = new System.Drawing.Size(190, 23);
            this.BtnMoveFiles.TabIndex = 34;
            this.BtnMoveFiles.Text = "MOVER ITENS SELECIONADOS";
            this.BtnMoveFiles.UseVisualStyleBackColor = true;
            this.BtnMoveFiles.Click += new System.EventHandler(this.BtnMoveFiles_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(307, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "PASTA";
            // 
            // txtFolderToMove
            // 
            this.txtFolderToMove.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtFolderToMove.FormattingEnabled = true;
            this.txtFolderToMove.Location = new System.Drawing.Point(355, 22);
            this.txtFolderToMove.Name = "txtFolderToMove";
            this.txtFolderToMove.Size = new System.Drawing.Size(214, 21);
            this.txtFolderToMove.TabIndex = 32;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "CATEGORIA";
            // 
            // cbLocation
            // 
            this.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocation.FormattingEnabled = true;
            this.cbLocation.Items.AddRange(new object[] {
            "$\\ATMOLIB\\Library\\Catalog",
            "$\\ATMOLIB\\Produtos Bosch",
            "$\\Sites\\CtP_TEF\\project",
            "$\\CONTENTCENTER\\en-US",
            "$\\CONTENTCENTER\\pt-BR",
            "$\\CONTENTCENTER\\pt-PT"});
            this.cbLocation.Location = new System.Drawing.Point(83, 22);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(211, 21);
            this.cbLocation.TabIndex = 30;
            this.cbLocation.SelectedIndexChanged += new System.EventHandler(this.cbLocation_SelectedIndexChanged);
            // 
            // tabelaItens
            // 
            this.tabelaItens.AllowUserToAddRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tabelaItens.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tabelaItens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tabelaItens.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColunaItem,
            this.isVault,
            this.IsInventor,
            this.ColunaCategoria,
            this.ColunaTipoArquivo,
            this.ColunaPasta,
            this.ColunaFile,
            this.ColunaRenomear,
            this.ColunaProcurar,
            this.ColunaDeletar,
            this.ColunaMover,
            this.ColunaPreview});
            this.tabelaItens.ContextMenuStrip = this.subMenu;
            this.tabelaItens.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabelaItens.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.tabelaItens.GridColor = System.Drawing.Color.SeaShell;
            this.tabelaItens.Location = new System.Drawing.Point(0, 0);
            this.tabelaItens.Name = "tabelaItens";
            this.tabelaItens.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tabelaItens.Size = new System.Drawing.Size(1170, 661);
            this.tabelaItens.TabIndex = 0;
            this.tabelaItens.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.TabelaItens_CellClick);
            // 
            // ColunaItem
            // 
            this.ColunaItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ColunaItem.HeaderText = "ITEM";
            this.ColunaItem.Name = "ColunaItem";
            this.ColunaItem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColunaItem.ToolTipText = "Selecione os itens para executar uma ação em lote";
            this.ColunaItem.Width = 50;
            // 
            // isVault
            // 
            this.isVault.HeaderText = "ESTÁ NO VAULT";
            this.isVault.Name = "isVault";
            this.isVault.ReadOnly = true;
            this.isVault.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isVault.ToolTipText = "Arquivos que já estão cadastrados no Vault";
            this.isVault.Width = 80;
            // 
            // IsInventor
            // 
            this.IsInventor.HeaderText = "ESTÁ NO INVENTOR";
            this.IsInventor.Name = "IsInventor";
            this.IsInventor.ReadOnly = true;
            this.IsInventor.ToolTipText = "Arquivos que fazem parte da montagem principal do Inventor";
            this.IsInventor.Width = 80;
            // 
            // ColunaCategoria
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColunaCategoria.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColunaCategoria.HeaderText = "CATEGORIA";
            this.ColunaCategoria.Name = "ColunaCategoria";
            this.ColunaCategoria.ReadOnly = true;
            this.ColunaCategoria.ToolTipText = "Classificação do arquivo (Norma, AtmoLib ou Content Center)";
            this.ColunaCategoria.Width = 130;
            // 
            // ColunaTipoArquivo
            // 
            this.ColunaTipoArquivo.HeaderText = "TIPO";
            this.ColunaTipoArquivo.Name = "ColunaTipoArquivo";
            this.ColunaTipoArquivo.ReadOnly = true;
            this.ColunaTipoArquivo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColunaTipoArquivo.ToolTipText = "Tipo de arquivo do Inventor (peça, montagem ou detalhamento)";
            this.ColunaTipoArquivo.Width = 60;
            // 
            // ColunaPasta
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColunaPasta.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColunaPasta.HeaderText = "PASTA";
            this.ColunaPasta.Name = "ColunaPasta";
            this.ColunaPasta.ReadOnly = true;
            this.ColunaPasta.ToolTipText = "Pasta onde está o arquivo";
            this.ColunaPasta.Width = 140;
            // 
            // ColunaFile
            // 
            this.ColunaFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColunaFile.HeaderText = "ARQUIVO";
            this.ColunaFile.Name = "ColunaFile";
            this.ColunaFile.ReadOnly = true;
            this.ColunaFile.ToolTipText = "Nome do arquivo";
            // 
            // ColunaRenomear
            // 
            this.ColunaRenomear.Description = "Renomear Arquivo";
            this.ColunaRenomear.HeaderText = "RENAME";
            this.ColunaRenomear.Image = global::Bosch_ImportData.Properties.Resources.ColunaRenomear;
            this.ColunaRenomear.Name = "ColunaRenomear";
            this.ColunaRenomear.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColunaRenomear.ToolTipText = "Renomear o arquivo";
            this.ColunaRenomear.Width = 60;
            // 
            // ColunaProcurar
            // 
            this.ColunaProcurar.HeaderText = "SEARCH";
            this.ColunaProcurar.Image = global::Bosch_ImportData.Properties.Resources.ColunaProcurar;
            this.ColunaProcurar.Name = "ColunaProcurar";
            this.ColunaProcurar.ToolTipText = "Substituir por um arquivo local";
            this.ColunaProcurar.Width = 60;
            // 
            // ColunaDeletar
            // 
            this.ColunaDeletar.HeaderText = "REMOVE";
            this.ColunaDeletar.Image = global::Bosch_ImportData.Properties.Resources.ColunaDeletar;
            this.ColunaDeletar.Name = "ColunaDeletar";
            this.ColunaDeletar.ToolTipText = "Deletar Arquivo";
            this.ColunaDeletar.Width = 60;
            // 
            // ColunaMover
            // 
            this.ColunaMover.HeaderText = "MOVE";
            this.ColunaMover.Image = global::Bosch_ImportData.Properties.Resources.ColunaMover;
            this.ColunaMover.Name = "ColunaMover";
            this.ColunaMover.ToolTipText = "Mover Arquivo";
            this.ColunaMover.Width = 60;
            // 
            // ColunaPreview
            // 
            this.ColunaPreview.HeaderText = "PREVIEW";
            this.ColunaPreview.Image = global::Bosch_ImportData.Properties.Resources.ColunaPreview;
            this.ColunaPreview.Name = "ColunaPreview";
            this.ColunaPreview.Width = 60;
            // 
            // tabATMOLIB
            // 
            this.tabATMOLIB.Controls.Add(this.tabelaATMOLIB);
            this.tabATMOLIB.Location = new System.Drawing.Point(4, 22);
            this.tabATMOLIB.Name = "tabATMOLIB";
            this.tabATMOLIB.Padding = new System.Windows.Forms.Padding(3);
            this.tabATMOLIB.Size = new System.Drawing.Size(1176, 727);
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
            this.tabelaATMOLIB.Size = new System.Drawing.Size(1170, 721);
            this.tabelaATMOLIB.StandardTab = true;
            this.tabelaATMOLIB.TabIndex = 1;
            this.tabelaATMOLIB.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tabelaATMOLIB_CellDoubleClick);
            this.tabelaATMOLIB.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.tabelaATMOLIB_CellValueChanged);
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
            // InventorIconImageList
            // 
            this.InventorIconImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.InventorIconImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.InventorIconImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 3000;
            this.toolTip1.BackColor = System.Drawing.Color.Honeydew;
            this.toolTip1.ForeColor = System.Drawing.Color.Navy;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.OwnerDraw = true;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ShowAlways = true;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "DICA";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(780, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(390, 59);
            this.groupBox2.TabIndex = 33;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RENOMEAR ARQUIVOS EM LOTE";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(636, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 34;
            this.button1.Text = "MOVER";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(189, 38);
            this.label4.TabIndex = 35;
            this.label4.Text = "Esse comando irá adicionar o nome da pasta antes do nome do arquivo";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(179, 26);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(199, 23);
            this.button2.TabIndex = 36;
            this.button2.Text = "RENOMEAR ITENS SELECIONADOS";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // ImportFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1454, 813);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(800, 400);
            this.Name = "ImportFiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BOSCH - NÃO CONECTADO";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImportFiles_FormClosing);
            this.Load += new System.EventHandler(this.ImportFiles_Load);
            this.subMenu.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabTabela.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabelaItens)).EndInit();
            this.tabATMOLIB.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabelaATMOLIB)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtZipFileName;
        private System.Windows.Forms.Button btnSearchZip;
        private System.Windows.Forms.Button btnExtrair;
        private System.Windows.Forms.ContextMenuStrip subMenu;
        public System.Windows.Forms.TreeView TreeBosch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.ImageList InventorIconImageList;
        private System.Windows.Forms.ToolStripMenuItem CheckAll;
        private System.Windows.Forms.ToolStripMenuItem UncheckAll;
        private System.Windows.Forms.Button BtnCheckIn;
        private System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTabela;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnMoveFiles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox txtFolderToMove;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbLocation;
        public System.Windows.Forms.DataGridView tabelaItens;
        private System.Windows.Forms.TabPage tabATMOLIB;
        public System.Windows.Forms.DataGridView tabelaATMOLIB;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColunaIsVault;
        private System.Windows.Forms.DataGridViewTextBoxColumn colunaFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colunaArquivo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colunaRBGBDETAILS;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaRBGBPRODUCERNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaRBGBPRODUCERORDERNO;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColunaItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isVault;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsInventor;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaCategoria;
        private System.Windows.Forms.DataGridViewImageColumn ColunaTipoArquivo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaPasta;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaFile;
        private System.Windows.Forms.DataGridViewImageColumn ColunaRenomear;
        private System.Windows.Forms.DataGridViewImageColumn ColunaProcurar;
        private System.Windows.Forms.DataGridViewImageColumn ColunaDeletar;
        private System.Windows.Forms.DataGridViewImageColumn ColunaMover;
        private System.Windows.Forms.DataGridViewImageColumn ColunaPreview;
        private System.Windows.Forms.Button btnCheckInNorma;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
    }
}