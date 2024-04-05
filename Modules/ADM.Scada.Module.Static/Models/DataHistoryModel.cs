using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADM.Scada.Modules.Static.Models
{
    public class DataHistoryModel
    {
        public int Id { get; set; }
        public string DataName { get; set; }
        public string Value { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
