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
       
        private void ImportFiles_Load(object sender, EventArgs e)
        {
            this.Show();
            ToggleWaitCursor(true);

            if (!VaultHelper.ConectarWithUserAndPassword())
            {
                MessageBox.Show("Não foi possivel conectar no Autodesk Vault. Verifique a conexão com o servidor." +
                    "O software será encerrado", "ERRO DE CONEXÂO COM O VAULT", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                this.Close();
            }

            this.Text = "BOSCH - CONECTADO";
            ToggleWaitCursor(false);
        }
        private void ImportFiles_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Parametros._invApp != null)
            {
                Parametros._invApp.SilentOperation = false;
                Parametros._invApp.Quit();
            }
        }


        #region EVENTOS
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
            ExtractFilesFromZip(txtZipFileName.Text);
            CreateTextFileWithFileNames();

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

        private void tabelaItens_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                MessageBox.Show(tabelaItens.Rows[e.RowIndex].Cells[2].Value.ToString());
            }
        }
        private void tabelaItens_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (Parametros.isSystemChange)
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
            Parametros.isSystemChange = true;
            tabelaItens.Rows.Clear();
            if (e.Node.Text.Contains('.'))
            {
                Produto prod = e.Node.Tag as Produto;
                NewTableRow(prod);
            }
            DataTableUpdate(e.Node.Nodes);
            Parametros.isSystemChange = false;
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
        
        private void cbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFolderToMove.Items.Clear();
            txtFolderToMove.Text = string.Empty;

            List<string> ListasPastasDisponiveis = new List<string>();
            ListasPastasDisponiveis = VaultHelper.GetChildrenFoldersByFolderName(cbLocation.Text);

            Log.gravarLog(ListasPastasDisponiveis.Count().ToString());
            foreach (string item in ListasPastasDisponiveis)
            {
                Log.gravarLog(item);
                txtFolderToMove.Items.Add(item);
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

        #endregion


        private void CreateReportFilesFromProdutos()
        {
            foreach (Produto prod in codNorma.Produtos)
            {
                string filePath = config.Default.tempVaultRootPath + codNorma.CodigoNorma + ".txt";
                File.AppendAllText(filePath, prod.NewFileName + System.Environment.NewLine);
            }
        }
        private void ClearUI()
        {
            TreeBosch.Nodes.Clear();
            tabelaItens.Rows.Clear();
        }
        private void ToggleWaitCursor(bool isWaitCursor)
        {
            this.UseWaitCursor = isWaitCursor;
        }
        private void ExtractFilesFromZip(string zipFileName)
        {
            ZipfileManipulate zipfileManipulate = new ZipfileManipulate(tabelaItens);
            codNorma = zipfileManipulate.ExtractZip(zipFileName);
        }
        private void SetTemporaryPaths()
        {
            Properties.Settings.Default.tempVaultRootPath = Path.Combine(Parametros.tempLocation, Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            Parametros.ipjPadrao = Path.Combine(config.Default.tempVaultRootPath, Parametros.ipjFileName);
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
            Parametros._invApp.Visible = false;
            Parametros._invApp.SilentOperation = true;

            //AssemblyDocument asmDoc = null;
            if (Parametros._invApp.Documents.Count == 0)
            {
                // Abrir um novo projeto
                Parametros._invApp.DesignProjectManager.DesignProjects.AddExisting(Parametros.ipjPadrao).Activate();
                codNorma.oAsmDoc = Parametros._invApp.Documents.Open(mainAssemblyPath, false) as AssemblyDocument;
                checkLinkRecursive(codNorma.oAsmDoc);
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
                else if (descriptor.FullDocumentName.EndsWith(".iam"))
                {
                    checkLinkRecursive(Parametros._invApp.Documents.Open(descriptor.FullDocumentName, false) as AssemblyDocument);
                }
            }
        }

        private string GetDisplayName(Produto prod)
        {
            if (prod.Type == ProductType.Desconhecido)
            {
                return $"{Path.GetFileName(prod.NewFileName)} - {prod.Filename}";
            }

            return Path.GetFileName(prod.NewFileName);
        }
        public Color GetRowColor(Produto prod)
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
        private int AddRowToTabelaItens(Produto prod)
        {
            string displayName = GetDisplayName(prod);

            return tabelaItens.Rows.Add(
                prod.Type.ToString(),
                prod.isVaultExisting,
                displayName,
                " BOTAO EXTRA ",
                prod.NewFileName
            );
        }
        public void NewTableRow(Produto prod)
        {
            int rowIndex = AddRowToTabelaItens(prod);
            tabelaItens.Rows[rowIndex].DefaultCellStyle.ForeColor = GetRowColor(prod);

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

        private void newRowDesconhecido(Produto prod)
        {

            int rowIndex = TabelaDesconhecidos.Rows.Add(
                    false,
                    prod.isVaultExisting,
                    $"{Path.GetFileName(prod.NewFileName)}",
                    prod.NewFileName);
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
