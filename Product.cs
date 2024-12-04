using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventor;

namespace Bosch_ImportData
{
    public class Item
    {
        public int Nivel { get; set; }
        public Product Produto { get; set; }
        public List<Item> Itens { get; set; }

    }


    public class Product
    {
        public string FileName { get; set; }
        public Document Document { get; set; }
        public InventorProperties InventorProperties { get; set; }

        public Product(Document _document)
        {
            Document = _document;
        }

    }

    public class InventorProperties
    {

    }
}
