using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class UserRepository
    {
        public List<User> _users {get; set;}
        private string filePath = "../../../Users.csv";
        public UserRepository() {
             _users = new List<User>();
            ReadUsers();
        }
        public void AddUser(User user)
        {
            _users.Add(user);
            WriteUsers();
        }
        public bool HasUser(User user)
        {
            foreach (User existingUser in _users)
            {
                if (existingUser.Equals(user))
                {
                    ((App)App.Current).SetLoggedInUser(existingUser);
                    return true;
                }
            }
            return false;
        }
        public void ReadUsers()
        {
            StreamReader reader;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }
            reader = new StreamReader(filePath);    
            while(!reader.EndOfStream) {
                parseLine(ref reader);
            }
        }
        private void parseLine(ref StreamReader reader)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(",");
            lineToObject(values);
        }
        private void lineToObject(string[] values)
        {
            _users.Add(new User(values[0], values[1], (Role)Enum.Parse(typeof(Role), values[2])));
        }

        private string userToCSV(User user)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(user.Username);
            sb.Append(',');
            sb.Append(user.Password);
            sb.Append(',');
            sb.Append(user.Role);
            return sb.ToString();
        }

        public void WriteUsers()
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);
            StreamWriter writer = File.CreateText(filePath);
        
            string line;
            foreach (User user in _users)
            {
                writer.Write(userToCSV(user));
                writer.Write("\n");
            }
            writer.Close();
        }
    }
}
