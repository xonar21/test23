using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GR.Core.BaseControllers;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Abstractions;
using GR.Crm.Abstractions.ViewModels.MergeViewModels;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Organizations.Abstractions;
using GR.Crm.Organizations.Abstractions.ViewModels.ContactsViewModels;
using GR.Crm.Organizations.Abstractions.ViewModels.WebProfilesViewModels;
using GR.Identity.Data.Permissions;
using GR.Identity.Permissions.Abstractions.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GR.Crm.Organizations.Razor.Controllers
{
    [Authorize]
    public class ContactController : BaseGearController
    {

        #region Injectable

        /// <summary>
        /// contact service
        /// </summary>
        private readonly ICrmContactService _contactService;

        /// <summary>
        /// Inject agreement service
        /// </summary>
        private readonly IAgreementService _agreementService;


        /// <summary>
        /// Inject crm merge
        /// </summary>
        private readonly ICrmMergeService _mergeService;


        /// <summary>
        /// Inject crm import/export
        /// </summary>
        private readonly ICrmImportExportService _importExportService;

        #endregion



        public ContactController(ICrmContactService contactService,
            IAgreementService agreementService,
            ICrmMergeService mergeService,
            ICrmImportExportService importExportService)
        {
            _contactService = contactService;
            _agreementService = agreementService;
            _mergeService = mergeService;
            _importExportService = importExportService;
        }


        public IActionResult Index(int nr = 1)
        {
            return View(nr);
        }

       
        public async Task<IActionResult> Details(Guid id)
        {
            var contactRequest = await _contactService.GetContactByIdAsync(id);
            if (!contactRequest.IsSuccess) return NotFound();
            return View(contactRequest.Result);
        }

        /// <summary>
        /// Get all contacts paginated 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<PagedResult<PaginateContactViewModel>>))]
       
        public async Task<JsonResult> GetAllContactsPaginated(PageRequest request)
            => await JsonAsync(_contactService.GetAllContactsPaginatedAsync(request));

        /// <summary>
        /// Get all contact
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetContactViewModel>>))]
       
        public async Task<JsonResult> GetAllContacts(bool includeDeleted = false)
            => await JsonAsync(_contactService.GetAllContactsAsync(includeDeleted));


        /// <summary>
        /// Get contact by id
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetContactViewModel>))]
        
        public async Task<JsonResult> GetContactById([Required] Guid contactId)
        {
            var response = await JsonAsync(_contactService.GetContactByIdAsync(contactId));
            return response;
        }

        /// <summary>
        /// Get contact by organization id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetContactViewModel>>))]
       
        public async Task<JsonResult> GetContactByOrganizationId([Required] Guid organizationId, bool includeDeleted = false)
            => await JsonAsync(_contactService.GetContactsByOrganizationIdAsync(organizationId, includeDeleted));

        /// <summary>
        /// Add new contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientCreate)]
        public async Task<JsonResult> AddNewContact([Required] ContactViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_contactService.AddNewContactAsync(model));
        }

        /// <summary>
        /// Update new contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<Guid>))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientUpdate)]
        public async Task<JsonResult> UpdateContact([Required] ContactViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_contactService.UpdateContactAsync(model));
        }


        /// <summary>
        /// Delete contact by id
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientDelete)]
        public async Task<JsonResult> DeleteContactById([Required] Guid contactId)
            => await JsonAsync(_contactService.DeleteContactAsync(contactId));


        /// <summary>
        /// Deactivate contact by id
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientDelete)]
        public async Task<JsonResult> DeactivateContactById([Required] Guid contactId)
        {
            var agreementRequest = await _agreementService.GetAllAgreementsAsync(true);

            if(agreementRequest.IsSuccess && agreementRequest.Result.FirstOrDefault(x=> x.ContactId == contactId) != null)
                return Json(new ResultModel { IsSuccess = false, Errors = new List<IErrorModel> { new ErrorModel { Message = "There is agreement that contains this contact" } } });

            return await JsonAsync(_contactService.DeactivateContactAsync(contactId));
        }

        /// <summary>
        /// Activate contact by id
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        [AuthorizePermission(PermissionsConstants.CorePermissions.BpmClientDelete)]
        public async Task<JsonResult> ActivateContactById([Required] Guid contactId)
            => await JsonAsync(_contactService.ActivateContactAsync(contactId));

        //////////////////////////////////////////////////////////////////////////////////////////// WebProfile///////////////////////////////////////////////////////////////

        /// <summary>
        /// Get web profile by id
        /// </summary>
        /// <param name="webProfileId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<GetWebProfileViewModel>))]
        public async Task<JsonResult> GetWebProfileById([Required] Guid webProfileId)
            => await JsonAsync(_contactService.GetWebProfileByIdAsync(webProfileId));


        /// <summary>
        /// Get web profile
        /// </summary>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<IEnumerable<GetWebProfileViewModel>>))]
        public async Task<JsonResult> GetAllWebProfile()
            => await JsonAsync(_contactService.GetAllProfileAsync());


        /// <summary>
        /// Add new web profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> AddWebProfile([Required] WebProfileViewModel model)
        {

            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_contactService.AddWebProfileAsync(model));
        }


        /// <summary>
        /// Update web profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateWebProfile([Required] WebProfileViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_contactService.UpdateWebProfileAsync(model));
        }


        /// <summary>
        /// Delete web profile
        /// </summary>
        /// <param name="webProfileId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteWebProfilePermanently([Required] Guid webProfileId)
            => await JsonAsync(_contactService.DeleteWebProfileAsync(webProfileId));

        /// <summary>
        /// Get webProfile image
        /// </summary>
        /// <param name="webProfileId"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetWebProfileImage([Required] Guid webProfileId)
        {
            var file = await _contactService.GetImageByteArrayAsync(webProfileId);

            if (!file.IsSuccess)
                return NotFound(StatusCode(404));

            return File(file.Result.ListByte, file.Result.Name);
        }



        ////////////////////////////////////////////////////////// Contact Web profile /////////////////////////////////////////////////////

        /// <summary>
        /// Get contactWebProfile by id
        /// </summary>
        /// <param name="contactWebProfileId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<ContactWebProfileViewModel>))]
        public async Task<JsonResult> GetContactWebProfileById([Required] Guid contactWebProfileId)
            => await JsonAsync(_contactService.GetContactWebProfileByIdAsync(contactWebProfileId));

        /// <summary>
        /// Get contactWebProfile by contact id
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel<ContactWebProfileViewModel>))]
        public async Task<JsonResult> GetContactWebProfileByContactId([Required] Guid contactId)
            => await JsonAsync(_contactService.GetContactWebProfileByContactIdAsync(contactId));



        /// <summary>
        /// Add new contactWebProfile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> AddContactWebProfile([Required] ContactWebProfileViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_contactService.AddContactWebProfileAsync(model));
        }


        /// <summary>
        /// Update new contactWebProfile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> UpdateContactWebProfile([Required] ContactWebProfileViewModel model)
        {
            if (!ModelState.IsValid) return Json(new InvalidParametersResultModel().AttachModelState(ModelState));
            return await JsonAsync(_contactService.UpdateContactWebProfileAsync(model));
        }


        /// <summary>
        /// Delete contactWebProfile by id
        /// </summary>
        /// <param name="contactWebProfileId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DeleteContactWebProfilePermanently([Required] Guid contactWebProfileId)
            => await JsonAsync(_contactService.DeleteContactWebProfileAsync(contactWebProfileId));


        /// <summary>
        /// Delete contactWebProfile by id
        /// </summary>
        /// <param name="contactWebProfileId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> DisableContactWebProfileById([Required] Guid contactWebProfileId)
            => await JsonAsync(_contactService.DisableContactWebProfileAsync(contactWebProfileId));

        /// <summary>
        /// Activate contactWebProfile by id
        /// </summary>
        /// <param name="contactWebProfileId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> ActivateContactWebProfileById([Required] Guid contactWebProfileId)
            => await JsonAsync(_contactService.ActivateContactWebProfileAsync(contactWebProfileId));


        /// <summary>
        /// Merge Contacts
        /// </summary>
        /// <param name="conta"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/[controller]/[action]")]
        [Produces("application/json", Type = typeof(ResultModel))]
        public async Task<JsonResult> MergeContacts([FromBody] MergeContactsViewModel contactsToBeMerged)
             => await JsonAsync(_mergeService.MergeContactsAsync(contactsToBeMerged), SerializerSettings);


        /// <summary>
        /// Import Contacts
        /// </summary>
        /// <param name="contactsToImport"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<JsonResult> ImportContacts(IFormCollection contactsToImport)
             => await JsonAsync(_importExportService.ImportAsync(contactsToImport, Url), SerializerSettings);
    }
}
