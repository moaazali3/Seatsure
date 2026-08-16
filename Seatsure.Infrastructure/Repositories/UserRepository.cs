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
        var result = await _context.Users.Where(e=> e.Id == id).FirstOrDefaultAsync();

        return result;
            
     }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var result = await _context.Users.Where(e=> e.Email == email).FirstOrDefaultAsync();
        return result;

    }

    public async Task AddAsync(User user)
    {
      await  _context.Users.AddAsync(user);
        await SaveChangesAsync();
            
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
