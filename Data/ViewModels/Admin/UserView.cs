using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invMed.Data
{
    public class UserView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public UserView() { }

        public UserView(AspNetUser user, string role)
        {
            Id = user.Id;
            Name = user.Name;
            Surname = user.Surname;
            UserName = user.UserName;
            Email = user.Email;
            Role = role;
        }

        public UserView(AspNetUser user)
        {
            Id = user.Id;
            Name = user.Name;
            Surname = user.Surname;
            UserName = user.UserName;
            Email = user.Email;
        }
    }
}
