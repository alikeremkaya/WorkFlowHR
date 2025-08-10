
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using WorkFlowHR.Application.DTOs.AppUserDTOs;
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Domain.Utilities.Concretes;
using WorkFlowHR.Domain.Utilities.Interfaces;

using WorkFlowHR.Infrastructure.Repositories.AppUserRepositories;

namespace WorkFlowHR.Application.Services.AppUserServices
{
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository _repo;
        private readonly string[] _managerEmails;
        private readonly ILogger<AppUserService> _logger;

        public AppUserService(
            IAppUserRepository repo,
            IConfiguration config,
            ILogger<AppUserService> logger)
        {
            _repo = repo;
            _managerEmails = config
                .GetSection("Authorization:ManagerEmails")
                .Get<string[]>() ?? Array.Empty<string>();
            _logger = logger;
        }

        public async Task<IDataResult<List<AppUserListDTO>>> GetAllAsync()
        {
            try
            {
                var entities = (await _repo.GetAllAsync(u => true, tracking: false)).ToList();
                var dtos = entities.Adapt<List<AppUserListDTO>>();
                return new SuccessDataResult<List<AppUserListDTO>>(dtos, "Users retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all users");
                return new ErrorDataResult<List<AppUserListDTO>>("Error fetching users: " + ex.Message);
            }
        }

        public async Task<IDataResult<AppUserDTO>> CreateAsync(AppUserCreateDTO createDto)
        {
            try
            {
                var entity = createDto.Adapt<AppUser>();
                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync();
                var dto = entity.Adapt<AppUserDTO>();
                return new SuccessDataResult<AppUserDTO>(dto, "User created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return new ErrorDataResult<AppUserDTO>("Error creating user: " + ex.Message);
            }
        }

        public async Task<IDataResult<AppUserDTO>> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return new ErrorDataResult<AppUserDTO>("User not found.");
            var dto = entity.Adapt<AppUserDTO>();
            return new SuccessDataResult<AppUserDTO>(dto, "User retrieved successfully.");
        }

        public async Task<IResult> UpdateAsync(AppUserUpdateDTO updateDto)
        {
            try
            {
                var entity = await _repo.GetByIdAsync(updateDto.Id);
                if (entity == null)
                    return new ErrorResult("User not found.");

                updateDto.Adapt(entity);
                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync();
                return new SuccessResult("User updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                return new ErrorResult("Error updating user: " + ex.Message);
            }
        }

        public async Task<IDataResult<AppUserDTO>> EnsureAppUserAsync(ClaimsPrincipal user)
        {
            try
            {
                var profile = user.Adapt<AppUser>();
                profile.Role = _managerEmails.Contains(profile.Email, StringComparer.OrdinalIgnoreCase)
                    ? "Manager"
                    : "Employee";

                var existing = (await _repo.GetAllAsync(u => u.Email == profile.Email, tracking: true))
                                   .FirstOrDefault();
                if (existing == null)
                {
                    await _repo.AddAsync(profile);
                }
                else
                {
                    profile.Id = existing.Id;
                    existing = profile.Adapt(existing);
                    await _repo.UpdateAsync(existing);
                    profile = existing;
                }

                await _repo.SaveChangesAsync();
                var dto = profile.Adapt<AppUserDTO>();
                return new SuccessDataResult<AppUserDTO>(dto, "User profile ensured.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ensuring user profile");
                return new ErrorDataResult<AppUserDTO>("Error ensuring user: " + ex.Message);
            }
        }

        public async Task<IDataResult<string>> GetRoleAsync(string email)
        {
            var existing = (await _repo.GetAllAsync(u => u.Email == email, tracking: false))
                               .FirstOrDefault();
            if (existing != null)
                return new SuccessDataResult<string>(existing.Role);

            var role = _managerEmails.Contains(email, StringComparer.OrdinalIgnoreCase)
                ? "Manager"
                : "Employee";
            return new SuccessDataResult<string>(role);
        }

        public async Task<IDataResult<AppUserDTO>> GetByEmailAsync(string email)
        {
            var existing = (await _repo.GetAllAsync(u => u.Email == email, tracking: false))
                               .FirstOrDefault();
            if (existing == null)
                return new ErrorDataResult<AppUserDTO>("User not found.");

            var dto = existing.Adapt<AppUserDTO>();
            return new SuccessDataResult<AppUserDTO>(dto, "User retrieved by email.");
        }
    }
}





