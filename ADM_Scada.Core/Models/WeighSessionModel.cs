using System;

namespace ADM_Scada.Core.Models
{
    // WeighSession class representing the weigh_session table
    public class WeighSessionModel
    {
        public int Id { get; set; }
        public string SessionCode { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? CustId { get; set; }
        public string CustName { get; set; }
        public string CustAddress { get; set; }
        public string BoatId { get; set; }
        public string SoNumber { get; set; }
        public int? QtyCounted { get; set; }
        public decimal? QtyWeighed { get; set; }
        public decimal? QtyTareWeigh { get; set; }  
        public decimal? QtyGoodWeigh { get; set; }
        public decimal? QtyOrderWeigh { get; set; }
        public decimal? Gap { get; set; }
        public string DocumentNo { get; set; }
        public int? ShiftDataId { get; set; }
        public int? UserId { get; set; }
        public string DeviceCode { get; set; }
        public string StatusCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
