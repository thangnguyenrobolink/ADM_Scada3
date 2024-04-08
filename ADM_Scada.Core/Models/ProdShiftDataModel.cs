using System;

namespace ADM_Scada.Core.Models
{
    // ProdShiftData class representing the prod_shift_data table
    public class ProdShiftDataModel
    {
        public int Id { get; set; }
        public string WorkOrderNo { get; set; }
        public string ProdCode { get; set; }
        public string LotNo { get; set; }
        public DateTime? ProductionDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string UserName { get; set; }
        public string ShiftNo { get; set; }
        public string CustCode { get; set; }
        public string DevideCode { get; set; }
        public decimal? QtyToPack { get; set; }
        public string WholeUom { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
