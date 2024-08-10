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
using Autodesk.DataManagement.Client.Framework.Vault.Forms.Controls;
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


    }


    public class Produto
    {
        public string temprootPath { get; set; } = Properties.Settings.Default.tempVaultRootPath;
        public string projectPath { get; set; } = Properties.Settings.Default.ProjectRootPath;
        public string AtmoPath { get; set; } = Properties.Settings.Default.AtmoRootPath;
        public string ContentCenterPath { get; set; } = Properties.Settings.Default.ContentCenterRootPath;


        //public string projectPath { get; set; } = @"Sites\CtP_TEF\project\";
        //public string AtmoPath { get; set; } = @"ATMOLIB\";
        //public string ContentCenterPath { get; set; } = @"ContentCenter\";
        public TreeNode node;

        public string Filename { get; set; }
        public string NewFileName { get; set; }
        public string FileNameSimplificado { get; set; }
        public ProductType Type { get; set; }
        public string Directory { get; set; }
        public Document InvDoc { get; set; }
        public bool isMissing { get; set; }
        //public string ContentCenterLanguage { get; set; }
        //public string ContentCenterNorma { get; set; }

        public Produto() { }

      

        public Produto(string _filename, string norma, bool missing = false)
        {
            isMissing = missing;
            Filename = _filename;
            FileInfo fileInfo = new FileInfo(Filename);
            string name = Path.GetFileNameWithoutExtension(Filename);

            if (name.StartsWith(norma))
            {
                Type = ProductType.Norma;
                NewFileName = Path.Combine(temprootPath, projectPath, norma, fileInfo.Name);

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


            //else if (Filename.Contains("ATMOLIB"))
            //{
            //    Type = ProductType.ATMO;
            //    string basePath = Path.Combine(temprootPath, AtmoPath);
            //    NewFileName = Path.Combine(temprootPath + AtmoPath, ConvertFilePath(Filename, "Library_from_ATMO-CDB"));
            //}
            else if (name.StartsWith("43") || name.StartsWith("45") || name.StartsWith("46") || name.StartsWith("47"))
            {
                Type = ProductType.NormaAuxiliar;
                NewFileName = Path.Combine(temprootPath, projectPath, fileInfo.Directory.Name, fileInfo.Name);
            }

            else
            {
                Type = ProductType.Desconhecido;          
                NewFileName = Path.Combine(temprootPath, "DESCONHECIDO", fileInfo.Name);
            }

            FileNameSimplificado = NewFileName.Replace(temprootPath, @"$\");
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

