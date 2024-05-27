using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Abstractions.Services.Authentications
{
   public interface IExternalAuthentication
    {
        Task<DTOs.Token> FacebookLoginAsync(string authToken,int accessToxenLifeTime);
        Task<DTOs.Token> GoogleLoginAsync(string idToken, int accessToxenLifeTime);
    }
}
