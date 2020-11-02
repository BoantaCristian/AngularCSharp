using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAPI.Models.ViewModels
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Seniority { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
