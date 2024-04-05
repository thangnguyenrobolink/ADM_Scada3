using System;

namespace ADM_Scada.Cores.Models
{
    public class ProductionHistoryModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? CustomerId { get; set; }
        public int UserId { get; set; }
        public int Shift { get; set; }

        public float Weight { get; set; }
        public string WO { get; set; }
        public string LOT { get; set; }

        public DateTime TimeStamp { get; set; }
    }

}
