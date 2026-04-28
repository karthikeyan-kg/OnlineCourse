using UserService.Domain.Entities;

namespace UserService.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByUsername(string username);
    }
}
