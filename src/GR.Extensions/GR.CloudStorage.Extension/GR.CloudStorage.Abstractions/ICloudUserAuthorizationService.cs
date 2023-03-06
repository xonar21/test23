using System;
using System.Threading.Tasks;
using GR.CloudStorage.Abstractions.Enums;

namespace GR.CloudStorage.Abstractions
{
    public interface ICloudUserAuthorizationService
    {
        /// <summary>
        /// Check if current token is outdated
        /// if yes exchange it for a new token. 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <param name="userId"></param>
        /// <param name="provider"></param>
        /// <param name="clientId"></param>
        /// <param name="redirectUri"></param>
        /// <param name="clientSecret"></param>
        /// <returns></returns>
        Task VerifyUser(string accessToken, string refreshToken, Guid userId,
            ExternalProviders provider, string clientId, string redirectUri, string clientSecret);
    }
}
