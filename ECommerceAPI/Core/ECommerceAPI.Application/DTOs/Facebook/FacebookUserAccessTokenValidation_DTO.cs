using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Facebook
{
    public class FacebookUserAccessTokenValidation_DTO
    {
        [JsonPropertyName("data")]
        public FacebookUserAccessTokenValidation_DTOData Data { get; set; }
    }

    public class FacebookUserAccessTokenValidation_DTOData
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
        [JsonPropertyName("is_valid")]
        public bool IsValid { get; set; }
    }
}
