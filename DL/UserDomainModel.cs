using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class UserDomainModel
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserRoleName { get; set; }

    }
}
