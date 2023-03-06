using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GR.CloudStorage.Abstractions
{
    public interface ICloudStorageDb
    {
        DbSet<IdentityUserToken<string>> UserTokens { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
