using GR.Core.BaseControllers;
using GR.Core.Helpers;
using GR.Crm.BussinesUnits.Abstractions;
using GR.Crm.BussinesUnits.Abstractions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GR.Crm.BussinesUnits.Razor.Controllers
{
    public class BussinesUnitController : BaseGearController
    {
        #region Injectable
        private readonly IBusinessUnitService _businessUnitService;

        #endregion
        public BussinesUnitController(IBusinessUnitService businessUnitService)
        {
            _businessUnitService = businessUnitService;
        }

        
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var model = new GetBusinessUnitDetailViewModel { Id = id };
            var businessUnitRequest = await _businessUnitService.GetBusinessUnitDetail(model);
            if (!businessUnitRequest.IsSuccess) return NotFound();
            return View(businessUnitRequest.Result);
        }

        public async Task<IActionResult> Departments()
        {
            return View();
        }

        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<IEnumerable<GetDepartmentViewModel>>))]
        public async Task<ActionResult> GetDepartments()
            => await JsonAsync(_businessUnitService.GetAllDepartments());

        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<ActionResult> CreateBusinessUnit(CreateBusinessUnitViewModel model)
            => await JsonAsync(_businessUnitService.CreateBusinessUnit(model), SerializerSettings);

        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<ActionResult> Edit(UpdateBusinessUnitViewModel model)
            => await JsonAsync(_businessUnitService.UpdateBusinessUnit(model), SerializerSettings);

        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<BusinessUnitDetailViewModel>))]
        public async Task<IActionResult> GetBusinessUnitById(GetBusinessUnitDetailViewModel model)
            => await JsonAsync(_businessUnitService.GetBusinessUnitDetail(model), SerializerSettings);

        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<ActionResult> DeleteBusinessUnit(DeleteBusinessUnitViewModel model)
        => await JsonAsync(_businessUnitService.DeleteBusinessUnit(model));

        [HttpGet]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel<BusinessUnitListViewModel>))]
        public async Task<ActionResult> GetBusinessUnitList()
        => await JsonAsync(_businessUnitService.GetBusinessUnitList());


        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<ActionResult> AssignLeader(AssignBusinessUnitLeaderViewModel model)
            => await JsonAsync(_businessUnitService.AssignBusinessUnitLeader(model));

        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<ActionResult> Rename(RenameBusinessUnitViewModel model)
        => await JsonAsync(_businessUnitService.RenameBusinessUnit(model));


        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<ActionResult> ActivateBusinessUnit(ActivateBusinessUnitViewModel model)
        => await JsonAsync(_businessUnitService.ActivateBusinessUnit(model));

        [HttpPost]
        public async Task<ActionResult> DeactivateBusinessUnit(DeleteBusinessUnitViewModel model)
        => await JsonAsync(_businessUnitService.DeleteBusinessUnit(model));

        [HttpPost]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<ActionResult> AddDepartments(AddDepartmenetsToBusinessUnitViewModel model)
        => await JsonAsync(_businessUnitService.AddDepartmentsToBusinessUnit(model));

        [HttpDelete]
        [Route(DefaultApiRouteTemplate)]
        [Produces(ContentType.ApplicationJson, Type = typeof(ResultModel))]
        public async Task<ActionResult> RemoveDepartment(Guid id)
        => await JsonAsync(_businessUnitService.RemoveBusinessUnit(id));

    }
}
