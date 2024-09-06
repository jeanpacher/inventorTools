using Inventor;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

using Path = System.IO.Path;
using File = System.IO.File;
using Color = System.Drawing.Color;
using config = Bosch_ImportData.Properties.Settings;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;



namespace Bosch_ImportData
{
    public partial class ImportFiles : Form
    {
        public Norma codNorma { get; set; }

        public ImportFiles()
        {
            InitializeComponent();
        }
        public ImportFiles(Inventor.Application invApp)
        {
            InitializeComponent();
            //_invApp = invApp;
        }

        #region EVENTOS
        private void ImportFiles_Load(object sender, EventArgs e)
        {
            this.Show();
            ToggleWaitCursor(true);

            if (!VaultHelper.ConectarWithAD())
            {
                if (!VaultHelper.ConectarWithUserAndPassword())
                {
                    MessageBox.Show("Não foi possivel conectar no Autodesk Vault. Verifique a conexão com o servidor." +
                        "O software será encerrado", "ERRO DE CONEXÂO COM O VAULT", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    this.Close();

                }
            }

            this.Text = "BOSCH - CONECTADO";
            ToggleWaitCursor(false);
        }
        private void ImportFiles_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Parametros._invApp.SilentOperation = false;

            }
            catch (Exception e1)
            {

                Log.gravarLog(e1.ToString() + System.Environment.NewLine);
            }

        }
        private void btnSearchZip_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Arquivos ZIP ou RAR (*.zip;*.rar)|*.zip;*.rar";
                openFileDialog.Title = "Selecione o projeto";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtZipFileName.Text = openFileDialog.FileName;
                }
            }
        }
        private void btnExtrair_Click(object sender, EventArgs e)
        {
            ToggleWaitCursor(true);
            ClearUI();

            Parametros.isSystemChange = true;

            codNorma = new Norma();         // Reinicializa codNorma para garantir que está limpo
            SetTemporaryPaths();

            if (!ExtractFilesFromZip(txtZipFileName.Text)) return;

            CreateTextFileWithFileNames();

            if (VerifyLinks())
                MessageBox.Show("TODOS OS ARQUIVOS NECESSÁRIOS FORAM ENCONTRADOS");
            else
                MessageBox.Show("EXISTEM VINCULADOS DE ARQUIVOS FALTANDO NA MONTAGEM PRINCIPAL");

            PopulateTreeView();
            CriarTabelaDesconhecida();
            ToggleWaitCursor(false);

            Parametros.isSystemChange = false;

        }
        private void btnMover_Click(object sender, EventArgs e)
        {
            MoverArquivo();
        }
        private void tabelaItens_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex == 3)
            //{
            //    MessageBox.Show(tabelaItens.Rows[e.RowIndex].Cells[2].Value.ToString());
            //}
        }
        private void tabelaItens_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            return;
            //if (!IsValidFileName(newFileName))

        }
        private void TreeViewNorma_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Parametros.isSystemChange = true;
            tabelaItens.Rows.Clear();
            if (e.Node.Text.Contains('.'))
            {
                Produto prod = e.Node.Tag as Produto;
                AdicionarLinhaTabelaItens(prod);
            }
            CriarTabelaItens(e.Node.Nodes);
            Parametros.isSystemChange = false;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (codNorma == null) return;

            TabelaDesconhecidos.Rows.Clear();

            if (tabControl1.SelectedTab.Text == "DESCONHECIDOS")
            {
                CriarTabelaDesconhecida();
            }

            if (tabControl1.SelectedTab.Text == "TABELA")
            {
                TreeBosch.SelectedNode = TreeBosch.TopNode;
            }

            if (tabControl1.SelectedTab.Text == "ATMOLIB")
            {
                CriarTabelaPropriedades();
            }
        }
        private void cbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFolderToMove.Items.Clear();
            txtFolderToMove.Text = string.Empty;

            List<string> ListasPastasDisponiveis = new List<string>();

            if (VaultHelper.connection != null)
            {
                ListasPastasDisponiveis = VaultHelper.GetFoldersByPath(cbLocation.Text);
            }

            if (Parametros.DicionarioNodes.TryGetValue(cbLocation.Text.Split('\\').Last(), out TreeNode CurrentNode))
            {
                if (CurrentNode.Nodes.Count > 0)
                {
                    foreach (TreeNode ChildrenNode in CurrentNode.Nodes)
                    {
                        if (!ListasPastasDisponiveis.Contains(ChildrenNode.Text))
                            ListasPastasDisponiveis.Add(ChildrenNode.Text);
                    }
                }
            }
            ListasPastasDisponiveis.Sort();
            txtFolderToMove.Items.AddRange(ListasPastasDisponiveis.ToArray());
        }
        private void tabelaItens_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (tabelaItens.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                Produto prod = (Produto)tabelaItens.Rows[e.RowIndex].Tag;
                if (prod.isVaultExisting)
                {
                    MessageBox.Show("Esse arquivo não pode ser renomeado pois já está cadastrado no Vault.");
                    return;
                }
                string oldFileName = prod.NewFileName;
                string newFileName = Microsoft.VisualBasic.Interaction.InputBox("Digite o novo nome do arquivo:" + Path.GetFileName(prod.NewFileName),
                                                                          "Renomear Arquivo", Path.GetFileNameWithoutExtension(prod.NewFileName));

                if (string.IsNullOrWhiteSpace(newFileName) || newFileName == Path.GetFileNameWithoutExtension(prod.NewFileName))
                    return;


                newFileName += Path.GetExtension(prod.NewFileName);
                newFileName = Path.Combine(Path.GetDirectoryName(prod.NewFileName), newFileName);

                ProcurarArquivosToRename(newFileName, oldFileName);
                prod.OldFileName = oldFileName;
                tabelaItens.Rows[e.RowIndex].Cells["ColunaName"].Value = Path.GetFileNameWithoutExtension(newFileName);
                tabelaItens.Rows[e.RowIndex].Cells["originalNameColumn"].Value = newFileName;
                atualizarProduto(prod, newFileName);
                codNorma.oAsmDoc.Update2(true);
                codNorma.oAsmDoc.Save2(true);
                MessageBox.Show("Arquivo renomeado com sucesso, links atualizados.");
            }
        }
        private void tabelaATMOLIB_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (Parametros.isSystemChange)
                return;

            Produto prod = (Produto)tabelaATMOLIB.Rows[e.RowIndex].Tag;
            try
            {

                if (e.ColumnIndex == tabelaATMOLIB.Columns["ColunaRBGBDETAILS"].Index)
                {
                    prod.propriedades.ChangePropertyValue(prod, "RBGBDETAILS", tabelaATMOLIB.CurrentCell.Value.ToString());
                    prod.propriedades.RBGBDETAILS = tabelaATMOLIB.CurrentCell.Value.ToString();
                }
                if (e.ColumnIndex == tabelaATMOLIB.Columns["ColunaRBGBPRODUCERNAME"].Index)
                {
                    prod.propriedades.ChangePropertyValue(prod, "RBGBPRODUCERNAME", tabelaATMOLIB.CurrentCell.Value.ToString());
                    prod.propriedades.RBGBPRODUCERNAME = tabelaATMOLIB.CurrentCell.Value.ToString();
                }
                if (e.ColumnIndex == tabelaATMOLIB.Columns["ColunaRBGBPRODUCERORDERNO"].Index)
                {
                    prod.propriedades.ChangePropertyValue(prod, "RBGBPRODUCERORDERNO", tabelaATMOLIB.CurrentCell.Value.ToString());
                    prod.propriedades.RBGBPRODUCERORDERNO = tabelaATMOLIB.CurrentCell.Value.ToString();
                }
            }
            catch (Exception e1)
            {
                Log.gravarLog($"Erro ao gravar a propriedade do arquivo: {Path.GetFileName(prod.NewFileName)} {System.Environment.NewLine} ERRO: {e1.ToString()}");
            }
        }
        #endregion
        private void ToggleWaitCursor(bool isWaitCursor)
        {
            this.UseWaitCursor = isWaitCursor;
        }
        private void ClearUI()
        {
            Parametros._invApp.Documents.CloseAll();
            TreeBosch.Nodes.Clear();
            tabelaItens.Rows.Clear();
            TabelaDesconhecidos.Rows.Clear();
            tabelaATMOLIB.Rows.Clear();

            cbLocation.Text = string.Empty;
            txtFolderToMove.Text = string.Empty;

            if (tabControl1.TabCount > 2)
                tabControl1.TabPages.Remove(tabDesconhecidos);
        }
        private void SetTemporaryPaths()
        {
            Properties.Settings.Default.tempVaultRootPath = Path.Combine(Parametros.tempLocation, Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            Parametros.ipjPadrao = Path.Combine(config.Default.tempVaultRootPath, Parametros.ipjFileName);
        }
        private bool ExtractFilesFromZip(string zipFileName)
        {
            if (string.IsNullOrEmpty(zipFileName) || !File.Exists(zipFileName))
            {
                ToggleWaitCursor(false);
                return false;
            }

            ZipfileManipulate zipfileManipulate = new ZipfileManipulate(tabelaItens, LIstaIcones);
            codNorma = zipfileManipulate.ExtractZip(zipFileName);

            return true;
        }
        private void PopulateTreeView()
        {
            // CRIAR A TREEVIEW
            TreeViewBosch tb = new TreeViewBosch(TreeBosch);
            tb.PopulateTreeView(codNorma);
            TreeBosch.SelectedNode = TreeBosch.TopNode;
        }
        private void CreateTextFileWithFileNames()
        {
            foreach (Produto prod in codNorma.Produtos)
            {
                string filePath = config.Default.tempVaultRootPath + codNorma.CodigoNorma + ".txt";
                File.AppendAllText(filePath, prod.NewFileName + System.Environment.NewLine);
            }
        }
        private bool VerifyLinks()
        {

            Parametros.mainAssemblyPath = Directory.GetFiles(config.Default.tempVaultRootPath, codNorma.CodigoNorma + ".iam", SearchOption.AllDirectories).FirstOrDefault();
            string mainDrawingPath = Directory.GetFiles(config.Default.tempVaultRootPath, codNorma.CodigoNorma + ".idw", SearchOption.AllDirectories).FirstOrDefault();
            if (string.IsNullOrEmpty(Parametros.mainAssemblyPath))
            {
                MessageBox.Show("Montagem não encontrada");
                Parametros.isResolved = false;
                return Parametros.isResolved;
            }

            if (!File.Exists(config.Default.DefaultIPJ))
            {
                MessageBox.Show("Arquivo IPJ não encontrado.\nVerifique se existe o arquivo: " + config.Default.DefaultIPJ);
                Parametros.isResolved = false;
                return Parametros.isResolved;
            }
            FileInfo ipjFile = new FileInfo(config.Default.DefaultIPJ);
            ipjFile.CopyTo(Parametros.ipjPadrao, true);
            Parametros.isResolved = true;

            try
            {
                if (Parametros._invApp.Documents.Count > 0)
                    Parametros._invApp = (Inventor.Application)Activator.CreateInstance(Type.GetTypeFromProgID("Inventor.Application"));
            }
            catch (Exception)
            {
                //Parametros._invApp = (Inventor.Application)Activator.CreateInstance(Type.GetTypeFromProgID("Inventor.Application"));
                Log.gravarLog("Erro ao pegar a instancia do Inventor");
            }


            Parametros._invApp.Visible = true;
            Parametros._invApp.SilentOperation = true;

            //AssemblyDocument asmDoc = null;
            if (Parametros._invApp.Documents.Count == 0)
            {
                // Abrir um novo projeto
                Parametros._invApp.DesignProjectManager.DesignProjects.AddExisting(Parametros.ipjPadrao).Activate();
                codNorma.oAsmDoc = Parametros._invApp.Documents.Open(Parametros.mainAssemblyPath, true) as AssemblyDocument;
                codNorma.Produtos.Find(x => x.NewFileName == Parametros.mainAssemblyPath).isAssemblyParticipant = true;
                checkLinkRecursive(codNorma.oAsmDoc);
            }
            else
            {
                MessageBox.Show("Todos os arquivos do Inventor precisam estar fechados. Por favor, feche todos os arquivos e tente novamente");
            }

            Parametros._invApp.SilentOperation = false;
            return Parametros.isResolved;
        }
        public void checkLinkRecursive(AssemblyDocument asmDoc)
        {
            foreach (DocumentDescriptor descriptor in asmDoc.ReferencedDocumentDescriptors)
            {
                if (descriptor.ReferenceMissing)
                {
                    Produto prod = new Produto(descriptor.FullDocumentName, codNorma.CodigoNorma, true);
                    codNorma.Produtos.Add(prod);
                    Log.gravarLog($"Arquivo faltante: {descriptor.FullDocumentName} {System.Environment.NewLine}");
                    Parametros.isResolved = false;
                }
                else
                {
                    Produto prod = codNorma.Produtos.Find(x => x.NewFileName == descriptor.FullDocumentName);

                    if (prod == null)
                    {
                        MessageBox.Show("Produto nulo: " + descriptor.FullDocumentName);
                    }
                    else if (prod.Type == ProductType.ATMOLIB_Library)
                    {
                        Document doc = Parametros._invApp.Documents.Open(descriptor.FullDocumentName, false);
                        prod.Doc = doc;
                        prod.propriedades = new Propriedades(doc);

                    }
                    prod.isAssemblyParticipant = true;
                    if (descriptor.FullDocumentName.EndsWith(".iam"))
                    {
                        checkLinkRecursive(Parametros._invApp.Documents.Open(descriptor.FullDocumentName, false) as AssemblyDocument);
                    }
                }
            }
        }
        private void CreateReportFilesFromProdutos()
        {
            foreach (Produto prod in codNorma.Produtos)
            {
                string filePath = config.Default.tempVaultRootPath + codNorma.CodigoNorma + ".txt";
                File.AppendAllText(filePath, prod.NewFileName + System.Environment.NewLine);
            }
        }
        public void ProcurarArquivosToRename(string newfilename, string oldfilename)
        {
            Parametros._invApp.SilentOperation = true;

            Document document = codNorma.oAsmDoc as Document;
            Dictionary<string, Document[]> dicionarioDocsToRename = new Dictionary<string, Document[]>();
            dicionarioDocsToRename.Clear();

            if (document.AllReferencedDocuments == null)
            {
                return;
            }
            foreach (Document DocToRename in document.AllReferencedDocuments)
            {
                if (DocToRename.FullFileName == oldfilename)
                {
                    foreach (Document docParent in DocToRename.ReferencingDocuments)
                    {
                        if (dicionarioDocsToRename.ContainsKey(docParent.FullFileName))
                            continue;

                        Document[] docs = new Document[]
                        {
                            docParent, DocToRename
                        };

                        dicionarioDocsToRename.Add(docParent.FullFileName, docs);
                    }
                }
            }
            RenomearArquivos(dicionarioDocsToRename, newfilename, oldfilename);

            Parametros._invApp.SilentOperation = false;

        }
        public void RenomearArquivos(Dictionary<string, Document[]> dicionario, string newfilename, string oldfilename)
        {
            foreach (KeyValuePair<string, Document[]> item in dicionario)
            {
                if (!item.Key.EndsWith(".iam"))
                    continue;
                try
                {
                    AssemblyDocument oAsm = Parametros._invApp.Documents.Open(item.Key, true) as AssemblyDocument;
                    foreach (ComponentOccurrence occ in oAsm.ComponentDefinition.Occurrences)
                    {
                        if (occ.Definition.Document == null)
                            continue;
                        if (occ.ReferencedFileDescriptor == null)
                            continue;
                        if (occ.ReferencedFileDescriptor.FullFileName == oldfilename)
                        {
                            Document oDoc = (Document)occ.Definition.Document;
                            if (oDoc != null)
                            {
                                oDoc.SaveAs(newfilename, false);
                                oDoc.PropertySets["Design Tracking Properties"]["Part Number"].Value = Path.GetFileNameWithoutExtension(newfilename);
                                apagarArquivo(oldfilename);
                            }
                        }
                    }
                    oAsm.Save();
                    oAsm.Save2(true);

                    if (oAsm.FullFileName != codNorma.oAsmDoc.FullFileName)
                        oAsm.Close();

                }
                catch (Exception e1)
                {
                    Log.gravarLog(e1.Message);
                }
            }
        }
        public bool apagarArquivo(string filename)
        {
            try
            {
                File.Delete(filename);
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Arquivo não foi apagado: \n" + ex.ToString());
                return false;
            }
        }
        private void OcultarTabDesconhecidos()
        {
            var desconhecidos = codNorma.Produtos.Where(x => x.Type == ProductType.Desconhecido).ToList();

            if (desconhecidos.Count == 0)
            {
                tabControl1.TabPages.Remove(tabDesconhecidos);
            }
        }
        private void atualizarProduto(Produto prod, string newFileName)
        {
            prod.NewFileName = newFileName;
            prod.FileNameSimplificado = newFileName.Replace(config.Default.tempVaultRootPath, @"$\");
            prod.node.Text = Path.GetFileName(prod.NewFileName);
        }

        // Método para validar o nome do arquivo
        private bool IsValidFileName(string fileName)
        {
            string invalidChars = new string(Path.GetInvalidFileNameChars());
            Regex regex = new Regex($"[{Regex.Escape(invalidChars)}]");
            return true;
            // return !regex.IsMatch(fileName);
        }
        public void PesquisarTreeNodes(TreeNode nodeParent)
        {
            foreach (TreeNode node in nodeParent.Nodes)
            {
                if (node.Parent == null) continue;

                if (nodeParent.Text == "Catalog")
                {
                    foreach (TreeNode nodeFilho in nodeParent.Nodes)
                    {
                        Parametros.pastas.Add(nodeFilho.Text);
                    }
                    return;
                }

                if (node.Nodes != null)
                {
                    PesquisarTreeNodes(node);
                }
            }
        }

        #region TABELAS
        public void CriarTabelaItens(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                Produto prod = node.Tag as Produto;
                if (prod == null || !node.Text.Contains('.'))
                {
                    CriarTabelaItens(node.Nodes);
                }
                else
                {
                    AdicionarLinhaTabelaItens(prod);

                    if (node.Nodes.Count > 0)
                    {
                        CriarTabelaItens(node.Nodes);
                    }
                }
            }
        }
        public void AdicionarLinhaTabelaItens(Produto prod)
        {
            string displayName = TableFormat.GetDisplayName(prod);

            int rowIndex = tabelaItens.Rows.Add(
               prod.Type.ToString(),
               prod.isVaultExisting,
               LIstaIcones.Images[prod.IconName],
               Path.GetDirectoryName(prod.NewFileName).Split('\\').Last(),
               Path.GetFileNameWithoutExtension(prod.NewFileName),
               "RENOMEAR",
               prod.isAssemblyParticipant,
               prod.NewFileName);

            tabelaItens.Rows[rowIndex].Tag = prod;
            tabelaItens.Rows[rowIndex].DefaultCellStyle.ForeColor = TableFormat.GetRowColor(prod);
            if (prod.isVaultExisting)
                tabelaItens.Rows[rowIndex].ReadOnly = true;
        }
        private void CriarTabelaDesconhecida()
        {
            if (codNorma.Produtos.Any(x => x.Type == ProductType.Desconhecido))
            {
                if (!tabControl1.TabPages.Contains(tabDesconhecidos))
                    tabControl1.TabPages.Add(tabDesconhecidos);

                foreach (Produto prod in codNorma.Produtos)
                {
                    if (prod.Type == ProductType.Desconhecido)
                    {
                        AdicionarLinhaTabelaDesconhecidos(prod);
                    }
                }
            }
            else
            {
                if (tabControl1.TabPages.Contains(tabDesconhecidos))
                    tabControl1.TabPages.Remove(tabDesconhecidos);
            }
        }
        public void CriarTabelaPropriedades()
        {
            tabelaATMOLIB.Rows.Clear();

            foreach (Produto prod in codNorma.Produtos)
            {

                if (prod.Type == ProductType.ATMOLIB_Library && !prod.isMissing)
                {
                    if (prod.propriedades == null) continue;

                    int rowIndex = tabelaATMOLIB.Rows.Add(
                           prod.isVaultExisting,
                           Path.GetDirectoryName(prod.NewFileName).Split('\\').Last(),
                           Path.GetFileName(prod.NewFileName),
                           prod.propriedades.RBGBDETAILS,
                           prod.propriedades.RBGBPRODUCERNAME,
                           prod.propriedades.RBGBPRODUCERORDERNO);

                    tabelaATMOLIB.Rows[rowIndex].Tag = prod;
                    tabelaATMOLIB.Rows[rowIndex].ReadOnly = prod.isVaultExisting;

                }
            }
        }
        private void AdicionarLinhaTabelaDesconhecidos(Produto prod)
        {

            int rowIndex = TabelaDesconhecidos.Rows.Add(
                    false,
                    $"{Path.GetFileName(prod.NewFileName)}",
                    prod.Filename);

            TabelaDesconhecidos.Rows[rowIndex].Tag = prod;
        }
        public bool MoverArquivo()
        {
            if (string.IsNullOrEmpty(txtFolderToMove.Text))
                return false;

            if (string.IsNullOrEmpty(cbLocation.Text))
                return false;

            ProductType productType;

            switch (cbLocation.Text.Split('\\').Last())
            {
                case "Catalog":
                    productType = ProductType.ATMOLIB_Library;
                    break;

                case "Produtos Bosch":
                    productType = ProductType.ATMOLIB_ProdutosBosch;
                    break;

                case "project":
                    productType = ProductType.Norma;
                    break;

                default:
                    productType = ProductType.ContentCenter;
                    break;

            }

            TreeViewBosch treeBosch = new TreeViewBosch(TreeBosch);
            List<DataGridViewRow> listaRows = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in TabelaDesconhecidos.Rows)
            {
                if (Convert.ToBoolean(row.Cells["ColunaCheck"].Value) == true)
                {
                    Produto prod = (Produto)row.Tag;

                    TreeBosch.Nodes.Remove(prod.node);
                    Parametros.DicionarioNodes.Remove(Path.GetFileName(prod.NewFileName));

                    prod.Type = productType;
                    prod.OldFileName = prod.NewFileName;
                    prod.FileNameSimplificado = Path.Combine(cbLocation.Text, txtFolderToMove.Text, Path.GetFileName(prod.NewFileName));
                    prod.NewFileName = Path.Combine(config.Default.tempVaultRootPath, prod.FileNameSimplificado.Substring(2));

                    treeBosch.NodeCreate(prod, Parametros.DicionarioNodes);
                    listaRows.Add(row);

                    if (!Directory.Exists(Path.GetDirectoryName(prod.NewFileName)))
                        Directory.CreateDirectory(Path.GetDirectoryName(prod.NewFileName));

                    ProcurarArquivosToRename(prod.NewFileName, prod.OldFileName);
                    if (prod.Type == ProductType.ATMOLIB_Library)
                    {
                        Document doc = Parametros._invApp.Documents.Open(prod.NewFileName, false);
                        prod.Doc = doc;
                        prod.propriedades = new Propriedades(doc);
                    }

                    codNorma.oAsmDoc.Update2(true);
                    codNorma.oAsmDoc.Save2(true);
                    //File.Move(prod.OldFileName, prod.NewFileName);

                }
            }
            listaRows.ForEach(TabelaDesconhecidos.Rows.Remove);

            if (TabelaDesconhecidos.RowCount == 0)
            {
                tabControl1.TabPages.Remove(tabDesconhecidos);
                if (Parametros.DicionarioNodes.TryGetValue("DESCONHECIDO", out TreeNode node))
                {
                    TreeBosch.Nodes.Remove(node);
                    tabelaItens.Rows.Clear();
                    TreeBosch.SelectedNode = TreeBosch.TopNode;
                    CriarTabelaItens(TreeBosch.Nodes);
                }
            }
            return true;
        }

        private void CheckAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in TabelaDesconhecidos.SelectedRows)
            {
                row.Cells[0].Value = true;
            }
        }
        #endregion
        private void UncheckAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in TabelaDesconhecidos.SelectedRows)
            {
                row.Cells[0].Value = false;
            }
        }
        private void AtualizarReferenciaNaMontagem(Produto prod)
        {
            AssemblyDocument assemblyDoc = Parametros._invApp.ActiveDocument as AssemblyDocument;

            if (assemblyDoc != null)
            {
                foreach (ComponentOccurrence occ in assemblyDoc.ComponentDefinition.Occurrences)
                {
                    if (occ.ReferencedDocumentDescriptor.FullDocumentName == prod.OldFileName)
                    {
                        // Atualiza o caminho da referência para o novo local do arquivo
                        occ.Replace(prod.NewFileName, true);
                    }
                }
                // Salva a montagem após as alterações
                assemblyDoc.Update();
                assemblyDoc.Save2(true);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Parametros._invApp.SilentOperation = true;
            FecharArquivos();

            if (codNorma.Produtos.Any(x => x.isMissing))
            {
                MessageBox.Show("Existem arquivos faltando na montagem");
                return;
            }

            else if (Parametros.DicionarioNodes.TryGetValue("$", out TreeNode nodePai))
            {
                CriarEstruturaPastas(nodePai.Nodes);
                CopiarArquivosToVaultFolder();
                AbrirMontagemToCheckIn();

                //GerarRelatorioByNodes(TreeBosch.Nodes);
                //GerarRelatorioByProduto();

                MessageBox.Show("Finalizado");
                Parametros._invApp.SilentOperation = false;
            }
        }
        public void FecharArquivos()
        {
            Parametros._invApp.Documents.CloseAll();
        }
        public bool CopiarArquivosToVaultFolder()
        {

            foreach (Produto prod in codNorma.Produtos)
            {
                prod.OldFileName = prod.NewFileName;
                prod.NewFileName = Path.Combine(Parametros.VaultWorkFolder, prod.FileNameSimplificado.Substring(2));
                try
                {
                    if (!File.Exists(prod.NewFileName))
                        File.Copy(prod.OldFileName, prod.NewFileName);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            return true;

        }
        public void AbrirMontagemToCheckIn()
        {

            if (Parametros._invApp.Documents.Count == 0)
            {
                // Abrir um novo projeto
                Parametros._invApp.DesignProjectManager.DesignProjects.AddExisting(Path.Combine(Parametros.VaultWorkFolder, Parametros.VaultProject)).Activate();
                Produto produto = codNorma.Produtos.Find(x => x.OldFileName == Parametros.mainAssemblyPath);
                codNorma.oAsmDoc = Parametros._invApp.Documents.Open(produto.NewFileName, true) as AssemblyDocument;
                ExecutarComandoCheckIn();
            }
        }
        public void CriarEstruturaPastas(TreeNodeCollection Nodes)
        {
            foreach (TreeNode node in Nodes)
            {

                try
                {
                    string extension = node.FullPath.Split('.').Last();

                    // string arquivo = Path.Combine(config.Default.tempVaultRootPath, node.FullPath.Substring(2));
                    if (extension.Length > 3)
                    {
                        string directory = Path.Combine(Parametros.VaultWorkFolder, node.FullPath.Substring(2));
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                    }

                    if (node.Nodes.Count > 0)
                    {
                        CriarEstruturaPastas(node.Nodes);
                    }
                }
                catch (Exception e1)
                {

                    Log.gravarLog(e1.Message);
                }

            }
        }
        public void GerarRelatorioByNodes(TreeNodeCollection Nodes)
        {
            string VaultWorkFolder = $@"C:\daten\users\{System.Environment.UserName}\Vault-Root";

            foreach (TreeNode node in Nodes)
            {
                if (node.Text.Contains("."))
                {
                    Produto prod = (Produto)node.Tag;
                    prod.NewFileName = Path.Combine(VaultWorkFolder, prod.FileNameSimplificado.Substring(2));
                    GerarRelatorio("relatorioNodes.txt", prod);
                }
                if (node.Nodes.Count > 0)
                {
                    GerarRelatorioByNodes(node.Nodes);
                }
            }

        }
        public void GerarRelatorioByProduto()
        {
            string VaultWorkFolder = $@"C:\daten\users\{System.Environment.UserName}\Vault-Root";


            foreach (Produto prod in codNorma.Produtos)
            {
                prod.NewFileName = Path.Combine(VaultWorkFolder, prod.FileNameSimplificado.Substring(2));
                GerarRelatorio("relatorioProdutos.txt", prod);

            }
        }
        public void GerarRelatorio(string nome_relatorio, Produto produto)
        {
            string[] names = produto.NewFileName.Split('\\');
            string linha = $"{produto.Type.ToString()};{names[names.Length - 2]};{names.Last()};{produto.NewFileName};{produto.node.Text}";

            string logPath = @"C:\KeepSoftwares\Bosch\Log\" + nome_relatorio;


            if (!Directory.Exists(Path.GetDirectoryName(logPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(logPath));

            File.AppendAllText(logPath, $"{linha}{System.Environment.NewLine}");
        }
        public void ExecutarComandoCheckIn()
        {
            try
            {
                Parametros._invApp.SilentOperation = false;
                CommandManager commandManager = Parametros._invApp.CommandManager;
                commandManager.ControlDefinitions["VaultCheckinTop"].Execute();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao executar o comando: " + ex.ToString());
            }
        }
        public void ListarComandos()
        {
            try
            {

                CommandManager commandManager = Parametros._invApp.CommandManager;

                foreach (ControlDefinition ctrlDef in commandManager.ControlDefinitions)
                {
                    File.AppendAllText(@"C:\KeepSoftwares\Bosch\Log\arquivos.txt", $"Command Name: {ctrlDef.DisplayName}, ID: {ctrlDef.InternalName}{System.Environment.NewLine}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao listar comandos: " + ex.Message);
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
            return;
            if (Parametros._invApp == null)
            {
                MessageBox.Show("NULO");
            }
            else
            {
                //ListarComandos();
                ExecutarComandoCheckIn();
                //int x = Parametros._invApp.Documents.Count;
                //string texto = $"{x.ToString()}{System.Environment.NewLine}";
                //if (x > 0)
                //{
                //    foreach (Document doc in Parametros._invApp.Documents)
                //    {
                //        texto += doc.FullDocumentName + System.Environment.NewLine;
                //    }

                //}
                //MessageBox.Show(texto);
            }
        }
    }


    public static class TableFormat
    {
        public static string GetDisplayName(Produto prod)
        {
            if (prod.Type == ProductType.Desconhecido)
            {
                return $"{Path.GetFileName(prod.NewFileName)} - {prod.Filename}";
            }

            return Path.GetFileName(prod.NewFileName);
        }
        public static Color GetRowColor(Produto prod)
        {
            if (prod.isMissing)
            {
                return Color.Red;
            }

            if (prod.Type == ProductType.Desconhecido)
            {
                return Color.MediumVioletRed;
            }

            if (prod.Type == ProductType.ATMOLIB_Library)
            {
                // AtmoLib Filename: FABRICANTE - CATALOGCODE
                // AtmoLib FolderPath: FABRICANTE
                string FilenamePath = Path.GetFileNameWithoutExtension(prod.NewFileName);
                string FolderPath = prod.node?.Parent?.Text;

                if (!FilenamePath.StartsWith(FolderPath))
                {
                    return Color.BlueViolet;
                }
            }
            return Color.Black;
        }
    }
}



//private void ListarObjetos(Norma norma, string root)
//{
//    LV_NORMA.View = System.Windows.Forms.View.Details;
//    LV_NORMA.FullRowSelect = true;
//    LV_NORMA.GridLines = true;
//    LV_NORMA.Columns.Add("Nome", 150, HorizontalAlignment.Left);
//    LV_ATMO.View = System.Windows.Forms.View.Details;
//    LV_ATMO.FullRowSelect = true;
//    LV_ATMO.GridLines = true;
//    LV_ATMO.Columns.Add("Nome", 150, HorizontalAlignment.Left);
//    LV_CC.View = System.Windows.Forms.View.Details;
//    LV_CC.FullRowSelect = true;
//    LV_CC.GridLines = true;
//    LV_CC.Columns.Add("Nome", 150, HorizontalAlignment.Left);
//    LV_DESCONHECIDO.View = System.Windows.Forms.View.Details;
//    LV_DESCONHECIDO.FullRowSelect = true;
//    LV_DESCONHECIDO.GridLines = true;
//    LV_DESCONHECIDO.Columns.Add("Nome", 150, HorizontalAlignment.Left);
//    foreach (Produto prod in norma.Produtos)
//    {
//        FileInfo file = new FileInfo(prod.Filename);
//        switch (prod.Type)
//        {
//            case ProductType.Norma:
//                LV_NORMA.Items.Add(Path.Combine(file.Directory.Name, Path.GetFileNameWithoutExtension(prod.Filename)));
//                break;
//            case ProductType.ATMOLIB_ProdutosBosch:
//                LV_ATMO.Items.Add(Path.Combine(file.Directory.Name, Path.GetFileNameWithoutExtension(prod.Filename)));
//                break;
//            case ProductType.ContentCenter:
//                LV_CC.Items.Add(Path.Combine(file.Directory.Name, Path.GetFileNameWithoutExtension(prod.Filename)));
//                break;
//            case ProductType.Desconhecido:
//                LV_DESCONHECIDO.Items.Add(Path.Combine(file.Directory.Name, Path.GetFileNameWithoutExtension(prod.Filename)));
//                break;
//        }
//    }
//}
