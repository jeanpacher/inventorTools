using System;
using System.Runtime.InteropServices;


namespace Bosch_ImportData
{
    static class InvApp
    {
        public static Inventor.Application inventorApp = null;
        public static void CreateInventorInstance()
        {

            try
            {
                // Tenta pegar a instância do Inventor em execução
                inventorApp = (Inventor.Application)Marshal.GetActiveObject("Inventor.Application");
            }
            catch (COMException)
            {
                // Se não houver uma instância em execução, cria uma nova
                inventorApp = (Inventor.Application)Activator.CreateInstance(Type.GetTypeFromProgID("Inventor.Application"));
            }

            // Verifica se a instância do Inventor foi obtida
            //if (inventorApp != null)
            //{
            //    // Faz algo com a instância do Inventor (por exemplo, abrir um documento)
            //    inventorApp.Documents.Open(@"C:\Users\Jean\Desktop\Teste\4729111862\4729111862\Workspaces\Arbeitsbereich\CtP_TEF\project\4729111862\4729111862_P002.ipt");

            //    // Fecha a instância do Inventor quando terminar
            //    inventorApp.Quit();
            //}
        }

    }
}


