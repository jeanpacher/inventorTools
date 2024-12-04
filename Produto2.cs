using Inventor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Config = Bosch_ImportData.Properties.Settings;
using File = Autodesk.Connectivity.WebServices.File;
using Folder = Autodesk.Connectivity.WebServices.Folder;
using Path = System.IO.Path;
using VDF = Autodesk.DataManagement.Client.Framework;

//Autodesk.Connectivity.WebServices.File arquivo;

namespace Bosch_ImportData
{
    public enum ProductType
    {
        NORMA,
        LIBRARY,
        PRODUTOS_BOSCH,
        NORMA_AUXILIAR,
        CONTENTCENTER,
        DESCONHECIDO,
        NAOENCONTRADO
    }
    public enum FileType
    {
        Part,
        Assembly,
        IdwDrawing,
        DwgDrawing,
        Presentation
    }
    public enum SourceFile
    {
        FromVault,
        FromClient,
        IsMissing,
    }

    public class Norma
    {
        public string CodigoNorma { get; set; }
        public List<Produto2> Produtos = new List<Produto2>();
        public AssemblyDocument oAsmDoc { get; set; }
        public Produto2 GetNewProduct(string filename, bool _isMissing)
        {
            Produto2 produto = new Produto2(filename, CodigoNorma, _isMissing);
            Produtos.Add(produto);
            return produto;
        }
    }
    public class Produto2
    {
        public string InternalFileName { get; set; }
        public string NewFileName { get; set; }
        public string FileNameSimplificado { get; set; }
        public string VaultFileName{ get; set; }
        public string OldFileName { get; set; } = string.Empty;
        public bool isMissing { get; set; }
        public bool isVaultExisting { get; set; }
        public bool isAssemblyParticipant { get; set; } = false;
        public string IconName { get; set; }
        public bool IsDeleted { get; set; } = false;
        public SourceFile sourceFile { get; set; } = SourceFile.FromClient;
        public string LifeCycleState { get; set; } = string.Empty;
        public ProductType Categoria { get; set; }
        public FileType TipoArquivo { get; set; }
        public Document Doc { get; set; }
        public List<string> ParentAssembly { get; set; } = new List<string>();
        public Image Thumbnail { get; set; } = null;
        public Propriedades propriedades { get; set; } = null;
        public TreeNode node { get; set; }

        public Produto2() { }
        public Produto2(string originalFilename, string codigo, bool _isMissing, bool _isVault)
        {
            isMissing = _isMissing;
            isVaultExisting = _isVault;

            if (isMissing && !isVaultExisting)
            {
                DefineTypeName(originalFilename, codigo);
                FileNameSimplificado = Path.Combine("M", FileNameSimplificado.Substring(2));
                FileNameSimplificado = FileNameSimplificado.Replace("/", "\\");
            }
        }
        public Produto2(File vaultFile, string originalFilename)
        {
            isVaultExisting = true;
            sourceFile = SourceFile.FromVault;
            isMissing = false;
            InternalFileName = originalFilename;
            NewFileName = originalFilename;

            Folder folder = VaultHelper.connection.WebServiceManager.DocumentService.GetFolderById(vaultFile.FolderId);
            FileNameSimplificado = Path.Combine(folder.FullName, vaultFile.Name);
            FileNameSimplificado = FileNameSimplificado.Replace("/", "\\");
            VaultFileName = Path.Combine(Config.Default.tempVaultRootPath, FileNameSimplificado.Substring(2));
           
            //DefineType();

            VaultHelper.DownloadFile(vaultFile);
            VaultHelper.DownloadFile(vaultFile, Path.GetDirectoryName(VaultFileName));
            isAssemblyParticipant = true;
        }
        public Produto2(string _internalFilename, string CodNorma, bool missing)
        {
            isMissing = missing;
            InternalFileName = _internalFilename;
            string name = Path.GetFileNameWithoutExtension(InternalFileName);


            // DEFININDO O ICONE DO INVENTOR (PEÇA, MONTAGEM, DETALHAMENTO, ETC)
            string[] imagensNames = { ".ipt", ".iam", ".dwg", ".idw", ".ipn", "outros" };

            if (imagensNames.Any(str => str.Contains(Path.GetExtension(InternalFileName))))
                IconName = Path.GetExtension(InternalFileName);
            else
                IconName = "outros";

            DefineTypeName(InternalFileName, CodNorma);

        }

       
        public void DefineType()
        {
            if (NewFileName.Contains("Catalog"))
                Categoria = ProductType.LIBRARY;
            else if (NewFileName.Contains("Produtos Bosch"))
                Categoria = ProductType.PRODUTOS_BOSCH;
            else if (NewFileName.Contains("ContentCenter"))
                Categoria = ProductType.CONTENTCENTER;
            else if (NewFileName.Contains("project"))
                Categoria = ProductType.NORMA;
            else
                Categoria = ProductType.DESCONHECIDO;
        }
        public void DefineTypeName(string filenameOriginal, string codigo)
        {
            string[] bibliotecas = { "en-US", "pt-BR, pt-PT" };
            filenameOriginal = filenameOriginal.Replace("/", "\\");
            string[] parts = filenameOriginal.Split('\\');
            string name = parts.Last();
            string partial_name = null;
            if (name.Contains("_"))
                partial_name = name.Split('_').First();
            else
                partial_name = name.Split('.').First();
           
            
            if (parts.Length > 2)
            {
                // -1 = ultima parte - filename
                // -2 = pasta do arquivo
                // -3 = pasta anterior a pasta do arquivo.
                if (bibliotecas.Any(str => str.Contains(parts[parts.Length - 3])))
                {
                    Categoria = ProductType.CONTENTCENTER;
                    FileNameSimplificado = Path.Combine("$", Config.Default.ContentCenterRootPath, parts[parts.Length - 3], parts[parts.Length - 2], name);
                    FileNameSimplificado = FileNameSimplificado.Replace("/", "\\");
                    NewFileName = Path.Combine(Config.Default.tempVaultRootPath, FileNameSimplificado.Substring(2));
                    return;
                }
            }

            if (partial_name == codigo)
            {
                Categoria = ProductType.NORMA;
                FileNameSimplificado = Path.Combine("$", Config.Default.Project, codigo, name);
            }

            else if (filenameOriginal.Contains("Catalog"))
            {
                Categoria = ProductType.LIBRARY;
                string classe = "Catalog";
                string substring = filenameOriginal.Substring(filenameOriginal.LastIndexOf(classe) + classe.Length + 1);
                FileNameSimplificado = Path.Combine("$", Config.Default.Catalog, substring);
                
            }
            else if (filenameOriginal.Contains("Produtos Bosch"))
            {
                Categoria = ProductType.PRODUTOS_BOSCH;
                string classe = "Produtos Bosch";
                string substring = filenameOriginal.Substring(filenameOriginal.LastIndexOf(classe) + classe.Length + 1);
                FileNameSimplificado = Path.Combine("$", Config.Default.ProdutosBosch, substring);
              
            }
            else if (name.StartsWith("43") || name.StartsWith("45") || name.StartsWith("46") || name.StartsWith("47"))
            {
                Categoria = ProductType.NORMA_AUXILIAR;
                FileNameSimplificado = Path.Combine("$", Config.Default.Project, parts[parts.Length - 2], name);
             
            }
            else if (filenameOriginal.Contains("Content Center Files"))
            {
                Categoria = ProductType.CONTENTCENTER;
                string pasta = name.Split('-').First().TrimEnd(' ');
                FileNameSimplificado = Path.Combine("$", Config.Default.ContentCenterRootPath, "en-US", pasta, name); 
            }

            else if (CheckVaultFolders(name))
            {
                Categoria = ProductType.LIBRARY;
            }

            else if (CheckFileVault(name))
            {
                DefineType();
                return; 
            }
            else
            {
                Categoria = ProductType.DESCONHECIDO;
                FileNameSimplificado = Path.Combine("$", "DESCONHECIDO", name);
                
            }

            FileNameSimplificado = FileNameSimplificado.Replace("/", "\\");
            NewFileName = Path.Combine(Config.Default.tempVaultRootPath, FileNameSimplificado.Substring(2));

        }

        public bool CheckFileVault(string name)
        {
            Autodesk.Connectivity.WebServices.File[] files = VaultHelper.FindFileByName(name);

            if (files == null) return false;

            foreach (Autodesk.Connectivity.WebServices.File file in files)
            {

                if (file.Name.EndsWith(name.Split('.').Last()))
                {
                    VDF.Vault.Currency.Entities.FileIteration fileItem = new VDF.Vault.Currency.Entities.FileIteration(VaultHelper.connection, file);

                    isVaultExisting = true;
                    sourceFile = SourceFile.FromVault;
                    isMissing = false;
                    

                    Folder folder = VaultHelper.connection.WebServiceManager.DocumentService.GetFolderById(file.FolderId);
                    FileNameSimplificado = Path.Combine(folder.FullName, file.Name);
                    FileNameSimplificado = FileNameSimplificado.Replace("/", "\\");
                    NewFileName = Path.Combine(Config.Default.tempVaultRootPath, FileNameSimplificado.Substring(2));


                

                    return true;
                }
            }
            return false;
        }
        public int GetFileExtension(string filename)
        {
            switch (Path.GetExtension(filename))
            {
                case ".ipt":
                    TipoArquivo = FileType.Part;
                    return 0;
                case ".iam":
                    TipoArquivo = FileType.Assembly;
                    return 1;
                case ".idw":
                    TipoArquivo = FileType.IdwDrawing;
                    return 2;
                case ".dwg":
                    TipoArquivo = FileType.DwgDrawing;
                    return 3;
                case ".ipn":
                    TipoArquivo = FileType.Presentation;
                    return 4;
                default:
                    return 5;
            }

        }
        public bool CheckVaultFolders(string name)
        {
            string inicioName = name.Split(' ').First().TrimEnd();
            List<string> ListasPastasDisponiveis = new List<string>();

            string pastaCatalogs = @"$\ATMOLIB\Library\Catalog";
            if (VaultHelper.connection != null)
            {
                ListasPastasDisponiveis = VaultHelper.GetFoldersByPath(pastaCatalogs);
            }

            foreach (string lista in ListasPastasDisponiveis)
            {
                if (lista.ToUpper() == inicioName.ToUpper())
                {
                    FileNameSimplificado = Path.Combine(pastaCatalogs, lista, name);
                    FileNameSimplificado = FileNameSimplificado.Replace("/", "\\");
                    NewFileName = Path.Combine(Config.Default.tempVaultRootPath, FileNameSimplificado.Substring(2));
                    return true;
                }
            }

            return false;
        }
    }
    public class Propriedades
    {
      

        public string RBGBDETAILS { get; set; }
        public string RBGBPRODUCERNAME { get; set; }
        public string RBGBPRODUCERORDERNO { get; set; }
        public Propriedades(Document document)
        {
            

            PropertySet customProps = document.PropertySets["Inventor User Defined Properties"];

            RBGBDETAILS = CheckOrCreateProperty(customProps, "RBGBDETAILS");
            RBGBPRODUCERNAME = CheckOrCreateProperty(customProps, "RBGBPRODUCERNAME");
            RBGBPRODUCERORDERNO = CheckOrCreateProperty(customProps, "RBGBPRODUCERORDERNO");
        }
        public string CheckOrCreateProperty(PropertySet customProps, string nomePropriedade)
        {
            try
            {
                return customProps[nomePropriedade].Value;
            }
            catch (Exception)
            {
                return createProperty(customProps, nomePropriedade);
            }
            #region old
            //try
            //{
            //    foreach (Property prop in customProps)
            //    {
            //        if (prop.Name == nomePropriedade)
            //        {
            //            return prop.Value.ToString();
            //        }
            //    }
            //    Property newProp = customProps.Add("", nomePropriedade);
            //    return "";
            //}
            //catch (Exception ex)
            //{
            //    return $"Erro ao verificar ou criar a propriedade: {ex.Message}";
            //} 
            #endregion
        }

        public string createProperty(PropertySet customprops, string nomePropriedade)
        {
            try
            {
                customprops.Add("", nomePropriedade);
                return "";
            }
            catch (Exception ex)
            {
                return $"Erro ao verificar ou criar a propriedade: {ex.Message}";
            }
        }
        public bool ChangePropertyValue(Produto2 prod, string NomePropriedade, string valorPropriedade)
        {
            Document doc = Parametros._invApp.Documents.Open(prod.NewFileName, false);
            PropertySet customProps = doc.PropertySets["Inventor User Defined Properties"];
          
            try
            {
                customProps[NomePropriedade].Value = valorPropriedade;
                //foreach (Property prop in customProps)
                //{
                //    if (prop.Name == NomePropriedade)
                //    {
                //        prop.Value = valorPropriedade;
                //        return true;
                //    }
                //}
                //Property newProp = customProps.Add(valorPropriedade, NomePropriedade);
                return true;
            }
            catch (Exception ex)
            {
                customProps.Add(valorPropriedade, NomePropriedade);
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }



    public class Estrutura
    {
        public Produto2 Parent { get; set; }
        public List<Produto2> Itens { get; set; } = new List<Produto2>();
    }

    //public class Item
    //{
    //    public Produto TopLevelProduct { get; set; }
    //    public List<Produto> Produtos { get; set; } = new List<Produto>();
    //}


    public class Produto
    {
        public string InternalZipFileName { get; set; }
        public string Filename { get; set; } = string.Empty;
        public string OldFileName { get; set; } = string.Empty;
        public bool IsMissing { get; set; }
        public bool IsVaultExisting { get; set; }
        public bool IsAssemblyParticipant { get; set; } = false;
        public string IconName { get; set; }
        public bool IsDeleted { get; set; } = false;
        public SourceFile SourceFile { get; set; }

        public FileType TipoArquivo { get; set; }
        public Document Doc { get; set; }
        public Image Thumbnail { get; set; }
        public Propriedades AtmoLibProperties { get; set; } = null;
        public TreeNode Node { get; set; }
    }




}
