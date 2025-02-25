using Domain.Models;

namespace Services.Services
{
    internal interface IUserService : IServices<User>
    {
        Task<User> GetByFirstLetterOfUserName(char letter);
    }
}
