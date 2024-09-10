using System.Text.Json.Serialization;

namespace User.Application.Dtos
{
    public class AuthenticationResponseDto
    {
        public AuthenticationResponseDto(string token, string refreshToken, string name)
        {
            Token = token;
            RefreshToken = refreshToken;
            Name = name;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Token { get; private set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RefreshToken { get; private set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Name { get; private set; }

    }
}
