using BaseShare.Common.Exceptions;
using BaseShare.Common.Results;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using User.Application.Contracts;
using User.Application.Dtos;
using User.Application.Errors;
using User.Domain.Contracts.Infrastructure;
using User.Domain.Entities;
using User.Domain.Security;


namespace User.Application.Services
{
    public class AuthenticationService(IOptions<JwtOptions> _jwtOptions,
                                       IJwtService _jwtService,
                                       IUserRepository _userRepository): IAuthenticationService
    {
        public async Task<Result<AuthenticationResponseDto>> Authenticate(AuthenticationRequestDto request)
        {
            var errors = ValidateAuthentication(request);

            if (errors.Any())
            {
                return Result.Failure<AuthenticationResponseDto>(new ValidationErrors(errors));
            }

            var userDb = await _userRepository.GetByEmailAsync(request.Login!);

            if (userDb is null)
                return Result.Failure<AuthenticationResponseDto>(AuthenticateError.UserNotFound);


            if (!BCrypt.Net.BCrypt.Verify(request.Password, userDb.Password))
                return Result.Failure<AuthenticationResponseDto>(AuthenticateError.UserNotFound);

            var tokenAccess = await GenerateAccessToken(userDb);
            var tokenRefresh = await GenerateRefreshToken(userDb);

            return new AuthenticationResponseDto(tokenAccess, tokenRefresh, userDb.Person!.Name);
        }

        private static Error[] ValidateAuthentication(AuthenticationRequestDto request)
        {
            List<Error> errors = [];

            if (string.IsNullOrWhiteSpace(request.Login))
                errors.Add(AuthenticateError.LoginEmpty);

            if (string.IsNullOrWhiteSpace(request.Password))
                errors.Add(AuthenticateError.PasswordEmpty);

            return [.. errors];
        }

        private async Task<string> GenerateAccessToken(UserObj user)
        {
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaim(new Claim(type: Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub,
                                              value: user.Person!.Id.ToString()));

            if (!string.IsNullOrEmpty(user.Person.Email))
            {
                identityClaims.AddClaim(new Claim(type: Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Email,
                                              value: user.Person.Email));
            }

            identityClaims.AddClaim(new Claim(type: Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti,
                                              value: Guid.NewGuid().ToString()));

            identityClaims.AddClaim(new Claim("User", "AllowedUser"));


            var handler = new JwtSecurityTokenHandler();
            var key = await _jwtService.GetCurrentSigningCredentials();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Value.Issuer,
                Audience = _jwtOptions.Value.Audience,
                SigningCredentials = key,
                Subject = identityClaims,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(60),
                IssuedAt = DateTime.UtcNow,
                TokenType = "at+jwt"
            });

            var encodedJwt = handler.WriteToken(securityToken);
            return encodedJwt;

        }

        private async Task<string> GenerateRefreshToken(UserObj user)
        {
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaim(new Claim(type: Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub,
                                              value: user.Person!.Id.ToString()));


            identityClaims.AddClaim(new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            // Refresh tokens usually have a longer expiration time.
            var handler = new JwtSecurityTokenHandler();
            var key = await _jwtService.GetCurrentSigningCredentials();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Value.Issuer,
                Audience = _jwtOptions.Value.Audience,
                SigningCredentials = key,
                Subject = identityClaims,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(1),
                IssuedAt = DateTime.UtcNow,
                TokenType = "rt+jwt"
            });

            var encodedJwt = handler.WriteToken(securityToken);
            return encodedJwt;
        }
    }
}
