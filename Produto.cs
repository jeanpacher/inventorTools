using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventor;
using System.IO;
using Path = System.IO.Path;
using System.Runtime.InteropServices;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using Config = Bosch_ImportData.Properties.Settings;
using System.Reflection;
using System.Xml.Linq;
using File = Autodesk.Connectivity.WebServices.File;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;
using Folder = Autodesk.Connectivity.WebServices.Folder;

//Autodesk.Connectivity.WebServices.File arquivo;

namespace Bosch_ImportData
{
    public enum ProductType
    {
        Norma,
        ATMOLIB_Library,
        ATMOLIB_ProdutosBosch,
        NormaAuxiliar,
        ContentCenter,
        Desconhecido,
        Vault,
        NaoEncontrado
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
        public List<Produto> Produtos = new List<Produto>();
        public AssemblyDocument oAsmDoc { get; set; }

        public Produto GetNewProduct(string filename, bool _isMissing)
        {
            Produto produto = new Produto(filename, CodigoNorma, _isMissing);
            Produtos.Add(produto);
            return produto;
        }
    }
    public class Produto
    {

        public TreeNode node;

        public string Filename { get; set; }
        public string NewFileName { get; set; }
        public string FileNameSimplificado { get; set; }
        public string OldFileName { get; set; } = string.Empty;
        public bool isMissing { get; set; }
        public bool isVaultExisting { get; set; }
        public bool isNeedMove { get; set; } = false;
        public SourceFile origem { get; set; }
        public ProductType Type { get; set; }
        public FileType TipoArquivo { get; set; }
        public string IconName { get; set; }
        public Propriedades propriedades { get; set; } = null;

        public Produto() { }
        public Produto(string _filename, string CodNorma, bool missing)
        {
            isMissing = missing;
            Filename = _filename;
            string name = Path.GetFileNameWithoutExtension(Filename);
            

            // DEFININDO O ICONE DO INVENTOR (PEÇA, MONTAGEM, DETALHAMENTO, ETC)
            string[] imagensNames = { ".ipt", ".iam", ".dwg", ".idw", ".ipn", "outros" };

            if (imagensNames.Any(str => str.Contains(Path.GetExtension(Filename))))
                IconName = Path.GetExtension(Filename);
            else
                IconName = "outros";


            // PESQUISANDO OS ARQUIVOS NO VAULT
            File[] files = null;

            if (VaultHelper.connection != null)
            {
                files = VaultHelper.FindFileByName(Path.GetFileName(NewFileName));
                if (files == null)
                    isVaultExisting = false;
                else
                    isVaultExisting = true;
            }
            //else
            //    MessageBox.Show("O Vault não está conectado.", "Erro de conexão com o Vault", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            if (isMissing)
            {
                if (isVaultExisting)
                {
                    isMissing = false;
                    origem = SourceFile.FromVault;
                    SetVaultProduct(files);
                }
                else
                {
                    origem = SourceFile.IsMissing;
                    DefineTypeName(Filename, CodNorma);
                    FileNameSimplificado = FileNameSimplificado.Replace('$', '@');
                }
            }
            else
            {
                origem = SourceFile.FromClient;
                DefineTypeName(Filename, CodNorma);
            }
        }

        public void SetVaultProduct(File[] files)
        {
            File file = files.FirstOrDefault();
            Folder folder = VaultHelper.connection.WebServiceManager.DocumentService.GetFolderById(file.FolderId);
            FileNameSimplificado = Path.Combine(folder.FullName, file.Name);
            NewFileName = Path.Combine(Config.Default.tempVaultRootPath, FileNameSimplificado.Substring(2));
            origem = SourceFile.FromVault;
            Type = ProductType.Vault;
            VaultHelper.DownloadFile(file, Path.GetDirectoryName(NewFileName));

        }


        //    else if (Filename.Contains("Content Center Files"))
        //    {
        //        Type = ProductType.ContentCenter;
        //        NewFileName = Path.Combine(temprootPath, Config.Default.ContentCenterRootPath, ConvertFilePath(Filename, "Content Center Files"));
        //    }

        //    else if (name.StartsWith(Directory.GetParent(Filename).Name))
        //    {

        //        Type = ProductType.ATMOLIB_Library;
        //        NewFileName = Path.Combine(temprootPath, Config.Default.Catalog, Directory.GetParent(Filename).Name, Path.GetFileName(Filename));
        //    }
        //    else
        //    {
        //        Type = ProductType.Desconhecido;
        //        NewFileName = Path.Combine(temprootPath, "DESCONHECIDO", fileInfo.Name);
        //    }



        //    FileNameSimplificado = NewFileName.Replace(temprootPath, @"$\");

        //}
        public void DefineTypeName(string filenameOriginal, string codigo)
        {
            string[] bibliotecas = { "en-US", "pt-BR, pt-PT" };
            filenameOriginal = filenameOriginal.Replace("/", "\\");
            string[] parts = filenameOriginal.Split('\\');
            string name = parts.Last();

            if (parts.Length > 2)
            {
                // -1 = ultima parte - filename
                // -2 = pasta do arquivo
                // -3 = pasta anterior a pasta do arquivo.
                if (bibliotecas.Any(str => str.Contains(parts[parts.Length - 3])))
                {
                    Type = ProductType.ContentCenter;
                    FileNameSimplificado = Path.Combine("$", Config.Default.ContentCenterRootPath, parts[parts.Length - 3], parts[parts.Length - 2], parts[parts.Length - 1]);
                    NewFileName = Path.Combine(Config.Default.tempVaultRootPath, FileNameSimplificado.Substring(2));
                    return;
                }
            }
            if (name.StartsWith(codigo))
            {
                Type = ProductType.Norma;
                FileNameSimplificado = Path.Combine("$", Config.Default.ProjectRootPath, codigo, name);
                NewFileName = Path.Combine(Config.Default.tempVaultRootPath, Config.Default.ProjectRootPath, codigo, Path.GetFileName(filenameOriginal));
            }
            else if (filenameOriginal.Contains("Catalog"))
            {
                Type = ProductType.ATMOLIB_Library;
                string classe = "Catalog";
                string substring = filenameOriginal.Substring(filenameOriginal.LastIndexOf(classe) + classe.Length + 1);
                FileNameSimplificado = Path.Combine("$", Config.Default.Catalog, substring);
                NewFileName = Path.Combine(Config.Default.tempVaultRootPath, FileNameSimplificado.Substring(2));
            }
            else if (filenameOriginal.Contains("Produtos Bosch"))
            {
                Type = ProductType.ATMOLIB_ProdutosBosch;
                string classe = "Produtos Bosch";
                string substring = filenameOriginal.Substring(filenameOriginal.LastIndexOf(classe) + classe.Length + 1);
                FileNameSimplificado = Path.Combine("$", Config.Default.ProdutosBosch, substring);
                NewFileName = Path.Combine(Config.Default.tempVaultRootPath, FileNameSimplificado.Substring(2));
            }
            else if (name.StartsWith("43") || name.StartsWith("45") || name.StartsWith("46") || name.StartsWith("47"))
            {
                Type = ProductType.NormaAuxiliar;
                FileNameSimplificado = Path.Combine("$", Config.Default.ProjectRootPath, parts[parts.Length - 2], name);
                NewFileName = Path.Combine(Config.Default.tempVaultRootPath, Config.Default.ProjectRootPath, parts[parts.Length - 2], name);
            }
            else
            {
                Type = ProductType.Desconhecido;
                FileNameSimplificado = Path.Combine("$", "DESCONHECIDO", name);
                NewFileName = Path.Combine(Config.Default.tempVaultRootPath, "DESCONHECIDO", name);
            }
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
                foreach (Property prop in customProps)
                {
                    if (prop.Name == nomePropriedade)
                    {
                        return prop.Value.ToString();
                    }
                }
                Property newProp = customProps.Add("", nomePropriedade);
                return "";
            }
            catch (Exception ex)
            {
                return $"Erro ao verificar ou criar a propriedade: {ex.Message}";
            }
        }

        public bool ChangePropertyValue(PropertySet customProps, string NomePropriedade, string valorPropriedade)
        {
            try
            {
                customProps[NomePropriedade].Value = valorPropriedade;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar a propriedade\n" + ex.ToString());
                return false;
            }
        }
    }
}
