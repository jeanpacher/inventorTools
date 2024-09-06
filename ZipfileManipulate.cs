﻿using System.IO;
using System.Linq;
using System.Windows.Forms;
using SharpCompress.Archives;
using SharpCompress.Common;



namespace Bosch_ImportData
{
    public class ZipfileManipulate
    {
        private DataGridView tabela { get; set; }
        public ImageList ImageList { get; set; }
        public ZipfileManipulate(DataGridView _tabela, ImageList _imageList)
        {
            tabela = _tabela;
            ImageList = _imageList;
        }

        public Norma ExtractZip(string zipFilePath)
        {
            Norma norma = new Norma()
            {
                CodigoNorma = Path.GetFileNameWithoutExtension(zipFilePath),
            };

            using (var archive = ArchiveFactory.Open(zipFilePath))
            {
                foreach (var entry in archive.Entries)
                {
                    if (!IsFileValidate(entry, norma))
                        continue;
                    Produto prod = norma.GetNewProduct(entry.Key, false);
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
        private bool IsFileValidate(IArchiveEntry entry, Norma norma)
        {
            if (norma.Produtos.Exists(x => Path.GetFileName(x.Filename) == Path.GetFileName(entry.Key)))
                return false;

            //if (dicionarioProdutos.ContainsKey(Path.GetFileName(entry.Key)))
            //    return false;

            if (entry.IsDirectory)
                return false;

            string[] pastasInvalidas = { "OldVersions", "Design Data", "_V", "Materiais", "Presets", "Templates" };
            string[] arquivosInvalidos = { ".log", ".old", ".bak", ".ipj", ".lck" };

            foreach (string pastaInvalida in pastasInvalidas)
            {
                if (entry.Key.Contains(pastaInvalida))
                {
                    return false;
                }
            }

            foreach (string invalido in arquivosInvalidos)
            {
                if (entry.Key.EndsWith(invalido))
                {
                    return false;
                }
            }

            return true;
        }
        public void CreateFirstTable(Produto prod)
        {
            string displayName = TableFormat.GetDisplayName(prod);

            int rowIndex = tabela.Rows.Add(
                     prod.Type.ToString(),
                       prod.isVaultExisting,
                       ImageList.Images[prod.IconName],
                       Path.GetDirectoryName(prod.NewFileName).Split('\\').Last(),
                       Path.GetFileName(prod.NewFileName),
                       "RENOMEAR",
                       prod.isAssemblyParticipant,
                       prod.NewFileName);

            tabela.Update();
            tabela.FirstDisplayedScrollingRowIndex = tabela.RowCount - 1;

        }
    }
}

