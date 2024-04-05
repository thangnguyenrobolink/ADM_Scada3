using System;

namespace ADM_Scada.Core.Models
{
    // Customer class representing the customer table
    public class CustomerModel
    {
        public int Id { get; set; }
        public string CustCode { get; set; }
        public string CustName { get; set; }
        public string CustAvatar { get; set; }
        public string CustAddress { get; set; }
        public string PaymentTerm { get; set; }
        public string EmailAddress { get; set; }
        public string FaxNo { get; set; }
        public string TelNo { get; set; }
        public string MobileNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
