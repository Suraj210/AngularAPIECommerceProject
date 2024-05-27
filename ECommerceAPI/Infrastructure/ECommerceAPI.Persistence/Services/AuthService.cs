using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.Abstractions.Token;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Application.DTOs.Facebook;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Application.Features.Commands.AppUser.LoginUser;
using ECommerceAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace ECommerceAPI.Persistence.Services
{
    public class AuthService : IAuthService
    {

        readonly HttpClient _httpClient;
        readonly IConfiguration _configuration;
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;


        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration,
                           UserManager<Domain.Entities.Identity.AppUser> userManager,
                           ITokenHandler tokenHandler, SignInManager<AppUser> signInManager)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
        }

        public async Task<Token> FacebookLoginAsync(string authToken, int accessToxenLifeTime)
        {
            string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:Client_Id"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:Client_Secret"]}&grant_type=client_credentials");


            FacebookAccessTokenResponse_DTO? facebookAccessTokenResponse = JsonSerializer.Deserialize<FacebookAccessTokenResponse_DTO>(accessTokenResponse);
            string userAccessTokenValidation = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={facebookAccessTokenResponse.AccessToken}");


            FacebookUserAccessTokenValidation_DTO? validation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidation_DTO>(userAccessTokenValidation);

            if (validation?.Data.IsValid != null)
            {
                string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&acces_token{authToken}");

                FacebookUserInfoResponse_DTO? userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponse_DTO>(userInfoResponse);

                UserLoginInfo userLoginInfo = new("FACEBOOK", validation.Data.UserId, "FACEBOOK");
                Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);

                return await CreateUserExternalAsync(user, userInfo.Email, userInfo.Name, userLoginInfo, accessToxenLifeTime);


            }

            throw new Exception("Invalid external authentication.");


        }

        public async Task<Token> GoogleLoginAsync(string idToken, int accessToxenLifeTime)
        {
            ValidationSettings? settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>()
                { _configuration["ExternalLoginSettings:Google:Client_Id"] }
            };

            Payload payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);


            UserLoginInfo userLoginInfo = new("GOOGLE", payload.Subject, "GOOGle");
            Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);
            return await CreateUserExternalAsync(user, payload.Email, payload.Name, userLoginInfo, accessToxenLifeTime);
        }

        public async Task<Token> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime)
        {
            Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(usernameOrEmail);

            if (user == null)
                user = await _userManager.FindByEmailAsync(usernameOrEmail);

            if (user == null)
                throw new NotFoundUserException();


            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded) // Autenthication succedded!
            {
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);
                return token;
            }



            throw new AuthenticationErrorException();

        }

        private async Task<Token> CreateUserExternalAsync(AppUser user, string email, string name, UserLoginInfo info, int accessTokenLifeTime)
        {
            bool result = user != null;

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = email,
                        UserName = email,
                        NameSurname = name
                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }

            if (result)
            {
                await _userManager.AddLoginAsync(user, info);
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);

                return token;
            }

            throw new Exception("Invalid external authentication.");

        }
        
    }
}
