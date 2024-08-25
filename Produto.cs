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

namespace Bosch_ImportData
{
    public enum ProductType
    {
        Norma,
        ATMO,
        ATMOLIB_Library,
        ATMOLIB_ProdutosBosch,
        NormaAuxiliar,
        ContentCenter,
        Desconhecido
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
        public string temprootPath { get; set; } = Properties.Settings.Default.tempVaultRootPath;
        public string projectPath { get; set; } = Properties.Settings.Default.ProjectRootPath;
        public TreeNode node;
        public string Filename { get; set; }
        public string NewFileName { get; set; }
        public string FileNameSimplificado { get; set; }
        public ProductType Type { get; set; }
        public bool isMissing { get; set; }
        public bool isVaultExisting { get; set; }
        public bool isNeedMove { get; set; } = false;
        public string PathToMove { get; set; } = string.Empty;

        public Produto() { }
        public Produto(string _filename, string CodNorma, bool missing)
        {
            isMissing = missing;
            Filename = _filename;
            FileInfo fileInfo = new FileInfo(Filename);
            string name = Path.GetFileNameWithoutExtension(Filename);

            if (name.StartsWith(CodNorma))
            {
                Type = ProductType.Norma;
                NewFileName = Path.Combine(temprootPath, projectPath, CodNorma, fileInfo.Name);
            }
            else if (Filename.Contains("Content Center Files"))
            {
                Type = ProductType.ContentCenter;
                NewFileName = Path.Combine(temprootPath, Properties.Settings.Default.ContentCenterRootPath, ConvertFilePath(Filename, "Content Center Files"));
            }
            else if (Filename.Contains("Catalog"))
            {
                Type = ProductType.ATMOLIB_Library;
                NewFileName = Path.Combine(temprootPath, Properties.Settings.Default.Catalog, ConvertFilePath(Filename, "Catalog"));
            }
            else if (Filename.Contains("Produtos Bosch"))
            {
                Type = ProductType.ATMOLIB_ProdutosBosch;
                NewFileName = Path.Combine(temprootPath, Properties.Settings.Default.ProdutosBosch, ConvertFilePath(Filename, "Produtos Bosch"));
            }

            else if (name.StartsWith("43") || name.StartsWith("45") || name.StartsWith("46") || name.StartsWith("47"))
            {
                Type = ProductType.NormaAuxiliar;
                NewFileName = Path.Combine(temprootPath, projectPath, fileInfo.Directory.Name, fileInfo.Name);
            }


            else if (name.StartsWith(Directory.GetParent(Filename).Name))
            {
                
                Type = ProductType.ATMOLIB_Library;
                NewFileName = Path.Combine(temprootPath, Properties.Settings.Default.Catalog, ConvertFilePath(Filename, "Produtos Bosch"));
            }
            else
            {
                Type = ProductType.Desconhecido;
                NewFileName = Path.Combine(temprootPath, "DESCONHECIDO", fileInfo.Name);
            }

            if (VaultHelper.connection != null)
                isVaultExisting = VaultHelper.FindFileByName(Path.GetFileName(NewFileName));


            FileNameSimplificado = NewFileName.Replace(temprootPath, @"$\");
        }

        private bool IsProdutoBosch()
        {
            return true;
        }

        private string ConvertFilePath(string originalFilePath, string oldPath)
        {
            // Remove a parte inicial e substitui por "C:\
            string relativePath = originalFilePath.Substring(originalFilePath.IndexOf(oldPath) + oldPath.Length + 1);
            relativePath = relativePath.Replace('/', '\\');
            return relativePath;
        }
    }


}
