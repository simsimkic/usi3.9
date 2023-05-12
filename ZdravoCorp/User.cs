using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public enum Role
    {
        ADMIN,
        DOCTOR,
        NURSE,
        PATIENT,
        MANAGER
    }
    public class User
    {
        public String Username { get; set; }
        public String Password { get; set; }
        public Role Role { get; set; }
        public User(String Username, String Password, Role Role) 
        {
            this.Username = Username;
            this.Password = Password;
            this.Role = Role;
        }
        public User(String Username, String Password)
        {
            this.Username = Username;
            this.Password = Password;
        }
        public override bool Equals(object? obj)
        {
            return obj is User user &&
                   Username == user.Username &&
                   Password == user.Password;
        }

    }
    
}
