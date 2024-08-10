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
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;



namespace Bosch_ImportData
{
    public partial class ImportFiles : Form
    {
        public Inventor.Application _invApp = null;
        public string tempRootPath { get; set; } = Properties.Settings.Default.tempVaultRootPath;
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
            ipjPadrao = tempRootPath + "StandardIPJ.ipj";
        }
        private void btnSearchZip_Click(object sender, EventArgs e)
        {
            //LV_ATMO.Items.Clear();
            //LV_CC.Items.Clear();
            //LV_DESCONHECIDO.Items.Clear();
            //LV_ATMO.Items.Clear();

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



            // Extrair os arquivos do zip no mesmo diretorio
            ZipfileManipulate zipfileManipulate = new ZipfileManipulate();
            codNorma = zipfileManipulate.ExtractZip(txtZipFileName.Text);


            // CRIAR UM TXT COM OS ARQUIVOS
            foreach (Produto prod in codNorma.Produtos)
            {
                File.AppendAllText(tempRootPath + codNorma.CodigoNorma + ".txt", prod.NewFileName + System.Environment.NewLine);
            }



            if (VerifyLinks())
                MessageBox.Show("MONTAGEM OK");
            else
                MessageBox.Show("ARQUIVOS FALTANDO");



            // CRIAR A TREEVIEW
            TreeViewBosch tb = new TreeViewBosch();
            tb.TreeCreate(TreeBosch, codNorma);



            //GroupByFolder();


            // Listar objetos
            //ListarObjetos(codNorma, destinationDirectory);


            // Procurar o arquivo principal de montagem do Inventor
            // string[] files = FindMainInventorAssembly(destinationDirectory, fileNameWithoutExtension);

            // Verifica se existe algum erro de link

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
        private bool VerifyLinks()
        {
            FileInfo ipjFile = new FileInfo(Path.Combine(System.Windows.Forms.Application.StartupPath, "StandardIPJ.ipj"));
            string projectFileName = Path.Combine(tempRootPath, "StandardIPJ.ipj");
            ipjFile.CopyTo(projectFileName, true);

            string mainAssemblyPath = Directory.GetFiles(tempRootPath, codNorma.CodigoNorma + ".iam", SearchOption.AllDirectories).FirstOrDefault();
            string mainDrawingPath = Directory.GetFiles(tempRootPath, codNorma.CodigoNorma + ".idw", SearchOption.AllDirectories).FirstOrDefault();

            if (mainAssemblyPath == null)
            {
                MessageBox.Show("Montagem não encontrada");
                return false;
            }

            isResolved = true;
            _invApp = (Inventor.Application)Activator.CreateInstance(Type.GetTypeFromProgID("Inventor.Application"));
            _invApp.Visible = true;
            _invApp.SilentOperation = true;
            AssemblyDocument asmDoc = null;

            if (_invApp.Documents.Count == 0)
            {
                // Abrir um novo projeto
                _invApp.DesignProjectManager.DesignProjects.AddExisting(projectFileName).Activate();
                asmDoc = _invApp.Documents.Open(mainAssemblyPath, true) as AssemblyDocument;

                checkLinkRecursive(asmDoc);
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
        private void BtnTeste_Click(object sender, EventArgs e)
        {
            OpenInventor();
        }
        private void btnCloseInventor_Click(object sender, EventArgs e)
        {
            _invApp.SilentOperation = false;
            _invApp.Quit();
        }
        private void ImportFiles_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_invApp != null)
            {
                _invApp.SilentOperation = false;
                _invApp.Quit();
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

            // Habilitar/desabilitar itens de menu com base no ListView
            //if (sourceControl == LV_NORMA)
            //{
            //    // Desabilitar menuItem1 se o ContextMenuStrip for aberto por listView1
            //    moverParaNormaToolStripMenuItem.Enabled = false;
            //}
            //else if (sourceControl == LV_ATMO)
            //{
            //    // Desabilitar menuItem2 se o ContextMenuStrip for aberto por listView2
            //    moverParaATMOLIBToolStripMenuItem.Enabled = false;
            //}
            //else if (sourceControl == LV_CC)
            //{
            //    // Desabilitar menuItem1 e menuItem2 se o ContextMenuStrip for aberto por listView3
            //    moverParaCCToolStripMenuItem.Enabled = false;
            //}
        }
        private void TreeViewNorma_AfterSelect(object sender, TreeViewEventArgs e)
        {
            isSystemChange = true;
            tabelaItens.Rows.Clear();

            if (e.Node.Text.Contains('.'))
            {
                Produto prod = e.Node.Tag as Produto;

                if (prod.Type == ProductType.Desconhecido)

                    tabelaItens.Rows.Add(prod.Type.ToString(), $"{Path.GetFileName(prod.NewFileName)} - {prod.Filename}", "SUBSTITUIR");

                else
                    tabelaItens.Rows.Add(prod.Type.ToString(), Path.GetFileName(prod.NewFileName), "SUBSTITUIR");

               

            }
            listarProdutos(e.Node.Nodes);


            isSystemChange = false;


         
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
                    
                        tabelaItens.Rows.Add(prod.Type.ToString(), $"{Path.GetFileName(prod.NewFileName)} - {prod.Filename}", "SUBSTITUIR");
                    
                    else
                        tabelaItens.Rows.Add(prod.Type.ToString(), Path.GetFileName(prod.NewFileName) , "SUBSTITUIR");



                    if (node.Nodes.Count > 0)
                    {
                        listarProdutos(node.Nodes);

                    }

                }

            }


        }
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
            
            if (e.ColumnIndex == 1)
            MessageBox.Show(tabelaItens.Rows[e.RowIndex].Cells[1].Value.ToString());

        }
 

        public void renameFile(Produto prod)
        {


            //if (prod.NewFileName.EndsWith(".ipt"))
            //{
            //    PartDocument document 
            //}

            //ComponentOccurrence occurrence = null;
            //occurrence.
            //Document doc = _invApp.ActiveEditDocument;
            //doc.FilePropertySets.
            //designProjectManager.
        }
    }
}


