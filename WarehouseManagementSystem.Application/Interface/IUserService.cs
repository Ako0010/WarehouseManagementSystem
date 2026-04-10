

using WarehouseManagementSystem.Application.DTOs;

namespace WarehouseManagementSystem.Application.Interface;

public interface IUserService
{
    public Task<List<UserDto>> GetAllUsersAsync(string currentUserId);
}
