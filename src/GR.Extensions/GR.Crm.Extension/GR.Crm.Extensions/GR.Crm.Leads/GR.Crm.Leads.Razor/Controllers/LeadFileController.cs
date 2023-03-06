using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.ViewModels.LeadFileViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GR.Crm.Leads.Razor.Controllers
{
    public class LeadFileController : BaseGearController
    {

        #region Injectable

        private readonly ILeadFileService _leadFileService;

        #endregion


        public LeadFileController(ILeadFileService leadFileService)
        {
            _leadFileService = leadFileService;
        }


        /// <summary>
        /// Get lead files by lead id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<GetLeadFileViewModel>>))]

        public async Task<JsonResult> GetAllLeadFilesByLeadId([Required]Guid  leadId)
            => await JsonAsync(_leadFileService.GetAllLeadFilesByLeadIdAsync(leadId), SerializerSettings);

        /// <summary>
        /// Get lead file by id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<GetLeadFileViewModel>))]

        public async Task<JsonResult> GetLeadFileById([Required] Guid leadFileId)
            => await JsonAsync(_leadFileService.GetLeadFileByIdAsync(leadFileId), SerializerSettings);


        /// <summary>
        /// Add lead files
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]

        public async Task<JsonResult> AddLeadFiles([Required] LeadFileViewModel model)
            => await JsonAsync(_leadFileService.AddLeadFilesAsync(model), SerializerSettings);


        /// <summary>
        /// Delete lead files
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]

        public async Task<JsonResult> DeleteLeadFile([Required] Guid leadFileId)
            => await JsonAsync(_leadFileService.DeleteLeadFileAsync(leadFileId), SerializerSettings);


        /// <summary>
        /// Get file
        /// </summary>
        /// <param name="leadFileId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetFile(Guid leadFileId)
        {
            var response = await _leadFileService.GetLeadFileByIdAsync(leadFileId);
            return response.Result != null
                ? File(response.Result.File, "application/octet-stream", response.Result.Name)
                : null;
        }

    }
}
