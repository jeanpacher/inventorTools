using Autodesk.Connectivity.WebServices;
using Bosch_ImportData.Properties;
using Inventor;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZstdSharp.Unsafe;
using Color = System.Drawing.Color;
using config = Bosch_ImportData.Properties.Settings;
using File = System.IO.File;
using Path = System.IO.Path;
using VDF = Autodesk.DataManagement.Client.Framework;

namespace Bosch_ImportData
{
    public partial class ImportFiles : Form
    {
        public bool ArquivosTemporariosCriados = false;
        public Norma codNorma { get; set; }
        public SplashScreen splashScreen;
        public ImportFiles()
        {
            InitializeComponent();
        }
        public ImportFiles(Inventor.Application invApp)
        {
            InitializeComponent();
            //_invApp = invApp;
        }

        private void StartOperations()
        {
            SplashScreen splash = new SplashScreen();
            splash.Show();


            Task.Run(() =>
            {
                for (int i = 0; i <= 100; i += 20)
                {
                    Thread.Sleep(500); // Simulando uma operação longa
                    this.Invoke(new Action(() =>
                    {
                        splash.UpdateProgress(i, $"Operação {i}% completa");
                    }));
                }

                this.Invoke(new Action(() => splash.Close()));
            });
        }
        #region EVENTOS
        private void ImportFiles_Load(object sender, EventArgs e)
        {
            this.Show();
            ArquivosTemporariosCriados = false;

            Root.Deserialize();
            splashScreen = new SplashScreen();

            ValidateVaultAndInventorInstances();
            this.Text = "BOSCH - CONECTADO";
            TableFormat.IconesInventor = InventorIconImageList;
            TableFormat.Tabela = tabelaItens;


        }

        public void ApagarTemporarios()
        {
            try
            {

                Directory.Delete(Properties.Settings.Default.tempVaultRootPath, true);
            }
            catch (Exception e1)
            {

                Log.GravarLog("Erro ao excuir arquivos temporarios");
            }
        }
        private void ImportFiles_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ArquivosTemporariosCriados)
            {

                DialogResult dialog = MessageBox.Show("Deseja deletar os arquivos temporários?", "Close App", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialog == DialogResult.Yes)
                {
                    CloseAllDocuments();

                    if (Parametros._invApp.Documents.Count == 0)
                    {
                        // Abrir um novo projeto
                        Parametros._invApp.DesignProjectManager.DesignProjects.AddExisting(Path.Combine(Parametros.VaultWorkFolder, Parametros.VaultProject)).Activate();
                    }
                    ApagarTemporarios();

                }
            }


            try
            {
                if (Parametros._invApp != null)
                    Parametros._invApp.SilentOperation = false;

            }
            catch (Exception e1)
            {
                Log.GravarLog(e1.ToString() + System.Environment.NewLine);
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
                    btnExtrair.Enabled = true;
                }
            }
        }
        private async void btnExtrair_Click(object sender, EventArgs e)
        {
            //ToggleWaitCursor(true);
            ClearUI();

            Parametros.isSystemChange = true;

            codNorma = new Norma();         // Reinicializa codNorma para garantir que está limpo
            SetTemporaryPaths();
            splashScreen = new SplashScreen();
            splashScreen.Show();

            if (await ExtractFilesFromZip(txtZipFileName.Text) == false)
                return;

            ArquivosTemporariosCriados = true;

            await VerifyLinks();
            await PesquisarVault();
            await ReplaceFromVault();
            await LerCustomParameters();
            await TreeViewHelper.CreateTreeView(TreeBosch, codNorma.Produtos, splashScreen);

            foreach (TreeNode node in TreeBosch.Nodes)
            {
                node.Expand();
            }
            tabelaItens.Rows.Clear();

            CriarTabelaItens(TreeBosch.Nodes);


            splashScreen.Close();

            ToggleWaitCursor(false);
            this.Cursor = Cursors.Default;
            btnExtrair.Enabled = false;
            btnUpdate.Enabled = true;
            btnReleaseFiles.Enabled = true;
            IsProjectFixed();
            Parametros.isSystemChange = false;

        }

        public async Task ReplaceFromVault()
        {
            foreach (Produto2 prod in codNorma.Produtos)
            {
                if (prod.sourceFile == SourceFile.FromVault && prod.isMissing)
                {
                    await ReplaceFile(prod, prod.VaultFileName);
                    prod.NewFileName = prod.VaultFileName;
                    prod.isMissing = false;
                    prod.isVaultExisting = true;
                    prod.DefineType();

                }
            }

            return;
        }

        private void BtnMoveFiles_Click(object sender, EventArgs e)
        {
            Parametros.isSystemChange = true;
            MoverArquivo();
            IsProjectFixed();
            Parametros.isSystemChange = false;

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
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (codNorma == null) return;


            if (tabControl1.SelectedTab.Text == "TABELA")
            {
                TreeBosch.SelectedNode = TreeBosch.TopNode;
            }

            if (tabControl1.SelectedTab.Text == "ATMOLIB")
            {
                CriarTabelaPropriedades();
            }

            if (tabControl1.SelectedTab.Text == "RELATÓRIOS")
            {
                CriarTabelaRelatorios();
            }
        }
        private void TreeViewNorma_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (Parametros.isSystemChange) return;

            Parametros.isSystemChange = true;
            tabelaItens.Rows.Clear();
            if (e.Node.Text.Contains('.'))
            {
                Produto2 prod = e.Node.Tag as Produto2;
                AdicionarLinhaTabelaItens(prod);
            }
            CriarTabelaItens(e.Node.Nodes);
            Parametros.isSystemChange = false;
        }
        private void tabelaATMOLIB_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (Parametros.isSystemChange)
                return;

            Produto2 prod = (Produto2)tabelaATMOLIB.Rows[e.RowIndex].Tag;
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
                Log.GravarLog($"Erro ao gravar a propriedade do arquivo: {Path.GetFileName(prod.NewFileName)} {System.Environment.NewLine} ERRO: {e1.ToString()}");
            }
        }
        private async void TabelaItens_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex < 0) return;

            Produto2 prod = (Produto2)tabelaItens.Rows[e.RowIndex].Tag;
            if (tabelaItens.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && !prod.isVaultExisting)
            {
                if (e.ColumnIndex == tabelaItens.Columns["ColunaItem"].Index)
                {
                    bool state = (bool)tabelaItens.Rows[e.RowIndex].Cells["ColunaItem"].Value;
                    tabelaItens.Rows[e.RowIndex].Cells["ColunaItem"].Value = !state;
                    return;
                }
            }

            if (tabelaItens.Columns[e.ColumnIndex] is DataGridViewImageColumn)
            {
                if (e.ColumnIndex == tabelaItens.Columns["ColunaTipoArquivo"].Index)
                    return;

                if (e.ColumnIndex == tabelaItens.Columns["ColunaPreview"].Index)
                {
                    MostrarMiniatura(e);
                    return;
                }

                if (prod.isVaultExisting)
                {
                    MessageBox.Show("Esse arquivo não pode ser manipulado, pois já está cadastrado no Vault.");
                    return;
                }

                if (e.ColumnIndex == tabelaItens.Columns["ColunaRenomear"].Index)
                {
                    string newFileName = Microsoft.VisualBasic.Interaction.InputBox("Digite o novo nome do arquivo:" + Path.GetFileName(prod.NewFileName),
                                                                     "Renomear Arquivo", Path.GetFileNameWithoutExtension(prod.NewFileName));

                    if (string.IsNullOrWhiteSpace(newFileName) || newFileName == Path.GetFileNameWithoutExtension(prod.NewFileName))
                        return;

                    newFileName += Path.GetExtension(prod.NewFileName);

                    if (isNewFileNameExisting(prod, newFileName)) return;

                    newFileName = Path.Combine(Path.GetDirectoryName(prod.NewFileName), newFileName);

                    RenomearArquivo(prod, newFileName, e);

                }


                else if (e.ColumnIndex == tabelaItens.Columns["ColunaProcurar"].Index)
                {
                    if (!prod.isMissing)
                    {
                        MessageBox.Show("O Arquivo está referenciado corretamente na montagem");
                        return;
                    }
                    else
                    {
                        string FileSearched = SearchAndCopyFile(prod);

                        if (string.IsNullOrEmpty(FileSearched)) return;

                        if (!await ReplaceFile(prod, FileSearched)) return;

                        AtualizarUnresolvedFile(prod, FileSearched);

                        IsProjectFixed();

                    }
                }
                else if (e.ColumnIndex == tabelaItens.Columns["ColunaDeletar"].Index)
                {
                    MessageBox.Show("Deletar");

                }
                else if (e.ColumnIndex == tabelaItens.Columns["ColunaMover"].Index)
                {
                    MessageBox.Show("Mover");
                }
            }
        }
        private void tabelaItens_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            //MostrarMiniatura(e);
        }
        private void TabelaDesconhecidos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            return;
            MostrarMiniatura(e);

        }
        private void tabelaATMOLIB_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            return;
            MostrarMiniatura(e);

        }
        private void btnDeletar_Click(object sender, EventArgs e)
        {

            List<DataGridViewRow> listaRows = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in tabelaItens.Rows)
            {
                if (Convert.ToBoolean(row.Cells["ColunaCheck"].Value) == true && !Convert.ToBoolean(row.Cells["colunaInventor"].Value))
                {
                    Produto2 prod = (Produto2)row.Tag;

                    TreeBosch.Nodes.Remove(prod.node);
                    Parametros.DicionarioNodes.Remove(Path.GetFileName(prod.NewFileName));

                    codNorma.Produtos.Remove(prod);
                    listaRows.Add(row);
                }
            }
            listaRows.ForEach(tabelaItens.Rows.Remove);
            //IsProjectFixed();

        }
        private void MoverArquivosToVault()
        {
            Parametros._invApp.SilentOperation = true;
            //FecharArquivos();

            if (codNorma.Produtos.Any(x => x.isMissing))
            {
                MessageBox.Show("Existem arquivos faltando na montagem");
                return;
            }

            //else if (Parametros.DicionarioNodes.TryGetValue("$", out TreeNode nodePai))
            //{
            //  CriarEstruturaPastas(nodePai.Nodes);
            CopiarArquivosToVaultFolder();
            AbrirMontagemToCheckIn();

            //GerarRelatorioByNodes(TreeBosch.Nodes);
            //GerarRelatorioByProduto();

            MessageBox.Show("Finalizado");
            Parametros._invApp.SilentOperation = false;
            //}
        }
        #endregion

        #region OPERAÇÔES PRODUTO

        //RENOMEAR ARQUIVOS
        public bool isNewFileNameExisting(Produto2 prod, string newFileName)
        {

            Autodesk.Connectivity.WebServices.File[] files = VaultHelper.FindFileByName(newFileName);

            if (files != null)
            {
                DialogResult result = MessageBox.Show("Arquivo já existe no vault, deseja substituir?", "Vault File", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("Ainda não implementado");
                    return true;
                }
                else
                    return true;
            }

            return false;
        }
        public Dictionary<string, Document[]> ListarArquivosParaRenomear(Produto2 prod)
        {
            Parametros._invApp.SilentOperation = true;
            Document document = codNorma.oAsmDoc as Document;
            Dictionary<string, Document[]> dicionarioDocsToRename = new Dictionary<string, Document[]>();
            dicionarioDocsToRename.Clear();
            if (document.AllReferencedDocuments == null)
            {
                return null;
            }
            foreach (Document DocToRename in document.AllReferencedDocuments)
            {
                if (DocToRename.FullFileName == prod.OldFileName)
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
            return dicionarioDocsToRename;
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
        public bool RenomearArquivo(Produto2 prod, string newFileName, DataGridViewCellEventArgs e)
        {
            prod.OldFileName = prod.NewFileName;
            prod.NewFileName = newFileName;
            prod.FileNameSimplificado = prod.NewFileName.Replace(config.Default.tempVaultRootPath, @"$\");
            prod.node.Text = Path.GetFileName(prod.NewFileName);

            try
            {
                Dictionary<string, Document[]> dicionarioDocsToRename = new Dictionary<string, Document[]>();
                dicionarioDocsToRename = ListarArquivosParaRenomear(prod);

                if (dicionarioDocsToRename == null)
                {
                    MessageBox.Show("Não existem arquivos para serem renomeados");
                    return false;
                }

                RenomearArquivos(dicionarioDocsToRename, prod.NewFileName, prod.OldFileName);
                tabelaItens.Rows[e.RowIndex].Cells["ColunaFile"].Value = Path.GetFileNameWithoutExtension(prod.NewFileName);
                codNorma.oAsmDoc.Update2(true);
                codNorma.oAsmDoc.Save2(true);
                MessageBox.Show("Arquivo renomeado com sucesso, links atualizados.");
                Parametros._invApp.SilentOperation = false;
                return true;
            }
            catch (Exception e1)
            {
                MessageBox.Show("Erro ao renomear o arquivo \n" + e1.Message);
                return false;
            }
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
                    Log.GravarLog(e1.Message);
                }
            }
        }

        // PROCURAR ARQUIVO
        public string SearchAndCopyFile(Produto2 prod)
        {
            string extension = Path.GetExtension(prod.NewFileName);
            string NewFileOriginalFileName = string.Empty;
            string NewFileTempFileName = string.Empty;

            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = $"Arquivos {extension} (*{extension})|*{extension}";
                    openFileDialog.Title = "Selecione o arquivo";
                    openFileDialog.InitialDirectory = Parametros.VaultWorkFolder;
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        NewFileOriginalFileName = openFileDialog.FileName;
                        NewFileTempFileName = Path.Combine(Settings.Default.tempVaultRootPath, NewFileOriginalFileName.Substring(Parametros.VaultWorkFolder.Length));
                        MessageBox.Show(NewFileOriginalFileName + "\n\n" + NewFileTempFileName);

                        if (File.Exists(NewFileTempFileName))
                        {
                            MessageBox.Show("Arquivo já existe na pasta desse projeto");
                            return string.Empty;
                        }
                        else
                        {
                            if (!Directory.Exists(Path.GetDirectoryName(NewFileTempFileName)))
                                Directory.CreateDirectory(Path.GetDirectoryName(NewFileTempFileName));

                            File.Copy(NewFileOriginalFileName, NewFileTempFileName);
                            return NewFileTempFileName;
                        }
                    }
                    else
                        return string.Empty;
                }
            }
            catch (Exception e1)
            {

                MessageBox.Show("Erro ao copiar o arquivo: " + e1.Message);
                Log.GravarLog(e1.ToString(), LogName.ProductLog);
                return string.Empty;
            }
        }
        public async Task<bool> ReplaceFile(Produto2 prod, string newFilePath)
        {
            Parametros._invApp.SilentOperation = true;
            foreach (string parentFileName in prod.ParentAssembly)
            {
                if (!parentFileName.EndsWith(".iam"))
                    continue;
                try
                {
                    AssemblyDocument oAsm = Parametros._invApp.Documents.Open(parentFileName, true) as AssemblyDocument;
                    foreach (ComponentOccurrence occ in oAsm.ComponentDefinition.Occurrences)
                    {
                        if (occ.ReferencedDocumentDescriptor.ReferenceMissing == true)
                        {
                            if (Path.GetFileName(occ.ReferencedFileDescriptor.FullFileName) == Path.GetFileName(prod.NewFileName))
                            {
                                occ.Replace2(newFilePath, true);
                            }
                        }
                    }
                    oAsm.Save();
                    oAsm.Save2(true);

                    if (oAsm.FullFileName != codNorma.oAsmDoc.FullFileName)
                        oAsm.Close();

                    Parametros._invApp.SilentOperation = false;
                    return true;
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                    Parametros._invApp.SilentOperation = false;
                    Log.GravarLog(e1.Message);
                    return false;
                }
            }
            return false;
        }

        //APAGAR ARQUIVOS
        public void RemoverArquivo()
        {

            List<DataGridViewRow> listaRows = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in tabelaItens.Rows)
            {
                if (Convert.ToBoolean(row.Cells["ColunaItem"].Value) == true && !Convert.ToBoolean(row.Cells["isVault"].Value))
                {
                    Produto2 prod = (Produto2)row.Tag;

                    TreeBosch.Nodes.Remove(prod.node);
                    Parametros.DicionarioNodes.Remove(Path.GetFileName(prod.NewFileName));

                    codNorma.Produtos.Remove(prod);
                    listaRows.Add(row);
                }
            }
            listaRows.ForEach(tabelaItens.Rows.Remove);

            IsProjectFixed();
        }
        public void AtualizarUnresolvedFile(Produto2 prod, string NewFile)
        {

            prod.OldFileName = prod.NewFileName;
            prod.NewFileName = NewFile;
            prod.FileNameSimplificado = NewFile.Replace(config.Default.tempVaultRootPath, @"$\");
            prod.isMissing = false;
            prod.isAssemblyParticipant = true;


            prod.DefineType();

            if (prod.Categoria == ProductType.LIBRARY)
            {
                try
                {
                    Document doc = Parametros._invApp.Documents.Open(prod.NewFileName, false);
                    prod.propriedades = new Propriedades(doc);
                    doc.Save();
                    doc.Close();
                }
                catch (Exception e1)
                {

                    Log.GravarLog($"Erro ao preencher as propriedades: {prod.NewFileName} --- {e1.ToString()}");
                }

            }


            TreeViewHelper.ApagarNodeByPath(TreeBosch, prod);
            //TreeBosch.Nodes.Remove(prod.node);

            if (Parametros.DicionarioNodesMissing.ContainsKey(prod.OldFileName))
                Parametros.DicionarioNodesMissing.Remove(prod.OldFileName);

            prod.node = null;

            TreeViewHelper.CreateNodeByPath(TreeBosch, prod);

            // TreeViewHelper.NodeCreate(prod, Parametros.DicionarioNodes);

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
        private void ModificarAtualizarProduto(Produto2 prod, string newFileName)
        {
            prod.OldFileName = prod.NewFileName;
            prod.NewFileName = newFileName;
            prod.FileNameSimplificado = newFileName.Replace(config.Default.tempVaultRootPath, @"$\");
            prod.node.Text = Path.GetFileName(prod.NewFileName);
        }
        public bool MoverArquivo()
        {
            if (string.IsNullOrEmpty(txtFolderToMove.Text) || string.IsNullOrEmpty(cbLocation.Text))
            {
                MessageBox.Show("Por favor, selecione o local de destino para onde os arquivos serão movidos");
                return false;
            }

            //   if (TabelaDesconhecidos.Rows.GetRowCount())
            ProductType NewproductType;

            switch (cbLocation.Text.Split('\\').Last())
            {
                case "Catalog":
                    NewproductType = ProductType.LIBRARY;
                    break;

                case "Produtos Bosch":
                    NewproductType = ProductType.PRODUTOS_BOSCH;
                    break;

                case "project":
                    NewproductType = ProductType.NORMA;
                    break;

                default:
                    NewproductType = ProductType.CONTENTCENTER;
                    break;

            }


            List<DataGridViewRow> listaRows = new List<DataGridViewRow>();
            List<Produto2> listaProdutos = new List<Produto2>();

            foreach (DataGridViewRow row in tabelaItens.Rows)
            {
                if (Convert.ToBoolean(row.Cells["ColunaItem"].Value) == true)
                {
                    Produto2 prod = (Produto2)row.Tag;

                    TreeViewHelper.ApagarNodeByPath(TreeBosch, prod);

                    // TreeBosch.Nodes.Remove(prod.node);
                    //Parametros.DicionarioNodes.Remove(Path.GetFileName(prod.Filename));

                    prod.Categoria = NewproductType;
                    prod.OldFileName = prod.NewFileName;
                    prod.FileNameSimplificado = Path.Combine(cbLocation.Text, txtFolderToMove.Text, Path.GetFileName(prod.NewFileName));
                    prod.NewFileName = Path.Combine(config.Default.tempVaultRootPath, prod.FileNameSimplificado.Substring(2));

                    TreeViewHelper.CreateNodeByPath(TreeBosch, prod);

                    //TreeViewHelper.NodeCreate(prod, Parametros.DicionarioNodes);
                    listaRows.Add(row);
                    listaProdutos.Add(prod);

                    if (!Directory.Exists(Path.GetDirectoryName(prod.NewFileName)))
                        Directory.CreateDirectory(Path.GetDirectoryName(prod.NewFileName));

                    ProcurarArquivosToRename(prod.NewFileName, prod.OldFileName);
                    //if (prod.Categoria == ProductType.LIBRARY)
                    //{
                    //    Document doc = Parametros._invApp.Documents.Open(prod.NewFileName, false);
                    //    prod.Doc = doc;
                    //    prod.propriedades = new Propriedades(doc);
                    //}

                    codNorma.oAsmDoc.Update2(true);
                    codNorma.oAsmDoc.Save2(true);
                    //File.Move(prod.OldFileName, prod.Filename);

                }
            }

            if (codNorma.Produtos.Any(x => x.Categoria == ProductType.DESCONHECIDO))
            {
                listaRows.ForEach(tabelaItens.Rows.Remove);
                BtnCheckInAtmo.Enabled = true;
                TreeBosch.SelectedNode = TreeBosch.TopNode;
                return true;
            }
            else
                return false;

            //return;
            //IsProjectFixed();
        }
        private bool IsProjectFixed()
        {
            if (codNorma.Produtos.Any(x => x.isMissing)) return false;

            SetButtonsEnable();

            try
            {
                codNorma.oAsmDoc.ResolveFile();
                codNorma.oAsmDoc.Save2(true);
            }
            catch (Exception e1)
            {
                Log.GravarLog($"Erro ao tentar resolver e salvar o arquivo: {codNorma.oAsmDoc.FullDocumentName} -- {e1.Message}", LogName.ProductLog);
            }


            return true;
        }
        private void AtualizarReferenciaNaMontagem(Produto2 prod)
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
        public void FecharArquivos()
        {
            Parametros._invApp.Documents.CloseAll();
        }
        public bool CopiarArquivosToVaultFolder()
        {
            foreach (Produto2 prod in codNorma.Produtos)
            {
                prod.OldFileName = prod.NewFileName;
                prod.FileNameSimplificado = prod.FileNameSimplificado.Replace('/', '\\');
                prod.NewFileName = Path.Combine(Parametros.VaultWorkFolder, prod.FileNameSimplificado.Substring(2));
                try
                {
                    if (!Directory.Exists(Path.GetDirectoryName(prod.NewFileName)))
                        Directory.CreateDirectory(Path.GetDirectoryName(prod.NewFileName));

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

                if (Parametros.NormaAssembly != null)
                {
                    codNorma.oAsmDoc = Parametros._invApp.Documents.Open(Parametros.NormaAssembly.NewFileName, true) as AssemblyDocument;

                }
                if (Parametros.NormaDrawing != null)
                {
                    DrawingDocument oDrawDoc = Parametros._invApp.Documents.Open(Parametros.NormaDrawing.NewFileName, true) as DrawingDocument;

                }
                //Produto2 produto = codNorma.Produtos.Find(x => x.OldFileName == Parametros.mainAssemblyPath);
                //codNorma.oAsmDoc = Parametros._invApp.Documents.Open(produto.NewFileName, true) as AssemblyDocument;
                splashScreen.Close();
                ExecutarComandoCheckIn();
            }
        }
        #endregion

        #region VALIDAÇÔES
        public bool ValidateVaultAndInventorInstances()
        {
            if (!VaultHelper.ConectarWithAD())
            {
                if (!VaultHelper.ConectarWithUserAndPassword())
                {
                    Log.GravarLog("Erro ao conectar ao Vault. Credenciais erradas", LogName.AppLog);
                    MessageBox.Show("Não foi possivel conectar no Autodesk VAULT. Verifique a conexão com o servidor." +
                        "O software será encerrado", "ERRO DE CONEXÂO COM O VAULT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            if (Parametros._invApp == null)
            {
                try
                {
                    Parametros._invApp = (Inventor.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Inventor.Application");
                }
                catch (Exception e1)
                {
                    Log.GravarLog("Erro ao iniciar a instância do Inventor", LogName.AppLog);
                    Log.GravarLog(e1.ToString(), LogName.AppLog);
                    MessageBox.Show(@"Erro Inexperado ao tentar abrir a instância do Inventor. Favor enviar o log para a KeepCAD\n " +
                        @"Pasta do log: C:\KeepCAD\ImportacaoArquivos\Log\AppLog.txt");
                    return false;
                }
            }
            return true;
        }
        private async Task<bool> VerifyLinks()
        {
            splashScreen.UpdateProgress(30, "Abrindo a montagem no Inventor para verificar possiveis erros de vinculos de peças");

            Parametros.mainAssemblyPath = Directory.GetFiles(config.Default.tempVaultRootPath, codNorma.CodigoNorma + ".iam", SearchOption.AllDirectories).FirstOrDefault();
            string mainDrawingPath = Directory.GetFiles(config.Default.tempVaultRootPath, codNorma.CodigoNorma + ".idw", SearchOption.AllDirectories).FirstOrDefault();

            if (string.IsNullOrEmpty(Parametros.mainAssemblyPath))
            {
                Log.GravarLog($"Montagem: {codNorma.CodigoNorma}.iam não encontrada", LogName.ProductLog);
                MessageBox.Show("Montagem não encontrada");
                Parametros.isResolved = false;
                return Parametros.isResolved;
            }

            if (!File.Exists(config.Default.DefaultIPJ))
            {
                Log.GravarLog($@"IPJ Padrão não encontrado em: C:\KeepCAD\ImportacaoArquivos\StandardIPJ.ipj", LogName.ProductLog);
                MessageBox.Show("Arquivo IPJ não encontrado.\nVerifique se existe o arquivo: " + config.Default.DefaultIPJ);
                Parametros.isResolved = false;
                return Parametros.isResolved;
            }

            FileInfo ipjFile = new FileInfo(config.Default.DefaultIPJ);
            ipjFile.CopyTo(Parametros.ipjPadrao, true);


            try
            {
                if (Parametros._invApp.Documents.Count > 0)
                    Parametros._invApp = (Inventor.Application)Activator.CreateInstance(Type.GetTypeFromProgID("Inventor.Application"));
            }
            catch (Exception)
            {
                //Parametros._invApp = (Inventor.Application)Activator.CreateInstance(Categoria.GetTypeFromProgID("Inventor.Application"));
                Log.GravarLog("Erro ao pegar a instancia do Inventor");
            }


            Parametros._invApp.Visible = true;
            Parametros._invApp.SilentOperation = true;
            List<string> item = new List<string>();


            if (Parametros._invApp.Documents.Count == 0)
            {
                // Abrir um novo projeto
                Parametros._invApp.DesignProjectManager.DesignProjects.AddExisting(Parametros.ipjPadrao).Activate();
                codNorma.oAsmDoc = Parametros._invApp.Documents.Open(Parametros.mainAssemblyPath, true) as AssemblyDocument;
                codNorma.Produtos.Find(x => x.NewFileName == Parametros.mainAssemblyPath).isAssemblyParticipant = true;

                int y = codNorma.oAsmDoc.AllReferencedDocuments.Count;
                await checkLinkRecursive(codNorma.oAsmDoc, item, 0, y);

                codNorma.oAsmDoc.Save2(true);
                codNorma.oAsmDoc.Update();
                //codNorma.oAsmDoc.Close();
                //Parametros._invApp.Documents.Open(codNorma.oAsmDoc.FullFileName, true);


            }
            else
            {
                MessageBox.Show("Todos os arquivos do Inventor precisam estar fechados. Por favor, feche todos os arquivos e tente novamente");
            }

            Parametros._invApp.SilentOperation = false;
            return Parametros.isResolved;
        }
        public async Task checkLinkRecursive(AssemblyDocument asmDoc, List<string> item, int contagem, int y)
        {
            foreach (DocumentDescriptor descriptor in asmDoc.ReferencedDocumentDescriptors)
            {
                if (item.Contains(descriptor.FullDocumentName))
                    continue;

                item.Add(descriptor.FullDocumentName);

                contagem++;
                int progress = (int)((contagem / (float)y) * 100);
                splashScreen.UpdateProgress(progress, $"Analisando os arquivos na montagem: {descriptor.FullDocumentName}");
                await Task.Delay(10); // Simula um pequeno delay para a UI ser atualizada

                if (descriptor.ReferenceMissing)
                {
                    // PESQUISANDO OS ARQUIVOS NO VAULT
                    Autodesk.Connectivity.WebServices.File[] files = null;
                    Produto2 produto = null;

                    if (VaultHelper.connection != null)
                    {
                        files = VaultHelper.FindFileByName(Path.GetFileName(Path.GetFileName(descriptor.FullDocumentName)));
                        if (files == null)
                        {
                            produto = new Produto2(descriptor.FullDocumentName, codNorma.CodigoNorma, true, false);
                            Log.GravarLog($"{descriptor.FullDocumentName};Não encontrado no pacote ZIP e nem no Vault", LogName.ProductLog);
                            Log.GravarLog($"MISSING: {descriptor.FullDocumentName}", LogName.ArquivosFaltantes);
                            Parametros.isResolved = false;
                        }
                        else
                        {
                            produto = new Produto2(files[0], descriptor.FullDocumentName);
                            //  produto.NewFileName = 
                            Log.GravarLog($"{descriptor.FullDocumentName};Não encontrado no pacote ZIP, mas encontrado no Vault;{files.First().Name}", LogName.ProductLog);
                            Log.GravarLog($"FOUND ON VAULT: {descriptor.FullDocumentName} - VAULT NAME: {files.First().Name}", LogName.ArquivosFaltantes);
                        }
                    }

                    if (produto != null)
                    {
                        codNorma.Produtos.Add(produto);
                        produto.ParentAssembly.Add(asmDoc.FullFileName);
                    }
                }
                else
                {
                    Produto2 prod = codNorma.Produtos.Find(x => x.NewFileName == descriptor.FullDocumentName);

                    if (prod == null)
                    {
                        //MessageBox.Show("Produto nulo: " + descriptor.FullDocumentName);
                        continue;
                    }

                    prod.isAssemblyParticipant = true;

                    if (descriptor.FullDocumentName.EndsWith(".iam"))
                    {
                        await checkLinkRecursive(Parametros._invApp.Documents.Open(descriptor.FullDocumentName, false) as AssemblyDocument, item, contagem, y);
                    }
                }
            }
        }
        private async Task PesquisarVault()
        {
            // PESQUISANDO OS ARQUIVOS NO VAULT
            if (VaultHelper.connection == null) return;
            int contagem = 0;

            foreach (Produto2 prod in codNorma.Produtos)
            {
                int progress = (int)((contagem / (float)codNorma.Produtos.Count) * 100);
                splashScreen.UpdateProgress(progress, $"Pesquisando se os arquivos existem no Vault: {prod.NewFileName}");
                await Task.Delay(10); // Simula um pequeno delay para a UI ser atualizada
                contagem++;

                //if (prod.isMissing) continue;

                Autodesk.Connectivity.WebServices.File[] files = VaultHelper.FindFileByName(Path.GetFileName(prod.NewFileName));
                if (files == null)
                    prod.isVaultExisting = false;
                else
                {
                    prod.isVaultExisting = true;

                    foreach (Autodesk.Connectivity.WebServices.File file in files)
                    {
                        if (file.Name.EndsWith(Path.GetExtension(prod.NewFileName)) && prod.Categoria == ProductType.LIBRARY)
                        {
                            //MessageBox.Show("Test");
                            prod.LifeCycleState = VaultHelper.GetLifecycleState(file);
                        }
                    }

                }

            }
        }
        public async Task LerCustomParameters()
        {
            int total = codNorma.Produtos.Count(x => x.Categoria == ProductType.LIBRARY);
            int contagem = 1;
            foreach (Produto2 prod in codNorma.Produtos)
            {
                if (prod.Categoria == ProductType.LIBRARY && !prod.isVaultExisting && !prod.isMissing)
                {
                    int progress = (int)((contagem / (float)total) * 100);
                    splashScreen.UpdateProgress(progress, $"Lendo propriedades custom da ATMOLIB: {prod.NewFileName}");
                    await Task.Delay(100); // Simula um pequeno delay para a UI ser atualizada
                    contagem++;

                    Document doc = Parametros._invApp.Documents.Open(prod.NewFileName, false);
                    prod.Doc = doc;
                    prod.propriedades = new Propriedades(doc);
                    doc.Save();
                    doc.Close();
                }
            }
        }
        private bool IsValidFileName(string fileName)
        {
            string invalidChars = new string(Path.GetInvalidFileNameChars());
            Regex regex = new Regex($"[{Regex.Escape(invalidChars)}]");
            return true;
            // return !regex.IsMatch(fileName);
        }
        #endregion

        #region OPERAÇÕES DE APOIO
        private void ToggleWaitCursor(bool isWaitCursor)
        {
            this.UseWaitCursor = isWaitCursor;
        }
        private void ClearUI()
        {
            Parametros._invApp.Documents.CloseAll();
            TreeBosch.Nodes.Clear();
            tabelaItens.Rows.Clear();
            // TabelaDesconhecidos.Rows.Clear();
            tabelaATMOLIB.Rows.Clear();
            tabelaRelatorios.Rows.Clear();
            btnCheckInNorma.Enabled = false;
            BtnCheckInAtmo.Enabled = false;

            cbLocation.Text = string.Empty;
            txtFolderToMove.Text = string.Empty;

        }
        private void SetTemporaryPaths()
        {
            Properties.Settings.Default.tempVaultRootPath = Path.Combine(Parametros.tempLocation, Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            Parametros.ipjPadrao = Path.Combine(config.Default.tempVaultRootPath, Parametros.ipjFileName);
        }
        private async Task<bool> ExtractFilesFromZip(string zipFileName)
        {
            if (string.IsNullOrEmpty(zipFileName) || !File.Exists(zipFileName))
            {
                ToggleWaitCursor(false);
                return false;
            }


            FileManipulateHelper zipfileManipulate = new FileManipulateHelper();
            codNorma = await zipfileManipulate.ExtractZip(zipFileName, splashScreen);

            return true;
        }
        private void PopulateTreeView()
        {
            // CRIAR A TREEVIEW         
            TreeViewHelper.PopulateTreeView(TreeBosch, codNorma);
            TreeBosch.SelectedNode = TreeBosch.TopNode;
        }
        private void CheckAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in tabelaItens.SelectedRows)
            {
                Produto2 prod = (Produto2)row.Tag;
                if (!prod.isVaultExisting)
                    row.Cells[0].Value = true;
            }
        }
        private void UncheckAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in tabelaItens.SelectedRows)
            {
                row.Cells[0].Value = false;
            }
        }
        private void CreateTextFileWithFileNames()
        {
            foreach (Produto2 prod in codNorma.Produtos)
            {
                string filePath = config.Default.tempVaultRootPath + codNorma.CodigoNorma + ".txt";
                File.AppendAllText(filePath, prod.NewFileName + System.Environment.NewLine);
            }
        }
        private void CreateReportFilesFromProdutos()
        {
            foreach (Produto2 prod in codNorma.Produtos)
            {
                string filePath = config.Default.tempVaultRootPath + codNorma.CodigoNorma + ".txt";
                File.AppendAllText(filePath, prod.NewFileName + System.Environment.NewLine);
            }
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

                    Log.GravarLog(e1.Message);
                }

            }
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
        public void MostrarMiniatura(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Produto2 prod = (Produto2)tabelaItens.Rows[e.RowIndex].Tag;
                if (prod.Thumbnail == null)
                {
                    try
                    {
                        Document doc = Parametros._invApp.Documents.Open(prod.NewFileName, false);
                        prod.Thumbnail = InventorThumbnail.GetThumbnail(doc);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Não foi possivel gerar a visualização");
                        return;

                    }

                    Form_Imagem form = new Form_Imagem();
                    form.imagem(prod.Thumbnail);
                    form.Show();
                }
            }
        }

        #endregion

        #region RELATORIOS
        public void GerarRelatorioByNodes(TreeNodeCollection Nodes)
        {
            string VaultWorkFolder = $@"C:\daten\users\{System.Environment.UserName}\VAULT-Root";

            foreach (TreeNode node in Nodes)
            {
                if (node.Text.Contains("."))
                {
                    Produto2 prod = (Produto2)node.Tag;
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
            string VaultWorkFolder = $@"C:\daten\users\{System.Environment.UserName}\VAULT-Root";


            foreach (Produto2 prod in codNorma.Produtos)
            {
                prod.NewFileName = Path.Combine(VaultWorkFolder, prod.FileNameSimplificado.Substring(2));
                GerarRelatorio("relatorioProdutos.txt", prod);

            }
        }
        public void GerarRelatorio(string nome_relatorio, Produto2 produto)
        {
            string[] names = produto.NewFileName.Split('\\');
            string linha = $"{produto.Categoria.ToString()};{names[names.Length - 2]};{names.Last()};{produto.NewFileName};{produto.node.Text}";

            string logPath = @"C:\KeepSoftwares\Bosch\Log\" + nome_relatorio;


            if (!Directory.Exists(Path.GetDirectoryName(logPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(logPath));

            File.AppendAllText(logPath, $"{linha}{System.Environment.NewLine}");
        }
        #endregion
        #region OPERAÇÕES DE TABELAS
        public void CriarTabelaItens(TreeNodeCollection nodes)
        {

            foreach (TreeNode node in nodes)
            {
                Produto2 prod = node.Tag as Produto2;
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
        public void AdicionarLinhaTabelaItens(Produto2 prod)
        {

            int rowIndex = tabelaItens.Rows.Add(TableFormat.Linha(prod));
        }
        public void CriarTabelaPropriedades()
        {
            tabelaATMOLIB.Rows.Clear();
            Parametros._invApp.SilentOperation = true;
            foreach (Produto2 prod in codNorma.Produtos)
            {

                if (prod.Categoria == ProductType.LIBRARY && !prod.isMissing)
                {
                    if (prod.propriedades == null)
                    {
                        try
                        {
                            Document doc = Parametros._invApp.Documents.Open(prod.NewFileName, false);
                            prod.propriedades = new Propriedades(doc);
                            doc.Save();
                            doc.Close();
                        }
                        catch (Exception e1)
                        {

                            Log.GravarLog($"Erro ao preencher as propriedades: {prod.NewFileName} --- {e1.ToString()}");
                        }
                    }

                    int rowIndex = tabelaATMOLIB.Rows.Add(
                           prod.isVaultExisting,
                           prod.LifeCycleState,
                           Path.GetDirectoryName(prod.NewFileName).Split('\\').Last(),
                           Path.GetFileName(prod.NewFileName),
                           prod.propriedades.RBGBDETAILS,
                           prod.propriedades.RBGBPRODUCERNAME,
                           prod.propriedades.RBGBPRODUCERORDERNO);

                    tabelaATMOLIB.Rows[rowIndex].Tag = prod;
                    tabelaATMOLIB.Rows[rowIndex].ReadOnly = prod.isVaultExisting;

                }
            }
            Parametros._invApp.SilentOperation = false;

        }
        public void CriarTabelaRelatorios()
        {
            tabelaRelatorios.Rows.Clear();

            List<Produto2> produtosFaltando = codNorma.Produtos.Where(x => x.isMissing).ToList();
            PreencherTabelaRelatorio(produtosFaltando, "ARQUIVOS FALTANDO");
            List<Produto2> produtosDesconhecidos = codNorma.Produtos.Where(x => x.Categoria == ProductType.DESCONHECIDO).ToList();
            PreencherTabelaRelatorio(produtosDesconhecidos, "ARQUIVOS NÃO CLASSIFICADOS");

        }
        public void PreencherTabelaRelatorio(List<Produto2> prods, string Erro)
        {
            foreach (Produto2 prod in prods)
            {

                int rowIndex = tabelaRelatorios.Rows.Add(
                       Erro,
                       prod.NewFileName,
                       prod.InternalFileName);

                tabelaRelatorios.Rows[rowIndex].Tag = prod;
                //tabelaATMOLIB.Rows[rowIndex].ReadOnly = prod.isVaultExisting;

            }
        }


        private void BtnCheckIn_Click(object sender, EventArgs e)
        {
            SetButtonsEnable();
            if (!BtnCheckInAtmo.Enabled)
            {
                MessageBox.Show("Todos os arquivos de biblioteca já foram enviados para o vault");
                return;
            }

            splashScreen = new SplashScreen();
            Parametros._invApp.SilentOperation = true;

            splashScreen.Show();
            CloseAllDocuments();
            CopiarArquivosLibraryParaVaultWorkfolder();
            splashScreen.Close();

            Parametros._invApp.SilentOperation = false;


        }
        public void CloseAllDocuments()
        {
            int qtde_total = Parametros._invApp.Documents.Count;
            int quantidade = 1;
            Parametros._invApp.SilentOperation = true;
            foreach (Document doc in Parametros._invApp.Documents)
            {

                try
                {
                    splashScreen.UpdateProgress((quantidade / qtde_total) * 100, $"{qtde_total} de {quantidade} -  Fechando arquivos do Inventor {System.Environment.NewLine}Arquivo: {doc.FullDocumentName}");
                    doc.Save2();
                    doc.Close();
                }
                catch (Exception e1)
                {
                    doc.Close(true);
                    Log.GravarLog(e1.Message, LogName.ProductLog);
                }
                finally
                {
                    quantidade++;
                }


            }

            if (Parametros._invApp.Documents.Count > 0)
                CloseAllDocuments();

            Parametros._invApp.SilentOperation = false;

        }
        public void CopiarArquivosLibraryParaVaultWorkfolder()
        {
            List<Produto2> produtosNormaParaCadastrar = codNorma.Produtos
                .Where(x => x.Categoria == ProductType.LIBRARY && x.isVaultExisting == false)
                .ToList();

            int contador = 1;
            foreach (Produto2 prod in produtosNormaParaCadastrar)
            {

                prod.OldFileName = prod.NewFileName;
                prod.FileNameSimplificado = prod.FileNameSimplificado.Replace('/', '\\');
                prod.NewFileName = Path.Combine(Parametros.VaultWorkFolder, prod.FileNameSimplificado.Substring(2));
                try
                {
                    splashScreen.UpdateProgress((contador / produtosNormaParaCadastrar.Count) * 100, $"Copiando arquivos para a estrutura do Vault{System.Environment.NewLine}From: {prod.OldFileName}{System.Environment.NewLine}To: {prod.NewFileName}");
                    contador++;
                    if (!Directory.Exists(Path.GetDirectoryName(prod.NewFileName)))
                        Directory.CreateDirectory(Path.GetDirectoryName(prod.NewFileName));

                    if (!File.Exists(prod.NewFileName))
                        File.Copy(prod.OldFileName, prod.NewFileName);
                }
                catch (Exception ex)
                {
                    Log.GravarLog(ex.Message);
                    continue;
                    //MessageBox.Show(ex.ToString());
                }
            }
            OpenAssemblies(produtosNormaParaCadastrar);
        }
        public void OpenAssemblies(List<Produto2> produtosNormaParaCadastrar)
        {
            List<Produto2> NormasParaCadastrarAssemblies = produtosNormaParaCadastrar.Where(x => x.NewFileName.EndsWith(".iam")).ToList();

            try
            {
                if (Parametros._invApp.Documents.Count > 0)
                    Parametros._invApp = (Inventor.Application)Activator.CreateInstance(Type.GetTypeFromProgID("Inventor.Application"));
            }
            catch (Exception)
            {
                //Parametros._invApp = (Inventor.Application)Activator.CreateInstance(Categoria.GetTypeFromProgID("Inventor.Application"));
                Log.GravarLog("Erro ao pegar a instancia do Inventor");
            }

            if (Parametros._invApp.Documents.Count != 0) return;


            Parametros._invApp.Visible = true;
            Parametros._invApp.SilentOperation = true;
            Parametros._invApp.DesignProjectManager.DesignProjects.AddExisting(Parametros.ProjectEDIT_ATMOLIB).Activate();

            List<string> arquivosToOpen = produtosNormaParaCadastrar.Select(x => x.NewFileName).ToList();

            foreach (Produto2 norma in NormasParaCadastrarAssemblies)
            {
                AssemblyDocument documentNorma = Parametros._invApp.Documents.Open(norma.NewFileName, true) as AssemblyDocument;
                if (arquivosToOpen.Contains(norma.NewFileName))
                    arquivosToOpen.Remove(norma.NewFileName);

                foreach (Document docDesc in documentNorma.AllReferencedDocuments)
                {
                    //if (docDesc is DocumentDescriptor)
                    {
                        // DocumentDescriptor OdocDesc = (DocumentDescriptor)docDesc;
                        if (arquivosToOpen.Contains(docDesc.FullDocumentName))
                            arquivosToOpen.Remove(docDesc.FullDocumentName);
                    }
                }
            }

            foreach (string arquivosRestantes in arquivosToOpen)
            {

                try
                {
                    if (arquivosRestantes.EndsWith("ipt"))
                    {

                        Document documentNorma = Parametros._invApp.Documents.Open(arquivosRestantes, true) as Document;
                    }

                }
                catch (Exception)
                {

                    continue;
                }
            }


        }
        public void VaultStatusUpdate()
        {
            foreach (Produto2 prod in codNorma.Produtos)
            {
                if (!prod.isVaultExisting)
                {
                    Autodesk.Connectivity.WebServices.File[] files = VaultHelper.FindFileByName(Path.GetFileName(prod.NewFileName));
                    if (files == null)
                        prod.isVaultExisting = false;
                    else
                        prod.isVaultExisting = true;
                }
                if (prod.isVaultExisting && prod.Categoria == ProductType.LIBRARY)
                {
                    Autodesk.Connectivity.WebServices.File[] files = VaultHelper.FindFileByName(Path.GetFileName(prod.NewFileName));
                    foreach (Autodesk.Connectivity.WebServices.File file in files)
                    {
                        if (file.Name.EndsWith(Path.GetExtension(prod.NewFileName)))
                        {
                            prod.LifeCycleState = VaultHelper.GetLifecycleState(file);
                        }
                    }
                }
            }
        }

        public void SetButtonsEnable()
        {
            VaultStatusUpdate();
            BtnCheckInAtmo.Enabled = false;
            btnCheckInNorma.Enabled = false;

            if (codNorma.Produtos.Any(x => x.Categoria == ProductType.DESCONHECIDO || x.Categoria == ProductType.NAOENCONTRADO || x.isMissing))
                return;

            var categoriasAtmo = new[] { ProductType.PRODUTOS_BOSCH, ProductType.LIBRARY };
            bool NeedCheckInLib = codNorma.Produtos.Any(x => categoriasAtmo.Contains(x.Categoria) && !x.isVaultExisting);

            if (NeedCheckInLib)
            {
                BtnCheckInAtmo.Enabled = true;
                return;
            }

            var categoriasNorma = new[] { ProductType.CONTENTCENTER, ProductType.NORMA, ProductType.NORMA_AUXILIAR };
            bool NeedCheckInNorma = codNorma.Produtos.Any(x => categoriasNorma.Contains(x.Categoria) && !x.isVaultExisting);

            if (NeedCheckInNorma)
            {
                btnCheckInNorma.Enabled = true;
                return;
            }
        }
        #endregion
        private void btnAtt_Click(object sender, EventArgs e)
        {
            tabelaItens.Rows.Clear();
            if (Parametros._invApp == null)
                MessageBox.Show("InvApp Nulo");
            else
                MessageBox.Show($"Inventor Silent Operation = {Parametros._invApp.SilentOperation.ToString()} " +
                    $"{System.Environment.NewLine}");

            SetButtonsEnable();

            CriarTabelaItens(TreeBosch.TopNode.Nodes);
            CriarTabelaPropriedades();
            TreeBosch.SelectedNode = TreeBosch.TopNode;

            this.Cursor = Cursors.Default;
            return;

        }
        private void btnCheckInNorma_Click(object sender, EventArgs e)
        {
            SetButtonsEnable();
            if (!btnCheckInNorma.Enabled)
            {
                MessageBox.Show("Todos os arquivos já foram enviados para o vault");
                return;
            }

            splashScreen = new SplashScreen();
            splashScreen.Show();
            Parametros._invApp.SilentOperation = true;

            CloseAllDocuments();
            splashScreen.UpdateProgress(50, "Copiando arquivos para a estrutura do Vault");
            MoverArquivosToVault();

            Parametros._invApp.SilentOperation = false;

        }

        private void btnReleaseFiles_Click(object sender, EventArgs e)
        {
            splashScreen = new SplashScreen();
            splashScreen.Show();
            splashScreen.UpdateProgress(20, "Liberando arquivos");
            string LifeCycleName = "BOSCH Release Process";
            string StateName = "LIB released";


            //string LifeCycleName = "Basic Release Process";
            //string StateName = "Released";
            LfCycState LifeCycleState = null;

            LfCycDef[] Lifecycles = VaultHelper.connection.WebServiceManager.LifeCycleService.GetAllLifeCycleDefinitions();

            foreach (LfCycDef lifeCycle in Lifecycles)
            {
                if (lifeCycle.Name == LifeCycleName)
                {
                    foreach (LfCycState state in lifeCycle.StateArray)
                    {
                        if (state.Name == StateName)
                        {
                            LifeCycleState = state;
                            break;
                        }
                    }
                    break;

                }
            }
            long[] toStatesId = new long[1];
            toStatesId[0] = LifeCycleState.Id;

            AtualizarStatus(toStatesId, false);
            splashScreen.UpdateProgress(40, "Liberando arquivos");

            AtualizarStatus(toStatesId, false);
            splashScreen.UpdateProgress(60, "Liberando arquivos");

            AtualizarStatus(toStatesId, false);
            splashScreen.UpdateProgress(80, "Liberando arquivos");

            AtualizarStatus(toStatesId, true);

            splashScreen.UpdateProgress(90, "Atualizando a tabela");
            tabelaItens.Rows.Clear();

            SetButtonsEnable();

            CriarTabelaItens(TreeBosch.TopNode.Nodes);
            CriarTabelaPropriedades();

            splashScreen.Close();
           


        }

        public void AtualizarStatus(long[] toStatesId, bool mensagem)
        {
            foreach (Produto2 prod in codNorma.Produtos)
            {
                if (prod.Categoria == ProductType.LIBRARY && prod.isVaultExisting)
                {
                    if (prod.LifeCycleState != "Released")
                    {
                        Autodesk.Connectivity.WebServices.File[] files = VaultHelper.FindFileByName(Path.GetFileName(prod.NewFileName));
                        foreach (Autodesk.Connectivity.WebServices.File file in files)
                        {
                            if (file.Name.EndsWith(Path.GetExtension(prod.NewFileName)))
                            {
                                long[] fileMasterId = new long[1];
                                fileMasterId[0] = file.MasterId;

                                try
                                {
                                    VaultHelper.connection.WebServiceManager.DocumentServiceExtensions.UpdateFileLifeCycleStates(fileMasterId, toStatesId, "Liberado automaticamente pelo importador de arquivos");

                                }
                                catch (Exception e1)
                                {
                                    Log.GravarLog("Erro ao atualizar o Status do arquivo: " + prod.NewFileName + "\n" + e1.ToString());
                                    if (mensagem)
                                        MessageBox.Show("Erro ao atualizar o Status \n" + e1.ToString());
                                }

                            }
                        }
                    }
                }
            }
        }
    }
    public static class TableFormat
    {
        public static ImageList IconesComandos;
        public static ImageList IconesInventor;
        public static DataGridView Tabela;

        public static Image GetInventorFileTypeIcon(Produto2 produto)
        {
            switch (Path.GetExtension(produto.NewFileName).Split('.').Last())
            {
                case "ipt":
                    return Resources._ipt;
                case "iam":
                    return Resources._iam;
                case "idw":
                    return Resources._idw;
                case "dwg":
                    return Resources._dwg;
                case "ipn":
                    return Resources._ipn;

                default: return Resources.Outros;
            }
        }
        public static string GetDisplayName(Produto2 prod)
        {
            if (prod.Categoria == ProductType.DESCONHECIDO)
            {
                return $"{Path.GetFileName(prod.NewFileName)} - {prod.InternalFileName}";
            }

            return Path.GetFileName(prod.NewFileName);
        }
        public static Color GetRowColor(Produto2 prod)
        {
            if (prod.isMissing)
            {
                return Color.Red;
            }

            if (prod.Categoria == ProductType.DESCONHECIDO)
            {
                return Color.MediumVioletRed;
            }

            if (prod.Categoria == ProductType.LIBRARY)
            {
                // AtmoLib InternalZipFileName: FABRICANTE - CATALOGCODE
                // AtmoLib FolderPath: FABRICANTE
                string Filename = Path.GetFileNameWithoutExtension(prod.NewFileName).Split(' ').First();
                string FilePath = Directory.GetParent(prod.NewFileName).Name;
                //string FilePath = prod.node?.Parent?.Text;

                if (Filename != FilePath)
                {
                    return Color.BlueViolet;
                }
            }
            return Color.Black;
        }
        public static DataGridViewRow Linha(Produto2 prod)
        {
            DataGridViewRow linhaRow = new DataGridViewRow();
            linhaRow.CreateCells(Tabela);

            // Atribua valores às células
            linhaRow.Cells[0].Value = false;
            linhaRow.Cells[1].Value = prod.isVaultExisting;
            linhaRow.Cells[2].Value = prod.isAssemblyParticipant;
            linhaRow.Cells[3].Value = prod.Categoria.ToString();
            linhaRow.Cells[4].Value = GetInventorFileTypeIcon(prod);  // Imagem do ícone
            linhaRow.Cells[5].Value = Path.GetDirectoryName(prod.NewFileName).Split('\\').Last();  // Diretório
            linhaRow.Cells[6].Value = Path.GetFileName(prod.NewFileName);  // Nome do arquivo
                                                                           //linhaRow.Cells[7].Value = 


            //    IconesComandos.Images["RENOMEAR"];  // Imagem para "Renomear"
            //linhaRow.Cells[8].Value = IconesComandos.Images["PROCURAR2"];  // Imagem para "Procurar"
            //linhaRow.Cells[9].Value = IconesComandos.Images["DELETAR"];  // Imagem para "Deletar"
            //linhaRow.Cells[10].Value = IconesComandos.Images["MOVER"];  // Imagem para "Mover"

            linhaRow.DefaultCellStyle.ForeColor = GetRowColor(prod);
            linhaRow.ReadOnly = prod.isVaultExisting;
            linhaRow.Tag = prod;


            return linhaRow;

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
//    foreach (Produto2 prod in norma.Produtos)
//    {
//        FileInfo file = new FileInfo(prod.InternalZipFileName);
//        switch (prod.Categoria)
//        {
//            case ProductType.Norma:
//                LV_NORMA.Items.Add(Path.Combine(file.Directory.Name, Path.GetFileNameWithoutExtension(prod.InternalZipFileName)));
//                break;
//            case ProductType.ATMOLIB_ProdutosBosch:
//                LV_ATMO.Items.Add(Path.Combine(file.Directory.Name, Path.GetFileNameWithoutExtension(prod.InternalZipFileName)));
//                break;
//            case ProductType.CONTENTCENTER:
//                LV_CC.Items.Add(Path.Combine(file.Directory.Name, Path.GetFileNameWithoutExtension(prod.InternalZipFileName)));
//                break;
//            case ProductType.DESCONHECIDO:
//                LV_DESCONHECIDO.Items.Add(Path.Combine(file.Directory.Name, Path.GetFileNameWithoutExtension(prod.InternalZipFileName)));
//                break;
//        }
//    }
//}
