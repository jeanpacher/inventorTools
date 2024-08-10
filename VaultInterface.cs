using Autodesk.Connectivity.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VDF = Autodesk.DataManagement.Client.Framework;

namespace Bosch_ImportData
{
    public class VaultInterface
    {

        //private VDF.Vault.Currency.Connections.Connection m_connection;
        public VDF.Vault.Currency.Connections.Connection connection;



        public void conectar()
        {
            // For demonstration purposes, the information is hard-coded.
            VDF.Vault.Results.LogInResult results = VDF.Vault.Library.ConnectionManager.LogIn(
                "192.168.10.218", "Selgron_Vault", "Administrator", "selgron@2015", VDF.Vault.Currency.Connections.AuthenticationFlags.Standard, null
                );

            if (!results.Success)
                return;

            connection = results.Connection;


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


            Autodesk.Connectivity.WebServices.File[] files = connection.WebServiceManager.DocumentService.FindFilesBySearchConditions(conditions, null, null, true, true, ref bookmark, out status);

            VDF.Vault.Currency.Entities.FileIteration fileItem;


            foreach (var file in files)
            {
                if (!file.Name.EndsWith(".iam"))
                    continue;


                fileItem = new VDF.Vault.Currency.Entities.FileIteration(connection, file);

                if (fileItem == null)
                {
                    MessageBox.Show("Caiu aqui");
                    return;
                }


                Autodesk.Connectivity.WebServices.FileAssocArray[] associationArrays = connection.WebServiceManager.DocumentService.GetFileAssociationsByIds(
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
                        list.Add(new VDF.Vault.Currency.Entities.FileIteration(connection, association.CldFile));
                        // MessageBox.Show(new VDF.Vault.Currency.Entities.FileIteration(connection, association.CldFile).EntityName);
                    }
                    else
                    {
                        // add the parent to the hashtable.
                        List<VDF.Vault.Currency.Entities.FileIteration> list = new List<VDF.Vault.Currency.Entities.FileIteration>();
                        list.Add(new VDF.Vault.Currency.Entities.FileIteration(connection, association.CldFile));
                        associationsByFile.Add(parent.Id, list);
                        // MessageBox.Show(new VDF.Vault.Currency.Entities.FileIteration(connection, association.CldFile).EntityName);

                    }


                    VDF.Vault.Settings.AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(connection);
                    settings.AddEntityToAcquire(new VDF.Vault.Currency.Entities.FileIteration(connection, association.CldFile));
                    //settings.AddFileToAcquire();

                    //string filePath = Path.Combine(Application.LocalUserAppDataPath, fileItem.EntityName);

                    //string filePath = @"C:\Vault_Workfolder\2\";


                    //settings.LocalPath = new VDF.Currency.FolderPathAbsolute(Path.GetDirectoryName(filePath));
                    connection.FileManager.AcquireFiles(settings);

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
            connection = results.Connection;


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

            Autodesk.Connectivity.WebServices.File[] files = connection.WebServiceManager.DocumentService.FindFilesBySearchConditions(conditions, null, null, true, true, ref bookmark, out status);

            if (files == null)
                return;

            foreach (var file in files)
            {
                try
                {

                    if (!file.Name.EndsWith(".idw"))
                        continue;

                    VDF.Vault.Currency.Entities.FileIteration fileItem = new VDF.Vault.Currency.Entities.FileIteration(connection, file);
                    VDF.Vault.Settings.AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(connection);

                    settings.AddEntityToAcquire(new VDF.Vault.Currency.Entities.FileIteration(connection, fileItem));
                    //settings.LocalPath = new VDF.Currency.FolderPathAbsolute(Path.GetDirectoryName(filePath));


                    //VDF.Vault.Currency.Entities.Folder pasta = new VDF.Vault.Currency.Entities.Folder(connection, fileItem);
                    connection.FileManager.AcquireFiles(settings);
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
            connection = results.Connection;


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

            Autodesk.Connectivity.WebServices.File[] files = connection.WebServiceManager.DocumentService.FindFilesBySearchConditions(conditions, null, null, true, true, ref bookmark, out status);

            if (files == null) return;

            foreach (var file in files)
            {
                if (!file.Name.EndsWith("idw"))
                    continue;



                // CRIANDO O FILEITEM
                VDF.Vault.Currency.Entities.FileIteration fileItem = new VDF.Vault.Currency.Entities.FileIteration(connection, file);

                VDF.Vault.Settings.AcquireFilesSettings oSettings = new VDF.Vault.Settings.AcquireFilesSettings(connection);
                oSettings.AddEntityToAcquire(fileItem);

                connection.FileManager.AcquireFiles(oSettings);

                //


                Autodesk.Connectivity.WebServices.FileAssocArray[] associationArrays = connection.WebServiceManager.DocumentService.GetFileAssociationsByIds(
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
                        list.Add(new VDF.Vault.Currency.Entities.FileIteration(connection, association.CldFile));
                        // MessageBox.Show(new VDF.Vault.Currency.Entities.FileIteration(connection, association.CldFile).EntityName);
                    }
                    else
                    {
                        // add the parent to the hashtable.
                        List<VDF.Vault.Currency.Entities.FileIteration> list = new List<VDF.Vault.Currency.Entities.FileIteration>();
                        list.Add(new VDF.Vault.Currency.Entities.FileIteration(connection, association.CldFile));
                        associationsByFile.Add(parent.Id, list);
                        // MessageBox.Show(new VDF.Vault.Currency.Entities.FileIteration(connection, association.CldFile).EntityName);

                    }


                    VDF.Vault.Settings.AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(connection);
                    settings.AddEntityToAcquire(new VDF.Vault.Currency.Entities.FileIteration(connection, association.CldFile));
                    //
                    //settings.AddFileToAcquire();

                    //string filePath = Path.Combine(Application.LocalUserAppDataPath, fileItem.EntityName);

                    //string filePath = @"C:\Vault_Workfolder\2\";


                    //settings.LocalPath = new VDF.Currency.FolderPathAbsolute(Path.GetDirectoryName(filePath));

                    connection.FileManager.AcquireFiles(settings);
                }


                break;



            }
        }
    }
}
