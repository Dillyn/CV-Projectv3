using Domain.Models;

namespace Integration.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByNameAsync(string name);
    }
}
