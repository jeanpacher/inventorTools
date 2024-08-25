
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Autodesk.Connectivity.WebServices;
using VDF = Autodesk.DataManagement.Client.Framework;
using config = Bosch_ImportData.Properties.Settings;
using File = Autodesk.Connectivity.WebServices.File;
using Folder = Autodesk.Connectivity.WebServices.Folder;

//using Autodesk.Connectivity.WebServicesTools;
//using Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;
//using Autodesk.DataManagement.Client.Framework.Vault.Currency.Connections;
//using System.Web.Services.Description;
//using static Autodesk.DataManagement.Client.Framework.Vault.Currency.Properties.PropertyDefinitionIds;
//using System.Data.Common;
//using System.Configuration;

namespace Bosch_ImportData
{
    public class VaultInterface
    {
     
        public void procurar()
        {

            // PropDef property;
            // property = PropertySearchType.AllProperties;
            //create a SearchCondition object
            SrchCond searchCondition = new SrchCond();
            searchCondition.PropDefId = 1;
            searchCondition.PropTyp = PropertySearchType.AllProperties;
            searchCondition.SrchOper = 1;
            searchCondition.SrchTxt = "000.30590.iam";
            //build our array of SearchConditions to use for the file search
            SrchCond[] conditions = new SrchCond[1];
            conditions[0] = searchCondition;
            string bookmark = string.Empty;
            SrchStatus status = null;
            Autodesk.Connectivity.WebServices.File[] files = VaultHelper.connection.WebServiceManager.DocumentService.FindFilesBySearchConditions(conditions, null, null, true, true, ref bookmark, out status);
            VDF.Vault.Currency.Entities.FileIteration fileItem;
            foreach (var file in files)
            {
                if (!file.Name.EndsWith(".iam"))
                    continue;
                fileItem = new VDF.Vault.Currency.Entities.FileIteration(VaultHelper.connection, file);
                if (fileItem == null)
                {
                    MessageBox.Show("Caiu aqui");
                    return;
                }
                Autodesk.Connectivity.WebServices.FileAssocArray[] associationArrays = VaultHelper.connection.WebServiceManager.DocumentService.GetFileAssociationsByIds(
                     new long[] { fileItem.EntityIterationId },
                    Autodesk.Connectivity.WebServices.FileAssociationTypeEnum.All, true,        // parent associations
                    Autodesk.Connectivity.WebServices.FileAssociationTypeEnum.Dependency, true,   // child associations
                    true, true);
                Autodesk.Connectivity.WebServices.FileAssoc[] associations = associationArrays[0].FileAssocs;
                Dictionary<long, List<VDF.Vault.Currency.Entities.FileIteration>> associationsByFile = new Dictionary<long, List<VDF.Vault.Currency.Entities.FileIteration>>();
                foreach (Autodesk.Connectivity.WebServices.FileAssoc association in associations)
                {
                    Autodesk.Connectivity.WebServices.File parent = association.ParFile;
                    if (associationsByFile.ContainsKey(parent.Id))
                    {
                        // parent is already in the hashtable, add an new child entry
                        List<VDF.Vault.Currency.Entities.FileIteration> list = associationsByFile[parent.Id];
                        list.Add(new VDF.Vault.Currency.Entities.FileIteration(VaultHelper.connection, association.CldFile));
                        // MessageBox.Show(new VDF.Vault.Currency.Entities.FileIteration(connection, association.CldFile).EntityName);
                    }
                    else
                    {
                        // add the parent to the hashtable.
                        List<VDF.Vault.Currency.Entities.FileIteration> list = new List<VDF.Vault.Currency.Entities.FileIteration>();
                        list.Add(new VDF.Vault.Currency.Entities.FileIteration(VaultHelper.connection, association.CldFile));
                        associationsByFile.Add(parent.Id, list);
                        // MessageBox.Show(new VDF.Vault.Currency.Entities.FileIteration(connection, association.CldFile).EntityName);
                    }
                    VDF.Vault.Settings.AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(VaultHelper.connection);
                    settings.AddEntityToAcquire(new VDF.Vault.Currency.Entities.FileIteration(VaultHelper.connection, association.CldFile));
                    //settings.AddFileToAcquire();
                    //string filePath = Path.Combine(Application.LocalUserAppDataPath, fileItem.EntityName);
                    //string filePath = @"C:\Vault_Workfolder\2\";
                    //settings.LocalPath = new VDF.Currency.FolderPathAbsolute(Path.GetDirectoryName(filePath));
                    VaultHelper.connection.FileManager.AcquireFiles(settings);
                }
                break;
            }
        }
        public void downloadFile(string arquivo)
        {
            //string filePath = @"C:\Vault_Workfolder\Vault_Selgron\temp\";
            //Directory.CreateDirectory(filePath);
            // For demonstration purposes, the information is hard-coded.
            VDF.Vault.Results.LogInResult results = VDF.Vault.Library.ConnectionManager.LogIn(
                "192.168.10.218", "Selgron_Vault", "Administrator", "selgron@2015", VDF.Vault.Currency.Connections.AuthenticationFlags.Standard, null
                );
            if (!results.Success)
            {
                MessageBox.Show("Erro na conexão");
                return;
            }
            VaultHelper.connection = results.Connection;
            //PropDef property;
            //create a SearchCondition object
            SrchCond searchCondition = new SrchCond();
            searchCondition.PropDefId = 1;
            searchCondition.PropTyp = PropertySearchType.AllProperties;
            searchCondition.SrchOper = 1;
            searchCondition.SrchTxt = arquivo;
            //build our array of SearchConditions to use for the file search
            SrchCond[] conditions = new SrchCond[1];
            conditions[0] = searchCondition;
            string bookmark = string.Empty;
            SrchStatus status = null;
            Autodesk.Connectivity.WebServices.File[] files = VaultHelper.connection.WebServiceManager.DocumentService.FindFilesBySearchConditions(conditions, null, null, true, true, ref bookmark, out status);
            if (files == null)
                return;
            foreach (var file in files)
            {
                try
                {
                    if (!file.Name.EndsWith(".idw"))
                        continue;
                    VDF.Vault.Currency.Entities.FileIteration fileItem = new VDF.Vault.Currency.Entities.FileIteration(VaultHelper.connection, file);
                    VDF.Vault.Settings.AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(VaultHelper.connection);
                    settings.AddEntityToAcquire(new VDF.Vault.Currency.Entities.FileIteration(VaultHelper.connection, fileItem));
                    //settings.LocalPath = new VDF.Currency.FolderPathAbsolute(Path.GetDirectoryName(filePath));
                    //VDF.Vault.Currency.Entities.Folder pasta = new VDF.Vault.Currency.Entities.Folder(connection, fileItem);
                    VaultHelper.connection.FileManager.AcquireFiles(settings);
                    //MessageBox.Show("Download concluido: " + Convert.ToString(settings.LocalPath));
                }
                catch (Exception e2)
                {
                    MessageBox.Show("Erro no arquivo: " + file.Name + "\n" + e2.ToString());
                }
            }
        }
        public void downloadAllFiles(string arquivo)
        {
            //string filePath = @"C:\Vault_Workfolder\Vault_Selgron\temp\";
            //Directory.CreateDirectory(filePath);
            // For demonstration purposes, the information is hard-coded.
            VDF.Vault.Results.LogInResult results = VDF.Vault.Library.ConnectionManager.LogIn(
                "192.168.10.218", "Selgron_Vault", "Administrator", "selgron@2015", VDF.Vault.Currency.Connections.AuthenticationFlags.Standard, null
                );
            if (!results.Success)
            {
                MessageBox.Show("Erro na conexão");
                return;
            }
            VaultHelper.connection = results.Connection;
            //PropDef property;
            //create a SearchCondition object
            SrchCond searchCondition = new SrchCond();
            searchCondition.PropDefId = 1;
            searchCondition.PropTyp = PropertySearchType.AllProperties;
            searchCondition.SrchOper = 1;
            searchCondition.SrchTxt = arquivo;
            //build our array of SearchConditions to use for the file search
            SrchCond[] conditions = new SrchCond[1];
            conditions[0] = searchCondition;
            string bookmark = string.Empty;
            SrchStatus status = null;
            Autodesk.Connectivity.WebServices.File[] files = VaultHelper.connection.WebServiceManager.DocumentService.FindFilesBySearchConditions(conditions, null, null, true, true, ref bookmark, out status);
            if (files == null) return;
            foreach (var file in files)
            {
                if (!file.Name.EndsWith("idw"))
                    continue;
                // CRIANDO O FILEITEM
                VDF.Vault.Currency.Entities.FileIteration fileItem = new VDF.Vault.Currency.Entities.FileIteration(VaultHelper.connection, file);
                VDF.Vault.Settings.AcquireFilesSettings oSettings = new VDF.Vault.Settings.AcquireFilesSettings(VaultHelper.connection);
                oSettings.AddEntityToAcquire(fileItem);
                VaultHelper.connection.FileManager.AcquireFiles(oSettings);
                //
                Autodesk.Connectivity.WebServices.FileAssocArray[] associationArrays = VaultHelper.connection.WebServiceManager.DocumentService.GetFileAssociationsByIds(
                     new long[] { fileItem.EntityIterationId },
                    Autodesk.Connectivity.WebServices.FileAssociationTypeEnum.All, true,        // parent associations
                    Autodesk.Connectivity.WebServices.FileAssociationTypeEnum.Dependency, true,   // child associations
                    true, true);
                Autodesk.Connectivity.WebServices.FileAssoc[] associations = associationArrays[0].FileAssocs;
                Dictionary<long, List<VDF.Vault.Currency.Entities.FileIteration>> associationsByFile = new Dictionary<long, List<VDF.Vault.Currency.Entities.FileIteration>>();
                foreach (Autodesk.Connectivity.WebServices.FileAssoc association in associations)
                {
                    Autodesk.Connectivity.WebServices.File parent = association.ParFile;
                    if (associationsByFile.ContainsKey(parent.Id))
                    {
                        // parent is already in the hashtable, add an new child entry
                        List<VDF.Vault.Currency.Entities.FileIteration> list = associationsByFile[parent.Id];
                        list.Add(new VDF.Vault.Currency.Entities.FileIteration(VaultHelper.connection, association.CldFile));
                        // MessageBox.Show(new VDF.Vault.Currency.Entities.FileIteration(connection, association.CldFile).EntityName);
                    }
                    else
                    {
                        // add the parent to the hashtable.
                        List<VDF.Vault.Currency.Entities.FileIteration> list = new List<VDF.Vault.Currency.Entities.FileIteration>();
                        list.Add(new VDF.Vault.Currency.Entities.FileIteration(VaultHelper.connection, association.CldFile));
                        associationsByFile.Add(parent.Id, list);
                        // MessageBox.Show(new VDF.Vault.Currency.Entities.FileIteration(connection, association.CldFile).EntityName);
                    }
                    VDF.Vault.Settings.AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(VaultHelper.connection);
                    settings.AddEntityToAcquire(new VDF.Vault.Currency.Entities.FileIteration(VaultHelper.connection, association.CldFile));
                    //
                    //settings.AddFileToAcquire();
                    //string filePath = Path.Combine(Application.LocalUserAppDataPath, fileItem.EntityName);
                    //string filePath = @"C:\Vault_Workfolder\2\";
                    //settings.LocalPath = new VDF.Currency.FolderPathAbsolute(Path.GetDirectoryName(filePath));
                    VaultHelper.connection.FileManager.AcquireFiles(settings);

                }
                break;
            }
        }
    }
    public class UserCredentials
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string credenciais()
        {
            return $"{Server}/{Database}/{Username}/{Password}";
        }
    }
    public static class VaultHelper
    {
        public static VDF.Vault.Currency.Connections.Connection connection = null;
        public static bool ConectarWithAD()
        {
            try
            {
                string filePath = Parametros.JsonFilename;
                UserCredentials credentials = new UserCredentials();
                credentials = ReadUserCredentials(filePath);


                VDF.Vault.Results.LogInResult results = VDF.Vault.Library.ConnectionManager.LogIn(
                    credentials.Server,
                    credentials.Database,
                    credentials.Username,
                    credentials.Password, VDF.Vault.Currency.Connections.AuthenticationFlags.WindowsAuthentication, null
                    );

                if (!results.Success)
                    return false;

                connection = results.Connection;

                Autodesk.Connectivity.WebServices.Folder rootFolder = connection.WebServiceManager.DocumentService.GetFolderRoot();
                config.Default.VaultRootPath = connection.WorkingFoldersManager.GetWorkingFolder(rootFolder.FullName).FullPath;
                return true;

            }
            catch (Exception e1)
            {
                Log.gravarLog(e1.ToString());
                MessageBox.Show("Erro ao conectar ao Vault \nErro: " + e1.Message);
                return false;
            }
        }
        public static bool ConectarWithUserAndPassword()
        {
            try
            {

                VDF.Vault.Results.LogInResult results = VDF.Vault.Library.ConnectionManager.LogIn(
                    "eng01-srv",
                    "Idugel",
                    "Administrator",
                    "1dugel.", VDF.Vault.Currency.Connections.AuthenticationFlags.Standard, null
                    );

                if (!results.Success)
                    return false;

                connection = results.Connection;

                Autodesk.Connectivity.WebServices.Folder rootFolder = connection.WebServiceManager.DocumentService.GetFolderRoot();
                config.Default.VaultRootPath = connection.WorkingFoldersManager.GetWorkingFolder(rootFolder.FullName).FullPath;
                return true;

            }
            catch (Exception e1)
            {
                Log.gravarLog(e1.ToString());
                MessageBox.Show("Erro ao conectar ao Vault \nErro: " + e1.Message);
                return false;
            }

        }
        public static UserCredentials ReadUserCredentials(string filePath)
        {
            try
            {
                // Verifica se o arquivo existe
                if (!System.IO.File.Exists(filePath))
                {
                    MessageBox.Show("Arquivo de credencial não encontrado");
                    throw new FileNotFoundException("O arquivo de credenciais não foi encontrado.");
                }

                // Lê o conteúdo do arquivo JSON
                string json = System.IO.File.ReadAllText(filePath);

                // Desserializa o JSON para o objeto UserCredentials
                UserCredentials credentials = new UserCredentials();
                credentials = JsonConvert.DeserializeObject<UserCredentials>(json);

                return credentials;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler as credenciais: {ex.Message}");
                return null;
            }
        }
        public static bool FindFileByName(string fileName)
        {

            //     PropDefId = The ID of the property definition to search on. This parameter is only used if
            //     PropTyp is set to SingleProperty.

            //     SrchOper = The type of search being done. If they types are AllProperties or AllPropertiesAndContent,
            //     then SrchOper must have a value of 1 (Contains).

            SrchCond searchCondition = new SrchCond
            {
                PropDefId = 9, // ID da propriedade do nome do arquivo
                PropTyp = PropertySearchType.SingleProperty,
                SrchOper = 3,  // Operador de pesquisa: "igual a"
                SrchTxt = fileName
            };

            string bookmark = string.Empty;
            SrchStatus status;
            File[] files = VaultHelper.connection.WebServiceManager.DocumentService.FindFilesBySearchConditions(

                new SrchCond[] { searchCondition },
                null,
                null,
                false,
                true,
                ref bookmark,
                out status);

            if (files == null)
                return false;
            else
                return true;
        }
        public static Folder FindFolderByName(string folderName)
        {
            //var foldItem = VaultHelper.connection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FLDR")

            try
            {
                // Definir a condição de pesquisa para o nome da pasta
                SrchCond searchCondition = new SrchCond
                {
                    PropDefId = 30, // ID da propriedade "Nome da Pasta"
                    PropTyp = PropertySearchType.SingleProperty,
                    SrchOper = 3,  // Operador de pesquisa: "igual a"
                    SrchRule = SearchRuleType.Must,
                    SrchTxt = folderName
                };
                string[] paths = { folderName };

                Folder[] folders = VaultHelper.connection.WebServiceManager.DocumentService.FindFoldersByPaths(paths);


                string bookmark = string.Empty;
                SrchStatus status;

                //Folder[] folders = VaultHelper.connection.WebServiceManager.DocumentService.FindFoldersBySearchConditions(
                //    new SrchCond[] { searchCondition },
                //    null,
                //    null,
                //    false,
                //    ref bookmark,
                //    out status
                //);

                // Retorna a primeira pasta encontrada com o nome especificado (ou null se não encontrada)
                return folders?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar a pasta: {ex.ToString()}");
                return null;
            }
        }
        public static List<string> GetChildrenForldersByFolderEntity(Folder folder)
        {
            List<string> listaSubPastas = new List<string>();

            Folder[] subpastas = VaultHelper.connection.WebServiceManager.DocumentService.GetFoldersByParentId(folder.Id, false);

            foreach (Folder subpasta in subpastas)
            {
                Log.gravarLog($"{subpasta.Id} - {subpasta.Name}");
                listaSubPastas.Add(subpasta.Name);
            }
            return listaSubPastas;
        }
        public static List<string> GetChildrenFoldersByFolderName(string FolderName)
        {
            List<string> ChildrenFolders = new List<string>();

            Folder folder = FindFolderByName(FolderName);

            Folder[] subpastas = VaultHelper.connection.WebServiceManager.DocumentService.GetFoldersByParentId(folder.Id, false);

            foreach (Folder subpasta in subpastas)
            {
                Log.gravarLog($"{subpasta.Id} - {subpasta.Name}");
                ChildrenFolders.Add(subpasta.Name);
            }
            return ChildrenFolders;
        }

        public static List<string> GetAllFolderNames()
        {
            List<string> folderNames = new List<string>();

            try
            {
                Autodesk.Connectivity.WebServices.Folder rootFolder = VaultHelper.connection.WebServiceManager.DocumentService.GetFolderRoot();
                Autodesk.Connectivity.WebServices.Folder[] VaultFolders = VaultHelper.connection.WebServiceManager.DocumentService.GetFoldersByParentId(rootFolder.Id, true);

                foreach (Folder folder in VaultFolders)
                {
                    Log.gravarLog(folder.Name + ";" + folder.FullName + ";" + folder.Id);
                }

            }
            catch (Exception e1)
            {

                MessageBox.Show("Erro ao buscar todas as pastas: " + e1.ToString());
            }
            // Obter a raiz das pastas


            return folderNames;
        }

    }
}
