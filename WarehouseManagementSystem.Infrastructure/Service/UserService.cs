using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagementSystem.Application.DTOs;
using WarehouseManagementSystem.Application.Interface;
using WarehouseManagementSystem.Domain.Model;

namespace WarehouseManagementSystem.Infrastructure.Service;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<UserDto>> GetAllUsersAsync(string currentUserId)
    {
        var users = await _userManager.GetUsersInRoleAsync("User");
        return users
            .Where(u => u.Id != currentUserId)
            .Select(u => new UserDto
        {
            Id = u.Id,
            FullName = $"{u.FirstName} {u.LastName}",
            Email = u.Email
        }).ToList();

    }
}
