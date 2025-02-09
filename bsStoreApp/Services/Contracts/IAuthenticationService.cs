using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

/*
<summary>
    populateExp ifadesi true gelirse refresh token süresi uzatılacak. false gelirse süreye dokunulmayacak.
</summary>
*/

namespace Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto);
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuthDto);
        Task<TokenDto> CreateToken(bool populateExp);
        Task<TokenDto> RefreshToken(TokenDto tokenDto);
    }
}
