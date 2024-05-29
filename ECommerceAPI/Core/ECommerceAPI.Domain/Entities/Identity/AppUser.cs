using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;

namespace ECommerceAPI.Domain.Entities.Identity
{
    public class AppUser : IdentityUser<string>
    {
        public string NameSurname { get; set; }
        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenEndDate { get; set; }
    }
}
