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
        public int UserGroup { get; set; }
        public string EmailAddress { get; set; }
        public string TelNo { get; set; }
        public string MobileNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsEnable { get; set; }

        public UserModel(int id, string userCode, string userName, string password, string userAvatar, int userGroup,
                         string emailAddress, string telNo, string mobileNo, DateTime? createdDate, string createdBy,
                         DateTime? updatedDate, string updatedBy, bool isEnable)
        {
            Id = id;
            UserCode = userCode;
            UserName = userName;
            Password = password;
            UserAvatar = userAvatar;
            UserGroup = userGroup;
            EmailAddress = emailAddress;
            TelNo = telNo;
            MobileNo = mobileNo;
            CreatedDate = createdDate;
            CreatedBy = createdBy;
            UpdatedDate = updatedDate;
            UpdatedBy = updatedBy;
            IsEnable = isEnable;
        }
        public UserModel()
        {
            Id = 0;
            UserCode = "000";
            UserName = "Guest";
            Password = "";
            UserAvatar = "Avatar.jpeg";
            UserGroup = 1;
            EmailAddress = "guest@company.com";
            TelNo = "8400000001";
            MobileNo = "8400000001";
            CreatedDate = DateTime.Now;
            CreatedBy = "Guest";
            UpdatedDate = DateTime.Now;
            UpdatedBy = "Guest";
            IsEnable = false;
        }
    }
}