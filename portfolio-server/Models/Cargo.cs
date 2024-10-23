using Microsoft.AspNetCore.Identity;

namespace portfolio_server.Models
{
    public class Cargo : IdentityRole
    {
        public Cargo()
        {
        
        }
        public Cargo(string name)
        {
            Id = name;
            Name = name;
            NormalizedName = name.ToUpper();
        }
    }
}