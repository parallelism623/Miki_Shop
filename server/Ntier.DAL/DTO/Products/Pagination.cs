using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntier.DAL.DTO.Products
{
    public class Pagination
    {
        public int _page { get; set; }
        public int _limit { get; set; }
        public int _totalRows { get; set; }
    }
}
