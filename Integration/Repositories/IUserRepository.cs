using Domain.Models;

namespace Integration.Repositories
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<List<UserEntity?>> GetByNameAsync(string name);
    }
}
