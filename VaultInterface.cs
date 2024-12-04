
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
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;
using Inventor;
using VDF1 = Autodesk.DataManagement.Client.Framework.Vault;


using Autodesk.Connectivity.Explorer.Extensibility;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Properties;


namespace Bosch_ImportData
{
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
                string filePath = Parametros.JsonVaultUserData;
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
                //config.Default.VaultRootPath = connection.WorkingFoldersManager.GetWorkingFolder(rootFolder.FullName).FullPath;
                return true;

            }
            catch (Exception e1)
            {
                Log.GravarLog(e1.ToString());
                MessageBox.Show("Erro ao conectar ao VAULT \nErro: " + e1.Message);
                return false;
            }
        }
        public static bool ConectarWithUserAndPassword()
        {
            try
            {

                VDF.Vault.Results.LogInResult results = VDF.Vault.Library.ConnectionManager.LogIn(
                    "eng01-srv",
                    "IdugelTeste",
                    "Administrator",
                    "1dugel.", VDF.Vault.Currency.Connections.AuthenticationFlags.Standard, null
                    );

                if (!results.Success)
                    return false;

                connection = results.Connection;

                Autodesk.Connectivity.WebServices.Folder rootFolder = connection.WebServiceManager.DocumentService.GetFolderRoot();
                return true;

            }
            catch (Exception e1)
            {
                Log.GravarLog(e1.ToString());
                MessageBox.Show("Erro ao conectar ao VAULT \nErro: " + e1.Message);
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
        public static File[] FindFileByName(string fileName)
        {

            //     PropDefId = The ID of the property definition to search on. This parameter is only used if
            //     PropTyp is set to SingleProperty.

            //     SrchOper = The type of search being done. If they types are AllProperties or AllPropertiesAndContent,
            //     then SrchOper must have a value of 1 (Contains).

            SrchCond searchCondition = new SrchCond
            {
                PropDefId = 9 , // ID da propriedade do nome do arquivo
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
                true,
                true,
                ref bookmark,
                out status);

            return files;

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
                Log.GravarLog($"{subpasta.Id} - {subpasta.Name}");
                listaSubPastas.Add(subpasta.Name);
            }
            return listaSubPastas;
        }
        public static List<string> GetChildrenFoldersByFolderName(string FolderName)
        {
            List<string> ChildrenFolders = new List<string>();
            try
            {

                Folder folder = FindFolderByName(FolderName);

                Folder[] subpastas = VaultHelper.connection.WebServiceManager.DocumentService.GetFoldersByParentId(folder.Id, false);

                foreach (Folder subpasta in subpastas)
                {
                    Log.GravarLog($"{subpasta.Id} - {subpasta.Name}");
                    ChildrenFolders.Add(subpasta.Name);
                }
                return ChildrenFolders;

            }
            catch (Exception)
            {

                return ChildrenFolders;
            }
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
                    Log.GravarLog(folder.Name + ";" + folder.FullName + ";" + folder.Id);
                }

            }
            catch (Exception e1)
            {

                MessageBox.Show("Erro ao buscar todas as pastas: " + e1.ToString());
            }
            // Obter a raiz das pastas


            return folderNames;
        }
        public static FileIteration DownloadFile(File file, string destination = null)
        {
            try
            {
                VDF.Vault.Currency.Entities.FileIteration fileItem = new VDF.Vault.Currency.Entities.FileIteration(VaultHelper.connection, file);
                VDF.Vault.Settings.AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(VaultHelper.connection);

                if (destination != null)
                    settings.LocalPath = new VDF.Currency.FolderPathAbsolute(destination);

                settings.AddEntityToAcquire(new VDF.Vault.Currency.Entities.FileIteration(VaultHelper.connection, fileItem));
                VaultHelper.connection.FileManager.AcquireFiles(settings);
                return fileItem;
            }
            catch (Exception e1)
            {
                Log.GravarLog("Erro no arquivo: " + file.Name + "\n" + e1.ToString());
                MessageBox.Show("Erro no arquivo: " + file.Name + "\n" + e1.ToString());
                return null;
            }
        }
        public static void DownloadFiles(File[] files, string destination = null)
        {

            foreach (var file in files)
            {
                try
                {
                    if (file.Name.EndsWith(".dwf"))
                        continue;

                    VDF.Vault.Currency.Entities.FileIteration fileItem = new VDF.Vault.Currency.Entities.FileIteration(VaultHelper.connection, file);
                    VDF.Vault.Settings.AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(VaultHelper.connection);
                    settings.AddEntityToAcquire(new VDF.Vault.Currency.Entities.FileIteration(VaultHelper.connection, fileItem));


                    if (destination != null)
                        settings.LocalPath = new VDF.Currency.FolderPathAbsolute(destination);
                    //VDF.VAULT.Currency.Entities.Folder pasta = new VDF.VAULT.Currency.Entities.Folder(connection, fileItem);
                    VaultHelper.connection.FileManager.AcquireFiles(settings);
                    //MessageBox.Show("Download concluido: " + Convert.ToString(settings.LocalPath));
                }
                catch (Exception e2)
                {
                    MessageBox.Show("Erro no arquivo: " + file.Name + "\n" + e2.ToString());
                }
            }
        }
        public static List<string> GetFoldersByPath(string caminho)
        {
            caminho = caminho.Replace('\\', '/');

            List<string> ListaPastas = new List<string>();
            try
            {
                string[] paths = { caminho };
                Folder pastaPai = VaultHelper.connection.WebServiceManager.DocumentService.FindFoldersByPaths(paths).First();
                if (pastaPai.Id < 0) return ListaPastas;

                Folder[] pastasFilho = VaultHelper.connection.WebServiceManager.DocumentService.GetFoldersByParentId(pastaPai.Id, false);
                if (pastasFilho == null) return ListaPastas;

                foreach (Folder pasta in pastasFilho)
                {
                    if (ListaPastas.Contains(pasta.Name)) continue;
                    ListaPastas.Add(pasta.Name);
                }

                return ListaPastas;
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
                return ListaPastas;
            }
        }
        public static void ListarPropDefIds()
        {
            try
            {
                // Obter o serviço de propriedades
                PropertyService propService = connection.WebServiceManager.PropertyService;

                // Buscar todas as propriedades
                PropDef[] propriedades = propService.GetPropertyDefinitionsByEntityClassId("FILE");

                // Exibir o ID e o nome de cada propriedade
                foreach (PropDef propriedade in propriedades)
                {
                    System.Diagnostics.Debug.WriteLine($"PropDefId: {propriedade.Id}, Nome: {propriedade.DispName}");
                    System.IO.File.AppendAllText(@"C:\KeepSoftwares\Bosch\Log\PropDefIdTable.txt", $"PropDefId: {propriedade.Id}, Nome: {propriedade.DispName}{System.Environment.NewLine}");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao listar PropDefIds: " + ex.Message);
            }
        }

        public static string GetLifecycleState(Autodesk.Connectivity.WebServices.File file)
        {
            return file.FileLfCyc.LfCycStateName;
        }

    }
}



