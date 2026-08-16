using Seatsure.Application.DTOs.Auth;

namespace Seatsure.Application.Services.Interfaces;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
}


// Business rule
// Business -> no generic 
