using System;

namespace ADM_Scada.Core.Models
{
    // UserGroup class representing the user_group table
    public class UserGroupModel
    {
        public int Id { get; set; }
        public string GroupDescription { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
