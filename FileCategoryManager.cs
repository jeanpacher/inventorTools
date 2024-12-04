using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bosch_ImportData
{
    class FileCategoryManager
    {
        private string libraryIPJPath = Parametros.ProjectEDIT_ATMOLIB; // Example paths, replace with actual path strings
        private string projectIPJPath = Parametros.ProjectVaultFullFileName;

        public string DefineIPJPath(Produto2 product)
        {
            switch (product.Categoria)
            {
                case ProductType.LIBRARY:
                case ProductType.PRODUTOS_BOSCH:
                    return libraryIPJPath;

                case ProductType.NORMA:
                case ProductType.NORMA_AUXILIAR:
                case ProductType.CONTENTCENTER:
                default:
                    return projectIPJPath;
            }
        }

        public bool IsLibrary(Produto2 product)
        {
            return product.Categoria == ProductType.LIBRARY || product.Categoria == ProductType.PRODUTOS_BOSCH;
        }

        public bool IsProject(Produto2 product)
        {
            return product.Categoria == ProductType.NORMA || product.Categoria == ProductType.NORMA_AUXILIAR || product.Categoria == ProductType.CONTENTCENTER;
        }
    }
}
