using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntier.DAL.DTO.Products
{
    public class StockDTO
    {
        public int? sizeId { get; set; }
        public int quantity { get; set; }
        public int Price { get; set; }
    }
}
