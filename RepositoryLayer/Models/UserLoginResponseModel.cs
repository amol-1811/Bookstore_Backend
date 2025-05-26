using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class UserLoginResponseModel
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}
