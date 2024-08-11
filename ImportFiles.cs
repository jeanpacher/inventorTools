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
using Path = System.IO.Path;
using Autodesk.Connectivity.WebServices;
using System.Net.NetworkInformation;
using File = System.IO.File;
using Autodesk.DataManagement.Client.Framework.Forms.Currency;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using config = Bosch_ImportData.Properties.Settings;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace Bosch_ImportData
{
    public partial class ImportFiles : Form
    {
        public Inventor.Application _invApp = null;
        // public string tempRootPath { get; set; } = config.Default.tempVaultRootPath;
        public Norma codNorma;
        public string ipjPadrao;
        public bool isResolved = true;
        public bool isSystemChange = true;
        public ImportFiles()
        {
            InitializeComponent();
        }
        private void ImportFiles_Load(object sender, EventArgs e)
        {
            codNorma = new Norma();
        }
        private void ImportFiles_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_invApp != null)
            {
                _invApp.SilentOperation = false;
                _invApp.Quit();
            }
        }
        #region BUTTONS CLIQUES
        private void btnSearchZip_Click(object sender, EventArgs e)
        {
            ListaLV.Items.Clear();
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Arquivos ZIP (*.zip)|*.zip";
                openFileDialog.Title = "Selecione o projeto";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtZipFileName.Text = openFileDialog.FileName;
                }
            }
        }
        private void btnExtrair_Click(object sender, EventArgs e)
        {
            TreeBosch.Nodes.Clear();
            tabelaItens.Rows.Clear();
            codNorma = new Norma();
            Properties.Settings.Default.tempVaultRootPath = Path.Combine(@"C:\Temp1\VaultWorkFolderBosch\", Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            ipjPadrao = Path.Combine(config.Default.tempVaultRootPath, "StandardIPJ.ipj");
            // Extrair os arquivos do zip no mesmo diretorio
            ZipfileManipulate zipfileManipulate = new ZipfileManipulate();
            codNorma = zipfileManipulate.ExtractZip(txtZipFileName.Text);
            // CRIAR UM TXT COM OS ARQUIVOS
            foreach (Produto prod in codNorma.Produtos)
            {
                File.AppendAllText(config.Default.tempVaultRootPath + codNorma.CodigoNorma + ".txt", prod.NewFileName + System.Environment.NewLine);
            }
            if (VerifyLinks())
                MessageBox.Show("MONTAGEM OK");
            else
                MessageBox.Show("ARQUIVOS FALTANDO");
            // CRIAR A TREEVIEW
            TreeViewBosch tb = new TreeViewBosch();
            tb.TreeCreate(TreeBosch, codNorma);
        }
        private void BtnTeste_Click(object sender, EventArgs e)
        {
            OpenInventor();
        }
        private void btnCloseInventor_Click(object sender, EventArgs e)
        {
            _invApp.SilentOperation = false;
            _invApp.Quit();
        }
        #endregion
        #region TABLE INTERACTIONS
        private void tabelaItens_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                MessageBox.Show(tabelaItens.Rows[e.RowIndex].Cells[1].Value.ToString());
            }
        }
        private void tabelaItens_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isSystemChange)
                return;
            if (e.ColumnIndex == 1) // Supondo que a coluna de índice 1 seja a que contém o nome do arquivo
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
                    // Renomear o arquivo fisicamente
                    //File.Move(oldFileName, newFilePath);
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
        #endregion
        #region TREEVIEW INTERACTIONS
        private void TreeViewNorma_AfterSelect(object sender, TreeViewEventArgs e)
        {
            isSystemChange = true;
            tabelaItens.Rows.Clear();
            if (e.Node.Text.Contains('.'))
            {
                Produto prod = e.Node.Tag as Produto;
                if (prod.Type == ProductType.Desconhecido)
                    tabelaItens.Rows.Add(prod.Type.ToString(), $"{Path.GetFileName(prod.NewFileName)} - {prod.Filename}", "SUBSTITUIR", prod.NewFileName);
                else
                    tabelaItens.Rows.Add(prod.Type.ToString(), Path.GetFileName(prod.NewFileName), "SUBSTITUIR", prod.NewFileName);
            }
            listarProdutos(e.Node.Nodes);
            isSystemChange = false;
        }
        #endregion
        private bool VerifyLinks()
        {
            FileInfo ipjFile = new FileInfo(Path.Combine(System.Windows.Forms.Application.StartupPath, "StandardIPJ.ipj"));
            //string projectFileName = Path.Combine(config.Default.tempVaultRootPath, "StandardIPJ.ipj");
            ipjFile.CopyTo(ipjPadrao, true);
            string mainAssemblyPath = Directory.GetFiles(config.Default.tempVaultRootPath, codNorma.CodigoNorma + ".iam", SearchOption.AllDirectories).FirstOrDefault();
            string mainDrawingPath = Directory.GetFiles(config.Default.tempVaultRootPath, codNorma.CodigoNorma + ".idw", SearchOption.AllDirectories).FirstOrDefault();
            if (mainAssemblyPath == null)
            {
                MessageBox.Show("Montagem não encontrada");
                return false;
            }
            isResolved = true;
            _invApp = (Inventor.Application)Activator.CreateInstance(Type.GetTypeFromProgID("Inventor.Application"));
            _invApp.Visible = true;
            _invApp.SilentOperation = true;
            //AssemblyDocument asmDoc = null;
            if (_invApp.Documents.Count == 0)
            {
                // Abrir um novo projeto
                _invApp.DesignProjectManager.DesignProjects.AddExisting(ipjPadrao).Activate();
                codNorma.oAsmDoc = _invApp.Documents.Open(mainAssemblyPath, true) as AssemblyDocument;
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
        private void OpenInventor()
        {
            if (InvApp.inventorApp != null)
            {
                // Faz algo com a instância do Inventor (por exemplo, abrir um documento)                
                InvApp.inventorApp.Visible = true;
                InvApp.inventorApp.Documents.Open(@"C:\Users\Jean\Desktop\Teste\4729111862\4729111862\Workspaces\Arbeitsbereich\CtP_TEF\project\4729111862\4729111862_P002.ipt");
                // Fecha a instância do Inventor quando terminar
                System.Threading.Thread.Sleep(100000);
                InvApp.inventorApp.Quit();
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
        public void listarProdutos(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                Produto prod = node.Tag as Produto;
                if (prod == null || !node.Text.Contains('.'))
                {
                    listarProdutos(node.Nodes);
                }
                else
                {
                    if (prod.Type == ProductType.Desconhecido)
                        tabelaItens.Rows.Add(prod.Type.ToString(), $"{Path.GetFileName(prod.NewFileName)} - {prod.Filename}", "SUBSTITUIR", prod.NewFileName);
                    else
                        tabelaItens.Rows.Add(prod.Type.ToString(), Path.GetFileName(prod.NewFileName), "SUBSTITUIR", prod.NewFileName);
                    if (node.Nodes.Count > 0)
                    {
                        listarProdutos(node.Nodes);
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
