using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Crm.Abstractions;
using GR.Crm.Abstractions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GR.Crm.Razor.Controllers
{
    public class GeneralConfigurationController : BaseGearController
    {
        #region Injectable
        /// <summary>
        /// Inject ICrmService
        /// </summary>
        private readonly ICrmService _crmService;
        #endregion

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public GeneralConfigurationController(ICrmService crmService)
        {
            _crmService = crmService;
        }
        #region GeneralConfigurations 
        /// <summary>
        /// Get General Configurations
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GeneralConfigurations>))]
        public async Task<JsonResult> GetGeneralConfigurations()
            => await JsonAsync(_crmService.GetGeneralConfigurations());


        /// <summary>
        /// Update General Configurations
        /// </summary>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GeneralConfigurations>))]
        public async Task<JsonResult> UpdateGeneralConfigurations(GeneralConfigurations model)
            => await JsonAsync(_crmService.UpdateGeneralConfigurations(model));
        #endregion

    }
}
