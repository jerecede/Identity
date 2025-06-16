using Identity.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Api.Services.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
