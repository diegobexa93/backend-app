using System.Text.Json.Serialization;

namespace User.Application.Dtos
{
    public class AuthenticationResponseDto
    {
        public AuthenticationResponseDto(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Token { get; private set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RefreshToken { get; private set; }

    }
}
