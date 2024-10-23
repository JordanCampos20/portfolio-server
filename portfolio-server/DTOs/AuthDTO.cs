using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace portfolio_server.DTOs
{
    public class AuthDTO
    {

    }

    public class SignUpDTO : AuthDTO
    {
        public string Name1 { get; set; } = null!;
        public string Username1 { get; set; } = null!;
        public string Email1 { get; set; } = null!;
        public string Password1 { get; set; } = null!;
        public string Email2 { get; set; } = null!;
        public string Password2 { get; set; } = null!;
    }

    public class SignInDTO : AuthDTO
    {
        public string Email1 { get; set; } = null!;
        public string Password1 { get; set; } = null!;
        public bool Remember1 { get; set; } = false;
    }

    public class ForgotPasswordDTO : AuthDTO
    {
        public string Email1 { get; set; } = null!;
    }

    public class ForgotPasswordConfirmDTO : AuthDTO
    {
        public string Token1 { get; set; } = null!;
        public string Password1 { get; set; } = null!;
        public string Password2 { get; set; } = null!;
    }
}