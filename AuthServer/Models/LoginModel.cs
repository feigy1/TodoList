using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Models
{
    public class LoginModel
    {
        public string Name { get; set; }

        public string Password { get; set; }
    }
}
