using Inventor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventor;


namespace Bosch_ImportData
{
    public class CheckInManager
    {
        private readonly Inventor.Application _invApp;
        private readonly FileCategoryManager _categoryManager;
        private readonly ProductValidator _validator;

        public CheckInManager(Inventor.Application invApp)
        {
            _invApp = invApp;
            _categoryManager = new FileCategoryManager();
            _validator = new ProductValidator();
        }

        public void ProcessCheckIn(List<Produto2> produtos)
        {
            // Separar bibliotecas e projetos e realizar validação
            var bibliotecas = produtos.Where(p => _categoryManager.IsLibrary(p)); // && _validator.IsReadyForCheckIn(p)).ToList();
            var projetos = produtos.Where(p => _categoryManager.IsProject(p)); // && _validator.IsReadyForCheckIn(p)).ToList();

            // Check-in para bibliotecas
            foreach (var produto in bibliotecas)
            {
                string ipjPath = _categoryManager.DefineIPJPath(produto);
                AbrirArquivoParaCheckIn(produto, ipjPath);
            }

            // Check-in para projetos
            foreach (var produto in projetos)
            {
                string ipjPath = _categoryManager.DefineIPJPath(produto);
                AbrirArquivoParaCheckIn(produto, ipjPath);
            }
        }

        private void AbrirArquivoParaCheckIn(Produto2 produto, string ipjPath)
        {
            //_invApp.DesignProjectManager.DesignProject = _invApp.DesignProjectManager.DesignProjects.ItemByPath(ipjPath);

            Document doc = _invApp.Documents.Open(produto.NewFileName);
            _invApp.CommandManager.ControlDefinitions["VaultCheckIn"].Execute();

            doc.Close(true);
        }
    }
}
