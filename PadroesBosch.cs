using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bosch_ImportData
{
    public static class Root
    {
        
        public static void Deserialize()
        {
            string jsonContent = System.IO.File.ReadAllText(Parametros.JsonRegrasDeOrganizacaoDosArquivos);
            ListaPadroesBosch padroes = JsonConvert.DeserializeObject<ListaPadroesBosch>(jsonContent);
            Parametros.padroesBosch = padroes;
        }
    }

    public class ListaPadroesBosch
    {
        public List<PadroesBosch> padroesBosch = new List<PadroesBosch>();

    }

public class PadroesBosch
{
    public string ReferenceString { get; set; }
    public string Norma { get; set; }
    public string RelativePath { get; set; }

}
}
