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
        public int? BoatId { get; set; }
        public string SoNumber { get; set; }
        public int? QtyCounted { get; set; }
        public float? QtyOrderWeigh { get; set; }
        public float? QtyTareWeigh { get; set; }
        public float? QtyWeighed { get; set; }
        public float? QtyInvoiceWeigh { get; set; }
        public float? Gap { get; set; }
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
