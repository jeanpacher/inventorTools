﻿using System.Collections.Generic;
using System.Windows.Forms;
using env = System.Environment;

namespace Bosch_ImportData
{
    public static class Parametros
    {
        public static Inventor.Application _invApp { get; set; } = null;
        public static string CodNorma;
        public static Produto2 NormaAssembly { get; set; } = null;
        public static Produto2 NormaDrawing { get; set; } = null;

        public static string ipjPadrao { get; set; }
        public static bool isResolved { get; set; } = true;
        public static bool isSystemChange { get; set; } = true;
        public static string mainAssemblyPath { get; set; } = string.Empty;
        
        public static string ipjFileName { get; set; } = "StandardIPJ.ipj";
        public static string tempLocation { get; set; } = @"C:\Temp1\VaultWorkFolderBosch\";
        public static string JsonVaultUserData { get; set; } = @"C:\KeepCAD\ImportacaoArquivos\Json\Data.json";
        public static string JsonRegrasDeOrganizacaoDosArquivos { get; set; } = @"C:\KeepCAD\ImportacaoArquivos\Json\ProductInformation.json";
        public static ListaPadroesBosch padroesBosch { get; set; } = new ListaPadroesBosch();

        
        public static string VaultWorkFolder { get; set; } = $@"C:\daten\users\{env.UserDomainName}_{env.UserName}\VAULT-Root\";
        public static string ProjectEDIT_ATMOLIB { get; set; } = $@"C:\daten\users\{env.UserDomainName}_{env.UserName}\VAULT-Root\ATMOLIB\EDIT_ATMOLIB.ipj";
        public static string ProjectVaultFullFileName { get; set; } = $@"C:\daten\users\{env.UserDomainName}_{env.UserName}\VAULT-Root\VAULT-Root_Inv2012.ipj";
        public static string VaultProject { get; set; } = @"VAULT-Root_Inv2012.ipj";
        public static Dictionary<string, Produto> DicionarioProdutos { get; set; } = new Dictionary<string, Produto>();
        public static Dictionary<string, Item> DicionarioEstruturas { get; set; } = new Dictionary<string, Item>();
        public static HashSet<string> pastas { get; set; } = new HashSet<string>();
        public static List<string> TreeViewPastas { get; set; } = new List<string>();
        public static Dictionary<string, TreeNode> DicionarioNodes { get; set; } = new Dictionary<string, TreeNode>();
        public static Dictionary<string, TreeNode> DicionarioNodesMissing { get; set; } = new Dictionary<string, TreeNode>();
        
        


    }

}
