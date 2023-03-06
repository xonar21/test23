using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GR.CloudStorage.Abstractions;
using GR.CloudStorage.Abstractions.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace GR.CloudStorage.Implementation
{
    public class BaseCloudUserDataService : IUserTokenDataService
    {
        private readonly ICloudStorageDb _applicationContext;

        public BaseCloudUserDataService(ICloudStorageDb applicationContext)
        {
            _applicationContext = applicationContext;
        }

        /// <summary>
        /// Set user token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="userId"></param>
        /// <param name="provider"></param>
        /// <param name="accessToken"></param>
        public virtual async Task SetUpUserToken(string accessToken, string refreshToken, Guid userId, ExternalProviders provider)
        {
            var currentToken = await _applicationContext.UserTokens.FirstOrDefaultAsync(x =>
                x.UserId == userId.ToString() && x.LoginProvider == provider.ToString());

            if (currentToken == null)
            {
                _applicationContext.UserTokens.Add(new IdentityUserToken<string>()
                {
                    UserId = userId.ToString(),
                    Value = "{" + $"\"access_token\":\"{accessToken}\",\"refresh_token\":\"{refreshToken}\"" +
                            "}",
                    LoginProvider = provider.ToString(),
                    Name = provider + ":" + userId
                });
            }
            else
            {
                currentToken.Value = "{" + $"\"access_token\":\"{accessToken}\",\"refresh_token\":\"{refreshToken}\"" +
                                     "}";
                _applicationContext.UserTokens.Update(currentToken);
            }

            await _applicationContext.SaveChangesAsync(new CancellationToken(false));
        }

        /// <summary>
        /// Delete user token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="provider"></param>
        public virtual async Task DeleteUserToken(Guid userId, ExternalProviders provider)
        {
            _applicationContext.UserTokens.Remove
                (await _applicationContext.UserTokens.FirstOrDefaultAsync(x => x.UserId == userId.ToString() && x.LoginProvider == provider.ToString())
                 ?? throw new InvalidOperationException());
            await _applicationContext.SaveChangesAsync(CancellationToken.None);
        }

        /// <summary>
        /// Get user Token for specified provider
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public virtual async Task<string> GetUserAccessToken(Guid userId, ExternalProviders provider)
        {
            return JObject.Parse((await _applicationContext.UserTokens.FirstOrDefaultAsync(x =>
                    x.UserId == userId.ToString() && x.LoginProvider == provider.ToString()))
                ?.Value).GetValue("access_token").Value<string>();
        }
        public virtual async Task<bool> CheckUserToken(Guid userId)
        {
            var exists = _applicationContext.UserTokens.Any(x => x.UserId == userId.ToString());

            string userToken = exists ? JObject.Parse((await _applicationContext.UserTokens.FirstOrDefaultAsync(x => x.UserId == userId.ToString()))?.Value)?.GetValue("access_token")?.Value<string>() : null;

            return !string.IsNullOrEmpty(userToken);
        }

        /// <summary>
        /// Get user refresh Token for specified provider
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public virtual async Task<string> GetUserRefreshToken(Guid userId, ExternalProviders provider)
        {
            return JObject.Parse((await _applicationContext.UserTokens.FirstOrDefaultAsync(x =>
                x.UserId == userId.ToString() && x.LoginProvider == provider.ToString()))
                ?.Value).GetValue("refresh_token").Value<string>();
        }
    }
}
