using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADM_Scada.Cores.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Market { get; set; }
        public string Ingredient { get; set; }
        public int Exp { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
