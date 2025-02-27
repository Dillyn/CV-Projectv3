using Domain.Models;

namespace Integration.Repositories
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<UserEntity?> GetByNameAsync(string name);
    }
}
