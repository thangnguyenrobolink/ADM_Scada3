using System;

namespace ADM_Scada.Core.Models
{
    // Variable class representing the variable table
    public class VariableModel
    {
        public int Id { get; set; }
        public int? DeviceId { get; set; }
        public int? Type { get; set; }
        public int? Area { get; set; }
        public int? Address { get; set; }
        public string Name { get; set; }
        public string Module { get; set; }
        public string Unit { get; set; }
        public string Message { get; set; }
        public float? Value { get; set; }
        public string Purpose { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
