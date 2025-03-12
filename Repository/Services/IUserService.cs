using Domain.Models;

namespace Services.Services
{
    internal interface IUserService : IServices<UserEntity>
    {
        Task<UserEntity> GetByFirstLetterOfUserName(char letter);
    }
}
