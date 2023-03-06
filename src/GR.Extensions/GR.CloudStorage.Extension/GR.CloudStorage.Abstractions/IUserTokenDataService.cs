using System;
using System.Threading.Tasks;
using GR.CloudStorage.Abstractions.Enums;

namespace GR.CloudStorage.Abstractions
{
    public interface IUserTokenDataService
    {
        Task DeleteUserToken(Guid userId, ExternalProviders provider);

        Task<string> GetUserAccessToken(Guid userId, ExternalProviders provider);

        Task<string> GetUserRefreshToken(Guid userId, ExternalProviders provider);

        Task SetUpUserToken(string accessToken, string refreshToken, Guid userId, ExternalProviders provider);
        Task<bool> CheckUserToken(Guid userId);
    }
}
