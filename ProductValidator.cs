using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bosch_ImportData
{
    public class ProductValidator
    {
        // Verificação de nome e presença de propriedades obrigatórias, além de validações específicas para ATMOLIB
        public bool IsValidName(Produto2 product)
        {
            return !string.IsNullOrWhiteSpace(product.NewFileName) && product.NewFileName.Length > 3;
        }

        public bool HasRequiredProperties(Produto2 product)
        {
            return true;
            //return product.propriedades != null && product.propriedades.ContainsKey("RequiredPropertyKey");
        }

        //public bool HasATMOLIBCustomProperties(Produto2 product)
        //{
        //    if (product.Categoria != ProductType.LIBRARY) return true;

        //    // Checa se as três propriedades estão presentes para ATMOLIB
        //    var requiredProperties = new[] { "RBGBDETAILS", "RBGBPRODUCERNAME", "RBGBPRODUCERORDERNO" };
        //    return requiredProperties.All(prop => product.propriedades);
            
                    
        //}

        //public bool IsReadyForCheckIn(Produto2 product)
        //{
        //    return IsValidName(product) && HasRequiredProperties(product) && HasATMOLIBCustomProperties(product) && !product.isMissing;
        //}
    }
}
