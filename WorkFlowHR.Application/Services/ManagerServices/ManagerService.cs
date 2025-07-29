using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkFlowHR.Application.DTOs.ChangePasswordDTOs;
using WorkFlowHR.Application.DTOs.MailDTOs;
using WorkFlowHR.Application.DTOs.ManagerDTOs;
using WorkFlowHR.Application.Services.AccountServices;
using WorkFlowHR.Application.Services.MailServices;
using WorkFlowHR.Domain.Entities;
using WorkFlowHR.Domain.Utilities.Concretes;
using WorkFlowHR.Domain.Utilities.Interfaces;
using WorkFlowHR.Infrastructure.Repositories.ManagerRepositories;

namespace WorkFlowHR.Application.Services.ManagerServices
{
    public class ManagerService:IManagerService
    {
        private readonly IAccountService _accountService;
        private readonly IManagerRepository _managerRepository;
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;
        private readonly IMailService _mailService;

        public ManagerService(IAccountService accountService, IManagerRepository managerRepository, IPasswordHasher<IdentityUser> passwordHasher, IMailService mailService)
        {
            _accountService = accountService;
            _managerRepository = managerRepository;
            _passwordHasher = passwordHasher;
            _mailService = mailService;
        }

        public async Task<IResult> ChangePasswordAsync(ManagerChangePasswordDTO managerChangePasswordDTO)
        {
            var manager = await _managerRepository.GetByIdAsync(managerChangePasswordDTO.Id);
            if (manager == null)
            {
                return new ErrorResult("Manager bulunamadı!");
            }

            var user = await _accountService.FindByIdAsync(manager.IdentityId);
            if (user == null)
            {
                return new ErrorResult("Kullanıcı bulunamadı!");
            }

            var result = await _accountService.ChangePasswordAsyncc(user, managerChangePasswordDTO.OldPassword, managerChangePasswordDTO.NewPassword);
            if (!result.Succeeded)
            {
                return new ErrorResult("Şifre değiştirme başarısız: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return new SuccessResult("Şifre başarıyla değiştirildi.");
        }

        public async Task<IDataResult<ManagerDTO>> CreateAsync(ManagerCreateDTO managerCreateDTO)
        {
            if (await _accountService.AnyAsync(x => x.Email == managerCreateDTO.Email))
            {
                return new ErrorDataResult<ManagerDTO>("Mail Adresi Kullanılmaktadır.");
            }

            IdentityUser identityUser = new IdentityUser
            {
                Email = managerCreateDTO.Email,
                NormalizedEmail = managerCreateDTO.Email.ToUpperInvariant(),
                UserName = managerCreateDTO.Email,
                NormalizedUserName = managerCreateDTO.Email.ToUpperInvariant(),
                EmailConfirmed = true
            };

            DataResult<ManagerDTO> result = new ErrorDataResult<ManagerDTO>();

            using (var transactionScope = await _managerRepository.BeginTransection().ConfigureAwait(false))
            {
                try
                {
                    var identityResult = await _accountService.CreateUserAsync(identityUser, Domain.Enums.Roles.Manager);
                    if (!identityResult.Succeeded)
                    {
                        var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                        result = new ErrorDataResult<ManagerDTO>(errors);
                        transactionScope.Rollback();
                        return result;
                    }

                    var manager = managerCreateDTO.Adapt<Manager>();
                    manager.IdentityId = identityUser.Id;

                    var randomPassword = PasswordHelper.GenerateRandomPassword();
                    var htmlMessage = $@"
                <table width='100%' style='font-family: Arial, sans-serif;'>
                    <tr>
                        <td align='center'>
                            <img src='*' alt='Your Company Logo' style='max-width: 200px; margin-bottom: 15px;' />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h1 style='color: #8E24AA;'>Request System</h1>
                            <p>Merhaba {manager.FirstName} {manager.LastName}</p>
                            <p><a href='https://www.workflowhr.com.tr' style='color: #8E24AA;'>https://www.workflowhr.com.tr</a> adresinden mail adresinizle aşağıda gönderilen şifre ile giriş sağlayabilirsiniz:</p>
                            <p><strong>Şifre:</strong> {randomPassword}</p>
                            <p>Teşekkürler.</p>
                            
                        </td>
                    </tr>
                </table>";

                    // E-posta ile şifre gönderme işlemi
                    var mailDTO = new MailDTO
                    {
                        Email = identityUser.Email,
                        Subject = "Welcome to YourApp",
                        Message = htmlMessage
                    };
                    await _mailService.SendMailAsync(mailDTO);

                    if (manager.Password is null)
                    {
                        // Şifreyi hashle
                        var hashedPassword = _passwordHasher.HashPassword(identityUser, randomPassword);
                        identityUser.PasswordHash = hashedPassword;

                        // Şifreyi hashlenmiş şekilde sakla
                        manager.Password = hashedPassword;
                    }
                    await _managerRepository.AddAsync(manager);
                    await _managerRepository.SaveChangesAsync();
                    result = new SuccessDataResult<ManagerDTO>("Mail Adresinizi kontrol ediniz.!!!");
                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<ManagerDTO>("Ekleme Başarısız: " + ex.Message);
                    transactionScope.Rollback();
                    throw;
                }
                finally
                {
                    transactionScope.Dispose();
                }
            }

            return result;
        }

        public async Task<IDataResult<ManagerDTO>> CreateEmployeeAsync(ManagerCreateDTO managerCreateDTO)
        {
            if (await _accountService.AnyAsync(x => x.Email == managerCreateDTO.Email))
            {
                return new ErrorDataResult<ManagerDTO>("Mail Adresi Kullanılmaktadır.");
            }

            IdentityUser identityUser = new IdentityUser
            {
                Email = managerCreateDTO.Email,
                NormalizedEmail = managerCreateDTO.Email.ToUpperInvariant(),
                UserName = managerCreateDTO.Email,
                NormalizedUserName = managerCreateDTO.Email.ToUpperInvariant(),

                EmailConfirmed = true
            };

            DataResult<ManagerDTO> result = new ErrorDataResult<ManagerDTO>();
            var strategy = await _managerRepository.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                var transactionScope = await _managerRepository.BeginTransection().ConfigureAwait(false);
                try
                {

                    var identityResult = await _accountService.CreateUserAsync(identityUser, Domain.Enums.Roles.Employee);
                    if (!identityResult.Succeeded)
                    {
                        var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                        result = new ErrorDataResult<ManagerDTO>(errors);
                        transactionScope.Rollback();
                        return;
                    }

                    var manager = managerCreateDTO.Adapt<Manager>();
                    manager.IdentityId = identityUser.Id;

                    var randomPassword = PasswordHelper.GenerateRandomPassword();
                    var htmlMessage = $@"
                        <table width='100%' style='font-family: Arial, sans-serif;'>
                            <tr>
                                <td align='center'>
                                    <img src='**' alt='Your Company Logo' style='max-width: 200px; margin-bottom: 15px;' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <h1 style='color: #8E24AA;'>Request System</h1>
                                    <p>Merhaba {manager.FirstName} {manager.LastName}</p>
                                    <p><p><a href='https://www.workflowhr.com.tr' style='color: #8E24AA;'>https://www.workflowhr.com.tr</a></p>
                                    </div> adresinden mail adresinizle aşağıda gönderilen şifre ile giriş sağlayabilirsiniz:</p>
                                    <p><strong>Şifre:</strong> {randomPassword}</p>
                                    <p>Teşekkürler.</p>
                                    
                                </td>
                            </tr>
                        </table>";



                    // E-posta ile şifre gönderme işlemi
                    var mailDTO = new MailDTO
                    {
                        Email = identityUser.Email,
                        Subject = "Welcome to YourApp",
                        Message = htmlMessage,


                    };
                    await _mailService.SendMailAsync(mailDTO);

                    if (manager.Password is null)
                    {
                        // Şifreyi hashle
                        var hashedPassword = _passwordHasher.HashPassword(identityUser, randomPassword);
                        identityUser.PasswordHash = hashedPassword;

                        // Şifreyi hashlenmiş şekilde sakla
                        manager.Password = hashedPassword;
                    }
                    await _managerRepository.AddAsync(manager);
                    await _managerRepository.SaveChangesAsync();
                    result = new SuccessDataResult<ManagerDTO>("Kullanıcı Ekleme Başarılı");
                    transactionScope.Commit();


                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<ManagerDTO>("Ekleme Başarısız: " + ex.Message);
                    transactionScope.Rollback();
                    throw;
                }
                finally
                {
                    transactionScope.Dispose();
                }
            });

            return result;
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            Result result = new ErrorResult();
            // Transaction ve strateji başlat
            var strategy = await _managerRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var transactionScope = await _managerRepository.BeginTransection().ConfigureAwait(false);
                try
                {
                    // Silinecek kullanıcıyı ID ile getir
                    var deletingUser = await _managerRepository.GetByIdAsync(id);
                    if (deletingUser == null)
                    {
                        result = new ErrorResult("Silinecek Kullanıcı Bulunamadı");
                        transactionScope.Rollback();
                        return;
                    }

                    // IdentityUser'ı sil
                    var identityResult = await _accountService.DeleteUserAsync(deletingUser.IdentityId);
                    if (!identityResult.Succeeded)
                    {
                        result = new ErrorResult("Kullanıcı Silme İşlemi Başarısız: " + string.Join(", ", identityResult.Errors.Select(e => e.Description)));
                        transactionScope.Rollback();
                        return;
                    }

                    // AppUser'ı sil ve değişiklikleri kaydet
                    await _managerRepository.DeleteAsync(deletingUser);
                    await _managerRepository.SaveChangesAsync();
                    result = new SuccessResult("Kullanıcı Silme İşlemi Başarılı");
                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorResult("Silme İşlemi Başarısız: " + ex.Message);
                    transactionScope.Rollback();
                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
            return result;
        }

        public async Task<IDataResult<List<ManagerListDTO>>> GetAllAsync()
        {
            var managerUsers = await _managerRepository.GetAllAsync();
            var managerSerDTOs = managerUsers.Adapt<List<ManagerListDTO>>();

            if (managerUsers.Count() <= 0)
            {
                return new ErrorDataResult<List<ManagerListDTO>>(managerSerDTOs, "Görüntülenecek kullanıcı bulunamadı");


            }

            return new SuccessDataResult<List<ManagerListDTO>>(managerSerDTOs, "Görüntülenecek kullanıcı eklendi.");
        }

        public async Task<IDataResult<ManagerDTO>> GetByIdAsync(Guid id)
        {
            try
            {
                var profileUser = await _managerRepository.GetByIdAsync(id);
                if (profileUser == null)
                {
                    return new ErrorDataResult<ManagerDTO>("Kullanıcı Bulunamadı");
                }
                var profileUserDTO = profileUser.Adapt<ManagerDTO>();
                return new SuccessDataResult<ManagerDTO>(profileUserDTO, "Kullanıcı başarıyla getirildi");
            }


            catch (Exception ex)
            {

                return new ErrorDataResult<ManagerDTO>("Kullanıcı Getirme işlemi başarısız:" + ex.Message);



            }
        }

        public async Task<IDataResult<ManagerDTO>> GetByIdentityUserIdAsync(string identityUserId)
        {

            var manager = await _managerRepository.GetByIdentityId(identityUserId);
            if (manager == null)
            {
                return new ErrorDataResult<ManagerDTO>("Manager bulunamadı");
            }

            var managerDTO = manager.Adapt<ManagerDTO>();
            return new SuccessDataResult<ManagerDTO>(managerDTO, "Manager başarıyla getirildi");
        }

        public async Task<IDataResult<ManagerDTO>> UpdateAsync(ManagerUpdateDTO managerUpdateDTO)
        {
            DataResult<ManagerDTO> result = new ErrorDataResult<ManagerDTO>();

            using var transactionScope = await _managerRepository.BeginTransection().ConfigureAwait(false);
            try
            {
                var updatingUser = await _managerRepository.GetByIdAsync(managerUpdateDTO.Id, false);

                if (updatingUser == null)
                {
                    result = new ErrorDataResult<ManagerDTO>("Güncellenecek kullanıcı bulunamadı");
                    transactionScope.Rollback();
                    return result;
                }

                updatingUser = managerUpdateDTO.Adapt(updatingUser);
                await _managerRepository.UpdateAsync(updatingUser);
                await _managerRepository.SaveChangesAsync();

                result = new SuccessDataResult<ManagerDTO>(updatingUser.Adapt<ManagerDTO>(), "Kullanıcı Güncelleme Başarılı");
                transactionScope.Commit();
            }
            catch (Exception ex)
            {
                result = new ErrorDataResult<ManagerDTO>("Güncelleme Başarısız: " + ex.Message);
                transactionScope.Rollback();
            }
            finally
            {
                transactionScope.Dispose();
            }

            return result;
        }
    }
}
