using System;

namespace ADM_Scada.Core.Models
{
    // Customer class representing the customer table
    public class CustomerModel
    {
        public override string ToString()
        {
            return $"{CustCode} {CustName}";
        }
        public int Id { get; set; }
        public string CustCode { get; set; }
        public string CustName { get; set; }
        public string CustCompany { get; set; }
        public string CustAvatar { get; set; }
        public string CustAdd { get; set; }
        public string PaymentTerm { get; set; }
        public string EmailAddress { get; set; }
        public string FaxNo { get; set; }
        public string TelNo { get; set; }
        public string MobileNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            CustomerModel other = (CustomerModel)obj;
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        public CustomerModel()
        {
            CustCode = "C123";
            CustName = "Example Customer";
            CustCompany = "Guest";
            CustAvatar = "9723582.jpg";
            CustAdd = "123 Example St, City";
            PaymentTerm = "Net 30";
            EmailAddress = "example@example.com";
            FaxNo = "123-456-7890";
            TelNo = "987-654-3210";
            MobileNo = "555-555-5555";
            CreatedDate = DateTime.Now;
            CreatedBy = "Admin";
            UpdatedDate = DateTime.Now; // Not updated yet
            UpdatedBy = "Guset"; // Not updated yet
        }
    }
}
