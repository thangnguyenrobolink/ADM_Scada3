using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADM_Scada.Cores.Model
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Company { get; set; }
        public string Code { get; set; }
        public DateTime TimeStamp { get; set; }
    }

}
