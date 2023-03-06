using System;
using System.Threading.Tasks;
using GR.Identity.Abstractions;
using GR.Identity.Abstractions.Extensions;
using GR.Identity.Abstractions.Helpers.Attributes;
using GR.Notifications.Abstractions.Models.Config;
using GR.Notifications.Hub.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace GR.Notifications.Hub.Hubs
{
    [GearAuthorize(GearAuthenticationScheme.IdentityWithBearer)]
    public class GearNotificationHub : Microsoft.AspNetCore.SignalR.Hub
    {

        /// <summary>
        /// Inject user manager
        /// </summary>
        private readonly IUserManager<GearUser> _userManager;

        private readonly IHttpContextAccessor _contextAccessor;

        public GearNotificationHub(
            IUserManager<GearUser> userManager, 
            IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        internal static class UserConnections
        {
            /// <summary>
            /// Store connections on memory
            /// </summary>
            public static readonly ConnectionMapping Connections = new ConnectionMapping();
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {

            await base.OnConnectedAsync();
            
            if (_userManager.IsAuthenticated())
            {
                var userIdReq = _userManager.FindUserIdInClaims();
                if (!userIdReq.IsSuccess) return;

                UserConnections.Connections.Add(new SignalrConnection
                {
                    ConnectionId = Context.ConnectionId,
                    UserId = userIdReq.Result
                });
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// On User disconnect
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            UserConnections.Connections.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
