using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADM_Scada.Cores.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public bool IsEnable { get; set; }
    }
}
