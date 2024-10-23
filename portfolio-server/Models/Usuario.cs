using Microsoft.AspNetCore.Identity;

namespace portfolio_server.Models
{
    public class Usuario : IdentityUser
    {
        public required string Nome { get; set; }
        public required string Imagem { get; set; }
    }
}