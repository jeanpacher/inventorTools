using Inventor;
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
using System.IO.Compression;
using Autodesk.Connectivity.WebServices;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

using Path = System.IO.Path;
using File = System.IO.File;
using Color = System.Drawing.Color;
using config = Bosch_ImportData.Properties.Settings;
using VDF = Autodesk.DataManagement.Client.Framework;
using System.Threading;
using System.Diagnostics;


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

            ////Parametros._invApp = (Inventor.Application)Marshal.GetActiveObject("Inventor.Application");

            //foreach (Document doc in Parametros._invApp.Documents)
            //{
            //    doc.Close(true);
            //}

            ////if (Parametros._invApp.Documents.VisibleDocuments.Count>0)
            //if (Parametros._invApp.Documents.Count > 0)
            //{
            //    MessageBox.Show("Todos os arquivos do Inventor precisam estar fechados. Por favor, feche todos os arquivos e tente novamente",
            //        "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.Close();
            //}

            ToggleWaitCursor(true);

            if (!VaultHelper.ConectarWithUserAndPassword())
            {
                MessageBox.Show("Não foi possivel conectar no Autodesk Vault. Verifique a conexão com o servidor." +
                    "O software será encerrado", "ERRO DE CONEXÂO COM O VAULT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //this.Close();
            }

            this.Text = "BOSCH - CONECTADO";
            ToggleWaitCursor(false);

        }
        public void teste()
        {
            Parametros._invApp = (Inventor.Application)Marshal.GetActiveObject("Inventor.Application");
            Document oasm = Parametros._invApp.ActiveEditDocument;


            int x = oasm.ReferencedDocumentDescriptors.Count;
            int y = oasm.AllReferencedDocuments.Count;
            int z = oasm.ReferencedDocuments.Count;
            int w = oasm.ReferencedFileDescriptors.Count;
            int h = oasm.ReferencedFiles.Count;

            Document odoc = Parametros._invApp.ActiveDocument;
            ReferencedFileDescriptors file = odoc.ReferencedFileDescriptors;
            DocumentDescriptorsEnumerator docdesc = odoc.ReferencedDocumentDescriptors;

            return;
        }

        private void ImportFiles_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Parametros._invApp != null)
            {
                Parametros._invApp.SilentOperation = false;
                //Parametros._invApp.Quit();
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
            if (Parametros.isSystemChange)
                return;

            return;

            if (e.ColumnIndex == 3) // Supondo que a coluna de índice 1 seja a que contém o nome do arquivo
            {
                string oldFileName = (string)tabelaItens.Rows[e.RowIndex].Cells["originalNameColumn"].Value;
                string newFileName = (string)tabelaItens.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (!IsValidFileName(newFileName))
                {
                    MessageBox.Show("Nome de arquivo inválido.");
                    return;
                }
                try
                {
                    string newFilePath = Path.Combine(Path.GetDirectoryName(oldFileName), newFileName);

                    // Atualizar os links da montagem principal
                    //UpdateAssemblyLinks(codNorma.oAsmDoc, oldFileName, newFilePath);

                    // Atualizar o nome do arquivo na tabela
                    tabelaItens.Rows[e.RowIndex].Cells["originalNameColumn"].Value = newFilePath;

                    int i = codNorma.Produtos.FindIndex(x => x.NewFileName == oldFileName);
                    atualizarProduto(codNorma.Produtos[i], newFilePath);
                    codNorma.oAsmDoc.Update2(true);
                    codNorma.oAsmDoc.Save2(true);
                    MessageBox.Show("Arquivo renomeado com sucesso, links atualizados.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao renomear o arquivo: {ex.Message}");
                }
            }
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
            CriarAtualizarTabelaItens(e.Node.Nodes);
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
                ListasPastasDisponiveis.AddRange(VaultHelper.GetChildrenFoldersByFolderName(cbLocation.Text));
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
                }
                string oldFileName = prod.NewFileName;
                string newFileName = Microsoft.VisualBasic.Interaction.InputBox("Digite o novo nome do arquivo:" + Path.GetFileName(prod.NewFileName),
                                                                          "Renomear Arquivo");

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

        #endregion

        private void ToggleWaitCursor(bool isWaitCursor)
        {
            this.UseWaitCursor = isWaitCursor;
        }
        private void ClearUI()
        {
            TreeBosch.Nodes.Clear();
            tabelaItens.Rows.Clear();
            TabelaDesconhecidos.Rows.Clear();
            cbLocation.Text = string.Empty;
            txtFolderToMove.Text = string.Empty;



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

            string mainAssemblyPath = Directory.GetFiles(config.Default.tempVaultRootPath, codNorma.CodigoNorma + ".iam", SearchOption.AllDirectories).FirstOrDefault();
            string mainDrawingPath = Directory.GetFiles(config.Default.tempVaultRootPath, codNorma.CodigoNorma + ".idw", SearchOption.AllDirectories).FirstOrDefault();
            if (mainAssemblyPath == null)
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

            Parametros._invApp = (Inventor.Application)Activator.CreateInstance(Type.GetTypeFromProgID("Inventor.Application"));
            //Parametros._invApp = (Inventor.Application)Marshal.GetActiveObject("Inventor.Application");
            Parametros._invApp.Visible = true;
            Parametros._invApp.SilentOperation = true;

            //AssemblyDocument asmDoc = null;
            if (Parametros._invApp.Documents.Count == 0)
            {
                // Abrir um novo projeto
                Parametros._invApp.DesignProjectManager.DesignProjects.AddExisting(Parametros.ipjPadrao).Activate();
                codNorma.oAsmDoc = Parametros._invApp.Documents.Open(mainAssemblyPath, true) as AssemblyDocument;
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
                        prod.propriedades = new Propriedades(doc);

                    }

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
                            }
                        }
                    }
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
        private void atualizarProduto(Produto prod, string newFileName)
        {
            prod.NewFileName = newFileName;
            prod.FileNameSimplificado = newFileName.Replace(config.Default.tempVaultRootPath, @"$\");
            prod.node.Text = Path.GetFileName(prod.NewFileName);
        }
        private void OcultarTabDesconhecidos()
        {
            var desconhecidos = codNorma.Produtos.Where(x => x.Type == ProductType.Desconhecido).ToList();

            if (desconhecidos.Count == 0)
            {
                tabControl1.TabPages.Remove(tabDesconhecidos);
            }
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

        public void CriarAtualizarTabelaItens(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                Produto prod = node.Tag as Produto;
                if (prod == null || !node.Text.Contains('.'))
                {
                    CriarAtualizarTabelaItens(node.Nodes);
                }
                else
                {
                    AdicionarLinhaTabelaItens(prod);

                    if (node.Nodes.Count > 0)
                    {
                        CriarAtualizarTabelaItens(node.Nodes);
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
               prod.NewFileName);

            tabelaItens.Rows[rowIndex].Tag = prod;
            tabelaItens.Rows[rowIndex].DefaultCellStyle.ForeColor = TableFormat.GetRowColor(prod);
            if (prod.isVaultExisting)
                tabelaItens.Rows[rowIndex].ReadOnly = true;
        }
        private void newRowDesconhecido(Produto prod)
        {

            int rowIndex = TabelaDesconhecidos.Rows.Add(
                    false,
                    $"{Path.GetFileName(prod.NewFileName)}",
                    prod.Filename);

            TabelaDesconhecidos.Rows[rowIndex].Tag = prod;
        }
        private void CriarTabelaDesconhecida()
        {
            if (codNorma.Produtos.Any(x => x.Type == ProductType.Desconhecido))
            {
                foreach (Produto prod in codNorma.Produtos)
                {
                    if (prod.Type == ProductType.Desconhecido)
                    {
                        newRowDesconhecido(prod);
                    }
                }
            }
            else
            {
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
                }
            }
        }

        public bool MoverArquivo()
        {

            if (string.IsNullOrEmpty(txtFolderToMove.Text))
                return false;

            if (string.IsNullOrEmpty(cbLocation.Text))
                return false;

            ProductType productType;

            //Definindo o tipo
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
                    string substringName = prod.FileNameSimplificado.Substring(2);
                    prod.NewFileName = Path.Combine(config.Default.tempVaultRootPath, substringName);

                    treeBosch.NodeCreate(prod, Parametros.DicionarioNodes);
                    listaRows.Add(row);

                    if (!Directory.Exists(Path.GetDirectoryName(prod.NewFileName)))
                        Directory.CreateDirectory(Path.GetDirectoryName(prod.NewFileName));

                    File.Move(prod.OldFileName, prod.NewFileName);
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
                    CriarAtualizarTabelaItens(TreeBosch.Nodes);
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
        private void UncheckAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in TabelaDesconhecidos.SelectedRows)
            {
                row.Cells[0].Value = false;
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
