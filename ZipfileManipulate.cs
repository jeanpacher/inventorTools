
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpCompress;
using Bosch_ImportData;
using SharpCompress.Archives;
using SharpCompress.Common;



namespace Bosch_ImportData
{
    public class ZipfileManipulate
    {
        private DataGridView tabela;
        public Norma norma { get; set; }
        public Dictionary<string, Produto> dicionarioProdutos = new Dictionary<string, Produto>();

        public ZipfileManipulate(DataGridView _tabela)
        {
            tabela = _tabela;
        }
        public Norma ExtractZip(string zipFilePath)
        {
            norma = new Norma();
            // Obter o nome do arquivo sem a extensão
            norma.CodigoNorma = Path.GetFileNameWithoutExtension(zipFilePath);
            // Criar a pasta de destino (se não existir)
            string destinationDirectory = Properties.Settings.Default.tempVaultRootPath;
            // Extrair arquivos do ZIP para a pasta de destino

            using (var archive = ArchiveFactory.Open(zipFilePath))
            {
                foreach (var entry in archive.Entries)
                {
                    bool isInvalid = false;
                    string[] pastasInvalidas = { "OldVersions", "Design Data", "_V", "Materiais", "Presets", "Templates" };
                    string[] arquivosInvalidos = { ".log", ".old", ".bak", ".ipj", ".lck" };

                    if (dicionarioProdutos.ContainsKey(Path.GetFileName(entry.Key)))
                        continue;


                    foreach (string pastaInvalida in pastasInvalidas)
                    {
                        if (entry.Key.Contains(pastaInvalida))
                        {
                            isInvalid = true;
                            break;
                        }
                    }

                    foreach (string invalido in arquivosInvalidos)
                    {
                        if (entry.Key.EndsWith(invalido))
                        {
                            isInvalid = true;
                            break;
                        }
                    }
                    if (isInvalid) { continue; }


                    if (entry.IsDirectory)
                        continue;



                    Produto prod = new Produto(entry.Key, norma.CodigoNorma);
                    norma.Produtos.Add(prod);
                    dicionarioProdutos.Add(Path.GetFileName(entry.Key), prod);


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

                    CreateFirstTable(prod);

                }
            }
            return norma;
        }

        public void CreateFirstTable(Produto prod)
        {
            int rowIndex = tabela.Rows.Add(prod.Type.ToString(),
                    prod.isVaultExisting,
                    $"{Path.GetFileName(prod.NewFileName)}",
                    "SUBSTITUIR",
                    prod.NewFileName);

            if (prod.isMissing)
                tabela.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;

            tabela.Update();
            tabela.FirstDisplayedScrollingRowIndex = tabela.RowCount - 1;

            // Atualizar a interface do usuário para refletir a mudança imediatamente

        }
    }
}

//public Norma ExtractZip(string zipFilePath)
//{
//    norma = new Norma();
//    // Obter o nome do arquivo sem a extensão
//    norma.CodigoNorma = Path.GetFileNameWithoutExtension(zipFilePath);
//    // Criar a pasta de destino (se não existir)
//    string destinationDirectory = Properties.Settings.Default.tempVaultRootPath;
//    // Extrair arquivos do ZIP para a pasta de destino

//    using (ZipArchive zipArchive = ZipFile.OpenRead(zipFilePath))
//    {
//        foreach (ZipArchiveEntry entry in zipArchive.Entries)
//        {
//            bool isInvalid = false;
//            string[] pastasInvalidas = { "OldVersions", "Design Data", "_V", "Materiais", "Presets", "Templates" };
//            string[] arquivosInvalidos = { ".log", ".old", ".bak", ".ipj" };


//            foreach (string pastaInvalida in pastasInvalidas)
//            {
//                if (entry.FullName.Contains(pastaInvalida))
//                {
//                    isInvalid = true;
//                    break;
//                }
//            }

//            foreach (string invalido in arquivosInvalidos)
//            {
//                if (entry.FullName.EndsWith(invalido))
//                {
//                    isInvalid = true;
//                    break;
//                }
//            }
//            if (isInvalid) { continue; }


//            if (entry.FullName.EndsWith("/") || entry.FullName.EndsWith("\\"))
//            {
//                if (!entry.FullName.Contains("OldVersions"))
//                { continue; }
//            }
//            else
//            {
//                Produto prod = new Produto(entry.FullName, norma.CodigoNorma);
//                // Criar diretório pai, se não existir
//                string parentDirectory = Path.GetDirectoryName(prod.NewFileName);
//                if (!Directory.Exists(parentDirectory) && !string.IsNullOrEmpty(parentDirectory))
//                {
//                    Directory.CreateDirectory(parentDirectory);
//                }
//                norma.Produtos.Add(prod);
//                entry.ExtractToFile(prod.NewFileName, true);

//                CreateFirstTable(prod);
//            }
//        }
//    }
//    return norma;
//}