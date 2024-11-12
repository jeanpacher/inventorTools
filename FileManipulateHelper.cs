using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using SharpCompress.Archives;
using SharpCompress.Common;



namespace Bosch_ImportData
{
    public class FileManipulateHelper
    {
        //private DataGridView tabela { get; set; }
        //public ImageList FileTypeIcon { get; set; }
        //public ImageList CommandIcon { get; set; }
       
        //public FileManipulateHelper(DataGridView _tabela, ImageList _imageList)
        //{
        //    tabela = _tabela;
        //    FileTypeIcon = _imageList;
        //}

        public async Task<Norma> ExtractZip(string zipFilePath,  SplashScreen splash)
        {
            Norma norma = new Norma()
            {
                CodigoNorma = Path.GetFileNameWithoutExtension(zipFilePath),
            };

            using (var archive = ArchiveFactory.Open(zipFilePath))
            {
                int totalEntries = archive.Entries.Count();
                int currentEntry = 0;

                foreach (var entry in archive.Entries)
                {
                    currentEntry++;
                    int progress = (int)((currentEntry / (float)totalEntries) * 100);
                    splash.UpdateProgress(progress, $"Extraindo e classificando: {entry.Key}");
                    await Task.Delay(100); // Simula um pequeno delay para a UI ser atualizada

                    if (!IsFileValidate(entry, norma))
                        continue;

                    Produto2 prod = norma.GetNewProduct(entry.Key, false);

                    // Criar diretório pai, se não existir
                    string parentDirectory = Path.GetDirectoryName(prod.NewFileName);
                    if (!Directory.Exists(parentDirectory) && !string.IsNullOrEmpty(parentDirectory))
                    {
                        Directory.CreateDirectory(parentDirectory);
                    }
                 
                    entry.WriteToFile(prod.NewFileName, new ExtractionOptions()
                    {
                        Overwrite = true,
                    });       
                }
            }
           // splash.Close();
            return norma;
        }
        private bool IsFileValidate(IArchiveEntry entry, Norma norma)
        {
            if (norma.Produtos.Exists(x => Path.GetFileName(x.InternalFileName) == Path.GetFileName(entry.Key)))
                return false;

            if (entry.IsDirectory)
                return false;

            string[] pastasInvalidas = { "OldVersions", "Design Data", "_V", "Materiais", "Presets", "Templates" };
            string[] arquivosInvalidos = { ".log", ".old", ".bak", ".ipj", ".lck" };

            foreach (string pastaInvalida in pastasInvalidas)
            {
                if (entry.Key.Contains(pastaInvalida))
                    return false;
            }

            foreach (string invalido in arquivosInvalidos)
            {
                if (entry.Key.EndsWith(invalido))
                    return false;
            }
            return true;
        }
        
        
        //public void CreateFirstTable(Produto2 prod)
        //{

        //    int rowIndex = tabela.Rows.Add(TableFormat.Linha(prod));

        //    tabela.Update();
        //    tabela.FirstDisplayedScrollingRowIndex = tabela.RowCount - 1;

        //}
    }


}

