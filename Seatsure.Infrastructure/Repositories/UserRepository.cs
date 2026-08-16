using Microsoft.EntityFrameworkCore;
using Seatsure.Application.Repositories;
using Seatsure.Domain;

namespace Seatsure.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        // TODO: Implement GetByIdAsync using DbContext
        throw new NotImplementedException();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        // TODO: Implement GetByEmailAsync using DbContext
        throw new NotImplementedException();
    }

    public async Task AddAsync(User user)
    {
        // TODO: Implement AddAsync using DbContext
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync()
    {
        // TODO: Implement SaveChangesAsync using DbContext
        throw new NotImplementedException();
    }
}
