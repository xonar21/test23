using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Identity.Abstractions;
using GR.Identity.Abstractions.Events;
using GR.Identity.Abstractions.Events.EventArgs.Users;
using GR.Identity.LdapAuth.Abstractions;
using GR.Identity.LdapAuth.Abstractions.Helpers;
using GR.Identity.LdapAuth.Abstractions.Models;
using GR.Identity.Abstractions.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace GR.Identity.LdapAuth
{
    public class LdapUserManager<TLdapUser> : BaseLdapUserManager<TLdapUser> where TLdapUser : LdapUser
    {
        /// <summary>
        /// Inject user manager
        /// </summary>
        private readonly IUserManager<GearUser> _userManager;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        public LdapUserManager(IUserManager<GearUser> userManager, IMapper mapper, ILdapService<TLdapUser> ldapService) : base(ldapService)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Get Administrator
        /// </summary>
        /// <returns></returns>
        public override LdapUser GetAdministrator()
        {
            return LdapService.GetAdministrator();
        }

        /// <inheritdoc />
        /// <summary>
        /// Checks the given password again the configured LDAP server.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override Task<bool> CheckPasswordAsync(TLdapUser user, string password)
        {
            return Task.Run(() => LdapService.Authenticate(user.DistinguishedName, password));
        }

        /// <inheritdoc />
        /// <summary>
        /// Find user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override Task<TLdapUser> FindByIdAsync(string userId)
        {
            return FindByNameAsync(userId);
        }

        /// <inheritdoc />
        /// <summary>
        /// Find user by name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override Task<TLdapUser> FindByNameAsync(string userName)
        {
            return Task.FromResult(LdapService.GetUserByUserName(userName));
        }

        /// <inheritdoc />
        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> CreateAsync(TLdapUser user, string password)
        {
            try
            {
                LdapService.AddUser(user, password);
            }
            catch (Exception e)
            {
                return await Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = "LdapUserCreateFailed", Description = e.Message ?? "The user could not be created." }));
            }

            return await Task.FromResult(IdentityResult.Success);
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="distinguishedName"></param>
        /// <returns></returns>
        public override async Task<IdentityResult> DeleteUserAsync(string distinguishedName)
        {
            try
            {
                LdapService.DeleteUser(distinguishedName);
            }
            catch (Exception e)
            {
                return await Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = "LdapUserDeleteFailed", Description = e.Message ?? "The user could not be deleted." }));
            }

            return await Task.FromResult(IdentityResult.Success);
        }

        /// <inheritdoc />
        /// <summary>
        /// Get Ldap users
        /// </summary>
        public override IQueryable<TLdapUser> Users => LdapService.GetAllUsers().AsQueryable();

        /// <summary>
        /// import ldap user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override async Task<ResultModel<Guid>> ImportAdUserAsync([Required] string userName)
            => await ImportAdUserAsync(userName, AdResources.DefaultPassword);

        /// <summary>
        /// import ad user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override async Task<ResultModel<Guid>> ImportAdUserAsync([Required] string userName, string password)
        {
            var result = new ResultModel<Guid>();
            if (string.IsNullOrEmpty(userName))
            {
                result.Errors.Add(new ErrorModel(string.Empty, $"Invalid username : {userName}"));
                return result;
            }

            var exists = await _userManager.UserManager.FindByNameAsync(userName);
            if (exists != null)
            {
                result.Errors.Add(new ErrorModel(string.Empty, $"UserName {userName} exists!"));
                return result;
            }

            var ldapUser = await FindByNameAsync(userName);
            if (ldapUser == null)
            {
                result.Errors.Add(new ErrorModel(string.Empty, $"There is no AD user with this username : {userName}"));
                return result;
            }

            var user = _mapper.Map<GearUser>(ldapUser);
            user.Id = Guid.NewGuid().ToString();
            user.UserName = ldapUser.SamAccountName;
            user.Email = ldapUser.EmailAddress;
            user.AuthenticationType = AuthenticationType.Ad;
            result.IsSuccess = true;
            _userManager.UserManager.Options.Password.RequireNonAlphanumeric = false;
            var req = await _userManager.UserManager.CreateAsync(user, password);
            if (!req.Succeeded)
            {
                result.Errors.Add(new ErrorModel(string.Empty, $"Fail to add user : {userName}"));
                result.IsSuccess = false;
            }
            else
            {
                IdentityEvents.Users.UserCreated(new UserCreatedEventArgs
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    UserId = user.Id.ToGuid()
                });

                result.Result = user.Id.ToGuid();
            }

            

            return result;
        }

    }
}