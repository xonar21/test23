using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GR.CloudStorage.Abstractions;
using GR.CloudStorage.Abstractions.Enums;
using GR.CloudStorage.Abstractions.Models;
using GR.CloudStorage.Implementation;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Core.Razor.ViewModels;
using GR.Identity.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace GR.Core.Razor.Controllers
{
    [Authorize]
    public class HomeController : BaseGearController
    {
        #region Injectable

        /// <summary>
        /// Inject provider of routes
        /// </summary>
        private readonly IActionDescriptorCollectionProvider _provider;

        private readonly IUserTokenDataService _userTokenDataService;

        private readonly IUserManager<GearUser> _userManager;

        private readonly IOptionsMonitor<CloudServiceSettings> _optionsMonitor;

        private readonly IStorageBaseService _service;

        private readonly IMemoryCache _memoryCache;

        #endregion

        public HomeController(IActionDescriptorCollectionProvider provider,
            IUserTokenDataService userTokenDataService,
            IUserManager<GearUser> userManager,
            IOptionsMonitor<CloudServiceSettings> optionsMonitor,
            IStorageBaseService service,
            IMemoryCache memoryCache)
        {
            _provider = provider;
            _userTokenDataService = userTokenDataService;
            _userManager = userManager;
            _optionsMonitor = optionsMonitor;
            _service = service;
            _memoryCache = memoryCache;
        }
        private Guid? LeadId { get; set; } = null;

        /// <summary>
        /// Dashboard view
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var userId = (await _userManager.GetCurrentUserAsync()).Result.Id;

            var leadId = _memoryCache.Get<string>(userId);
            if (leadId != null)
            {
                _memoryCache.Remove(leadId);
                return RedirectToAction("Details", "Leads", new { id = Guid.Parse(leadId) });
            }
            if (AppRoutes.RegisteredRoutes.Any()) return View();
            var routes = _provider.ActionDescriptors.Items.Select(x =>
                Url.Action(x.RouteValues["Action"], x.RouteValues["Controller"]).ToLowerInvariant()).ToList();
            AppRoutes.RegisteredRoutes = routes;
            return View();
        }

        /// <summary>
        /// Error page
        /// </summary>
        /// <returns></returns>
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        public async Task<RedirectResult> OnAuthComplete(string code)
        {
            var user = await _userManager.GetCurrentUserAsync();

            var dict = new Dictionary<string, string>
            {
                {"client_id", _optionsMonitor.CurrentValue.ClientId},
                {"redirect_uri", _optionsMonitor.CurrentValue.ReturnUrl},
                {"client_secret", _optionsMonitor.CurrentValue.ClientSecret},
                {"code", code},
                {"grant_type", "authorization_code"}
            };
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var postAction = await client.PostAsync("https://login.microsoftonline.com/common/oauth2/v2.0/token", new FormUrlEncodedContent(dict), CancellationToken.None);
            var result = JsonConvert.DeserializeObject<CloudLoginModel>(await postAction.Content.ReadAsStringAsync());
            await _userTokenDataService.SetUpUserToken(result.AccessToken, result.RefreshToken, Guid.Parse(user.Result.Id),
                ExternalProviders.OneDrive);
            return new RedirectResult("Index");
        }

        public async Task<IActionResult> LogInMicrosoft(Guid? leadId)
        {
            if (leadId != null)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(300));
                var userId = (await _userManager.GetCurrentUserAsync()).Result.Id;
                _memoryCache.Set(userId, leadId.ToString(), cacheEntryOptions);

            }
            return Redirect(_service.GetCodeLoginUrl(
                "files.readwrite.all offline_access User.Read Sites.Read.All"));
        }
    }
}