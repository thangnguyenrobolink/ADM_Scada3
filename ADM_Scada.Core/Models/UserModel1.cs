using System;

namespace ADM_Scada.Core.Models
{
    // User class representing the user table
    public class UserModel
    {
        public int Id { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserAvatar { get; set; }
        public string UserGroup { get; set; }
        public string EmailAddress { get; set; }
        public string TelNo { get; set; }
        public string MobileNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsEnable { get; set; }
    }
}
