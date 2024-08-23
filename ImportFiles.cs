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


namespace Bosch_ImportData
{
    public partial class ImportFiles : Form
    {
        public Inventor.Application _invApp = null;
        public Norma codNorma;
        public string ipjPadrao;
        public bool isResolved = true;
        public bool isSystemChange = true;


        public string ipjFileName = "StandardIPJ.ipj";
        public string tempLocation = @"C:\Temp1\VaultWorkFolderBosch\";
        public VaultInterface vault;
        public HashSet<string> pastas = new HashSet<string>();
        public List<string> pastasCatalogs = new List<string>();

        public ImportFiles()
        {
            InitializeComponent();
        }
        public ImportFiles(Inventor.Application invApp)
        {
            //_invApp = invApp;
        }
        
        private void ImportFiles_Load(object sender, EventArgs e)
        {
            this.Show();
            this.UseWaitCursor = true;

            // CRIAÇÃO DOS OBJETOS
            vault = new VaultInterface();
            codNorma = new Norma();


            // CONEXÃO COM O VAULT
            if (!vault.conectarJean())
                this.Text = "BOSCH - NÃO CONECTADO";
            else
                this.Text = "BOSCH - CONECTADO";

            this.UseWaitCursor = false;
        }


        #region EVENTOS
        private void ImportFiles_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_invApp != null)
            {
                _invApp.SilentOperation = false;
                _invApp.Quit();
            }
        }
        private void btnSearchZip_Click(object sender, EventArgs e)
        {
            ListaLV.Items.Clear();

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
            ExtractFilesFromZip(txtZipFileName.Text);
            CreateTextFileWithFilenames();

            if (VerifyLinks())
            {
                MessageBox.Show("TODOS OS ARQUIVOS NECESSÁRIOS FORAM ENCONTRADOS");
            }
            else
            {
                MessageBox.Show("EXISTEM VINCULADOS DE ARQUIVOS FALTANDO NA MONTAGEM PRINCIPAL.");
            }

            PopulateTreeView();
            ToggleWaitCursor(false);
        }
        private void CreateTextFileWithFilenames()
        {
            foreach (Produto prod in codNorma.Produtos)
            {
                string filePath = config.Default.tempVaultRootPath + codNorma.CodigoNorma + ".txt";
                File.AppendAllText(filePath, prod.NewFileName + System.Environment.NewLine);
            }
        }
        private void ToggleWaitCursor(bool isWaitCursor)
        {
            this.UseWaitCursor = isWaitCursor;
        }
        private void ClearUI()
        {
            TreeBosch.Nodes.Clear();
            tabelaItens.Rows.Clear();
        }
        private void ExtractFilesFromZip(string zipFileName)
        {
            ZipfileManipulate zipfileManipulate = new ZipfileManipulate(tabelaItens);
            codNorma = zipfileManipulate.ExtractZip(zipFileName);
        }
        private void SetTemporaryPaths()
        {
            Properties.Settings.Default.tempVaultRootPath = Path.Combine(tempLocation, Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            ipjPadrao = Path.Combine(config.Default.tempVaultRootPath, ipjFileName);
        }
        private void PopulateTreeView()
        {
            // CRIAR A TREEVIEW
            TreeViewBosch tb = new TreeViewBosch();
            tb.TreeCreate(TreeBosch, codNorma);
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

        private void tabelaItens_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                MessageBox.Show(tabelaItens.Rows[e.RowIndex].Cells[2].Value.ToString());
            }
        }
        private void tabelaItens_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isSystemChange)
                return;
            if (e.ColumnIndex == 2) // Supondo que a coluna de índice 1 seja a que contém o nome do arquivo
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
                    UpdateAssemblyLinks(codNorma.oAsmDoc, oldFileName, newFilePath);

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
            isSystemChange = true;
            tabelaItens.Rows.Clear();
            if (e.Node.Text.Contains('.'))
            {
                Produto prod = e.Node.Tag as Produto;
                NewTableRow(prod);
            }
            DataTableUpdate(e.Node.Nodes);
            isSystemChange = false;
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Text == "DESCONHECIDOS")
            {
                txtFolderToMove.Visible = true;
                btnMover.Visible = true;
                cbLocation.Visible = true;


                foreach (Produto prod in codNorma.Produtos)
                {
                    if (prod.Type == ProductType.Desconhecido)
                    {
                        newRowDesconhecido(prod);
                    }
                }

                // Aqui você pode adicionar qualquer lógica necessária, como carregar dados ou atualizar o conteúdo da aba
            }

            if (tabControl1.SelectedTab.Text == "TABELA")
            {
                txtFolderToMove.Visible = false;
                btnMover.Visible = false;
                cbLocation.Visible = false;
            }



        }

        #endregion

        private bool VerifyLinks()
        {

            string mainAssemblyPath = Directory.GetFiles(config.Default.tempVaultRootPath, codNorma.CodigoNorma + ".iam", SearchOption.AllDirectories).FirstOrDefault();
            string mainDrawingPath = Directory.GetFiles(config.Default.tempVaultRootPath, codNorma.CodigoNorma + ".idw", SearchOption.AllDirectories).FirstOrDefault();
            if (mainAssemblyPath == null)
            {
                MessageBox.Show("Montagem não encontrada");
                isResolved = false;
                return isResolved;
            }


            if (!File.Exists(config.Default.DefaultIPJ))
            {
                MessageBox.Show("Arquivo IPJ não encontrado.\nVerifique se existe o arquivo: " + config.Default.DefaultIPJ);
                isResolved = false;
                return isResolved;
            }

            FileInfo ipjFile = new FileInfo(config.Default.DefaultIPJ);
            ipjFile.CopyTo(ipjPadrao, true);


            isResolved = true;

            _invApp = (Inventor.Application)Activator.CreateInstance(Type.GetTypeFromProgID("Inventor.Application"));
            _invApp.Visible = false;
            _invApp.SilentOperation = true;

            //AssemblyDocument asmDoc = null;
            if (_invApp.Documents.Count == 0)
            {
                // Abrir um novo projeto
                _invApp.DesignProjectManager.DesignProjects.AddExisting(ipjPadrao).Activate();
                codNorma.oAsmDoc = _invApp.Documents.Open(mainAssemblyPath, false) as AssemblyDocument;
                checkLinkRecursive(codNorma.oAsmDoc);
            }
            _invApp.SilentOperation = false;
            return isResolved;
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
                    isResolved = false;
                }
                else if (descriptor.FullDocumentName.EndsWith(".iam"))
                {
                    checkLinkRecursive(_invApp.Documents.Open(descriptor.FullDocumentName, false) as AssemblyDocument);
                }
            }
        }


        private void subMenu_Opening(object sender, CancelEventArgs e)
        {
            // Obter o controle que acionou o ContextMenuStrip
            var sourceControl = subMenu.SourceControl as ListView;
            if (sourceControl != null && sourceControl.SelectedItems.Count == 0)
            {
                // Cancelar a abertura do menu se nenhum item estiver selecionado
                e.Cancel = true;
                return;
            }
            moverParaNormaToolStripMenuItem.Enabled = true;
            moverParaCCToolStripMenuItem.Enabled = true;
            moverParaATMOLIBToolStripMenuItem.Enabled = true;
        }
        public void DataTableUpdate(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                Produto prod = node.Tag as Produto;
                if (prod == null || !node.Text.Contains('.'))
                {
                    DataTableUpdate(node.Nodes);
                }
                else
                {
                    NewTableRow(prod);

                    if (node.Nodes.Count > 0)
                    {
                        DataTableUpdate(node.Nodes);
                    }
                }
            }
        }
        private void UpdateAssemblyLinks(AssemblyDocument assembly, string oldFileName, string newFileName)
        {
            try
            {
                foreach (ComponentOccurrence occ in assembly.ComponentDefinition.Occurrences)
                {
                    if (occ.ReferencedFileDescriptor.FullFileName == oldFileName)
                    {
                        Document odoc = occ.Definition.Document;
                        if (odoc != null)
                        {
                            odoc.SaveAs(newFileName, false);
                            odoc.PropertySets["Design Tracking Properties"]["Part Number"].Value = Path.GetFileNameWithoutExtension(newFileName);
                            odoc.Update2(true);
                            odoc.Save();
                        }
                    }
                    if (occ.DefinitionDocumentType == DocumentTypeEnum.kAssemblyDocumentObject)
                    {
                        Document document = occ.Definition.Document;
                        UpdateAssemblyLinks(document as AssemblyDocument, oldFileName, newFileName);
                        document.Save2(true);
                        document.Update2(true);
                    }
                }
                // Salvar as alterações na montagem
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar links da montagem: {ex.Message}");
            }
        }
        private void atualizarProduto(Produto prod, string newFileName)
        {
            prod.NewFileName = newFileName;
            prod.FileNameSimplificado = newFileName.Replace(config.Default.tempVaultRootPath, @"$\");
            prod.node.Text = Path.GetFileName(prod.NewFileName);
        }

        public Color CorEsquema(Produto produto)
        {
            return Color.Red;
        }


        public void NewTableRow(Produto prod)
        {
            int rowIndex;

            if (prod.isMissing)
            {
                rowIndex = tabelaItens.Rows.Add(prod.Type.ToString(),
                    prod.isVaultExisting,
                    $"{Path.GetFileName(prod.NewFileName)}",
                    "SUBSTITUIR",
                    prod.NewFileName);

                tabelaItens.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
            }

            else if (prod.Type == ProductType.Desconhecido)
            {
                rowIndex = tabelaItens.Rows.Add(prod.Type.ToString(),
                    prod.isVaultExisting,
                    $"{Path.GetFileName(prod.NewFileName)} - {prod.Filename}",
                    "SUBSTITUIR",
                    prod.NewFileName);

                tabelaItens.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.MediumVioletRed;
            }

            else if (prod.Type == ProductType.ATMOLIB_Library)
            {
                string nome = Path.GetFileNameWithoutExtension(prod.NewFileName);
                string pastaPai = prod.node.Parent.Text;

                rowIndex = tabelaItens.Rows.Add(prod.Type.ToString(),
                    prod.isVaultExisting,
                    Path.GetFileName(prod.NewFileName),
                    "SUBSTITUIR",
                    prod.NewFileName);

                if (!nome.StartsWith(pastaPai))
                    tabelaItens.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.BlueViolet;
            }

            else
            {
                rowIndex = tabelaItens.Rows.Add(prod.Type.ToString(),
                    prod.isVaultExisting,
                    Path.GetFileName(prod.NewFileName),
                    "SUBSTITUIR",
                    prod.NewFileName);

                tabelaItens.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;
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
                        pastas.Add(nodeFilho.Text);
                    }
                    return;
                }

                if (node.Nodes != null)
                {
                    PesquisarTreeNodes(node);
                }
            }
        }

        private void newRowDesconhecido(Produto prod)
        {

            int rowIndex = TabelaDesconhecidos.Rows.Add(
                    false,
                    prod.isVaultExisting,
                    $"{Path.GetFileName(prod.NewFileName)}",
                    prod.NewFileName);
        }



        private void cbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> ListasPastasDisponiveis = new List<string>();
            txtFolderToMove.Items.Clear();
            txtFolderToMove.Text = string.Empty;

            Autodesk.Connectivity.WebServices.Folder pasta = vault.FindFolderByName(cbLocation.Text);
            ListasPastasDisponiveis = vault.GetFolderChildrenByFolder(pasta);

            Log.gravarLog(ListasPastasDisponiveis.Count().ToString());
            foreach (string item in ListasPastasDisponiveis)
            {
                Log.gravarLog(item);
                txtFolderToMove.Items.Add(item);
            }

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
