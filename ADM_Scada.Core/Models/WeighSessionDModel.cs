using System;

namespace ADM_Scada.Core.Models
{
    // WeighSessionD class representing the weigh_session_d table
    public class WeighSessionDModel
    {
        public int Id { get; set; }
        public string SessionCode { get; set; }
        public decimal? CurrentWeigh { get; set; }
        public string Barcode { get; set; }
        public string ProdCode { get; set; }
        public string ProdFullName { get; set; }
        public string ProdD365Code { get; set; }
        public DateTime? ProductionDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? QtyCounted { get; set; }
        public decimal? QtyWeighed { get; set; }
        public decimal? Gap { get; set; }
        public int? ShiftDataId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
