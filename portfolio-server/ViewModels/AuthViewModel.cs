using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finance_control_server.ViewModels
{
    public class AuthViewModel(string email1, string token)
    {
        public string Email1 { get; set; } = email1;
        public string Token { get; set; } = token;
    }

    public class TokenViewModel
    {
        public bool Authenticated { get; set; }
        public DateTime Expiration { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}