using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Bosch_ImportData
{
    public  class ZipfileManipulate
    {
        public Norma norma { get; set; }
        public Norma ExtractZip(string zipFilePath)
        {
            norma = new Norma();
            // Obter o nome do arquivo sem a extensão
            norma.CodigoNorma = Path.GetFileNameWithoutExtension(zipFilePath);
            // Criar a pasta de destino (se não existir)
            string destinationDirectory = Properties.Settings.Default.tempVaultRootPath;
            // Extrair arquivos do ZIP para a pasta de destino
            using (ZipArchive zipArchive = ZipFile.OpenRead(zipFilePath))
            {
                foreach (ZipArchiveEntry entry in zipArchive.Entries)
                {
                    if (entry.FullName.Contains("OldVersions") || entry.FullName.Contains("_V"))
                    { continue; }
                    string[] invalidos = { ".log", ".old", ".bak", ".ipj" };
                    bool isInvalid = false;
                    foreach (string invalido in invalidos)
                    {
                        if (entry.FullName.EndsWith(invalido))
                        {
                            isInvalid = true;
                            break;
                        }
                    }
                    if (isInvalid) { continue; }
                    if (entry.FullName.EndsWith("/") || entry.FullName.EndsWith("\\"))
                    {
                        if (!entry.FullName.Contains("OldVersions"))
                        { continue; }
                        // Criar diretório
                        //Directory.CreateDirectory(entryPath);
                    }
                    else
                    {
                        Produto prod = new Produto(entry.FullName, norma.CodigoNorma);
                        // Criar diretório pai, se não existir
                        string parentDirectory = Path.GetDirectoryName(prod.NewFileName);
                        if (!Directory.Exists(parentDirectory) && !string.IsNullOrEmpty(parentDirectory))
                        {
                            Directory.CreateDirectory(parentDirectory);
                        }
                        norma.Produtos.Add(prod);
                        entry.ExtractToFile(prod.NewFileName, true);
                    }
                }
            }
            return norma;
        }
    }
}
