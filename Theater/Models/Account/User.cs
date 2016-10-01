using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Theater.Models.Account
{
    public enum Role
    {
        Customer = 0,
        Сourier = 1
    }
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string Password { get; set; }

        public User()
        { }

        public User(int id, string name, string password, int roleId, string email, string phone)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Email = email;
            Role = (Role)roleId;
            Password = password;
        }
    }
}