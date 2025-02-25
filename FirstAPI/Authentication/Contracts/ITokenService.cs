using Domain.Models;

namespace FirstAPI.Authentication.Contracts
{
    public interface ITokenService
    {
        string CreateToken(UserI user);

    }
}
