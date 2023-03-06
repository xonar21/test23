using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Responses;
using GR.Crm.BussinesUnits.Abstractions;
using GR.Crm.BussinesUnits.Abstractions.Models;
using GR.Crm.BussinesUnits.Abstractions.ViewModels;
using GR.Identity.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GR.Crm.BussinesUnits.Infrastructure
{
    public class BusinessUnitService : IBusinessUnitService
    {
        #region Injectable
        private readonly IBusinessUnitContext _businessUnitContext;

        private readonly IUserManager<GearUser> _userManager;

        private readonly IMapper _mapper;
        #endregion
        public BusinessUnitService(IBusinessUnitContext businessUnitContext,
            IUserManager<GearUser> userManager,
            IMapper mapper)
        {
            _businessUnitContext = businessUnitContext;
            _userManager = userManager;
            _mapper = mapper;
        }


        public async Task<ResultModel> ActivateBusinessUnit(ActivateBusinessUnitViewModel model)
        => await _businessUnitContext.ActivateRecordAsync<BusinessUnit>(model.Id);


        public async Task<ResultModel> AddDepartmentsToBusinessUnit(AddDepartmenetsToBusinessUnitViewModel model)
        {
            var checkBusinessUnit = await _businessUnitContext.BusinessUnits
                .FirstOrDefaultAsync(x => x.Id == model.BusinessUnitId);

            if (checkBusinessUnit == null)
                return new NotFoundResultModel();

            var notDeletableBusinessUnit = await _businessUnitContext.BusinessUnits
                .FirstOrDefaultAsync(x => !x.IsDeleted);

            var existingDepartments = await _businessUnitContext.Departments.Where(x => x.BusinessUnitId == model.BusinessUnitId)
                .ToListAsync();

            // Remove all departments form this business unit
            if (model.DepartmentIds == null)
            {
                foreach (var item in existingDepartments)
                {
                    item.BusinessUnitId = notDeletableBusinessUnit.Id;
                    _businessUnitContext.Departments.Update(item);
                }
                return await _businessUnitContext.PushAsync();
            }

            // Remove departments that has been unselected and add new departments
            var departmentsToAdd = model.DepartmentIds.Except(existingDepartments.Select(x => x.Id)).ToList();
            var departmentsToRemove = existingDepartments.Select(x => x.Id).Except(model.DepartmentIds).ToList();

            if (departmentsToRemove.Count != 0)
            {
                foreach (var item in departmentsToRemove)
                {
                    var departmentRemove = _businessUnitContext.Departments.First(x => x.Id == item);
                    departmentRemove.BusinessUnitId = notDeletableBusinessUnit.Id;
                    _businessUnitContext.Departments.Update(departmentRemove);
                }
            }

            if (departmentsToAdd.Count != 0)
            {
                foreach (var item in departmentsToAdd)
                {
                    var departmentAdd = _businessUnitContext.Departments.First(x => x.Id == item);
                    departmentAdd.BusinessUnitId = model.BusinessUnitId;

                    _businessUnitContext.Departments.Update(departmentAdd);
                }
            }

            return await _businessUnitContext.PushAsync();
        }

        public async Task<ResultModel> AssignBusinessUnitLeader(AssignBusinessUnitLeaderViewModel model)
        {
            var entity = await _businessUnitContext.BusinessUnits.FindAsync(model.Id);

            if (entity == null)
                return new NotFoundResultModel();

            entity.BusinessUnitLeadId = model.BusinessUnitLeadId;

            _businessUnitContext.BusinessUnits.Update(entity);

            return await _businessUnitContext.PushAsync();
        }

        public async Task<ResultModel> CreateBusinessUnit(CreateBusinessUnitViewModel model)
        {
            var businessUnit = new BusinessUnit()
            {
                Name = model.Name,
                Description = model.Description,
                BusinessUnitLeadId = model.BusinessUnitLeadId,
                Address = model.Address,
            };

            await _businessUnitContext.BusinessUnits.AddAsync(businessUnit);

            return await _businessUnitContext.PushAsync();
        }

        public async Task<ResultModel> DeleteBusinessUnit(DeleteBusinessUnitViewModel model)
        {
            var entity = await _businessUnitContext.BusinessUnits.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (entity == null)
                return new NotFoundResultModel();


            var hasDepartments = _businessUnitContext.Departments.Where(x => x.BusinessUnitId == model.Id);

            if (hasDepartments.Any())
            {
                foreach (var item in hasDepartments)
                {
                    item.BusinessUnitId = _businessUnitContext.BusinessUnits.First(x => !x.IsDeleted).Id;
                    item.IsDeleted = true;
                    _businessUnitContext.Departments.Update(item);
                }
            }

            entity.IsDeleted = true;
            _businessUnitContext.BusinessUnits.Update(entity);

            return await _businessUnitContext.PushAsync();
        }

        public async Task<ResultModel> RemoveBusinessUnit(Guid Id)
        => await _businessUnitContext.RemovePermanentRecordAsync<BusinessUnit>(Id);

        public async Task<ResultModel<BusinessUnitDetailViewModel>> GetBusinessUnitDetail(GetBusinessUnitDetailViewModel model)
        {
            var entity = await _businessUnitContext.BusinessUnits
                .Include(x => x.BusinessUnitLead)
                .Include(x => x.Departments)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (entity == null)
                return new NotFoundResultModel<BusinessUnitDetailViewModel>();

            return new SuccessResultModel<BusinessUnitDetailViewModel>( _mapper.Map<BusinessUnitDetailViewModel>(entity));
        }

        public async Task<ResultModel<IEnumerable<GetDepartmentViewModel>>> GetAllDepartments()
        {
            var departments = await _businessUnitContext.Departments.Where(x => !x.IsDeleted).ToListAsync();
            return new SuccessResultModel<IEnumerable<GetDepartmentViewModel>>(_mapper.Map<IEnumerable<GetDepartmentViewModel>>(departments));
        }

        public async Task<ResultModel<BusinessUnitListViewModel>> GetBusinessUnitList()
        {
            var businessUnits = await _businessUnitContext.BusinessUnits
                    .OrderBy(x => x.Name)
                    .Include(x => x.Departments)
                    .ThenInclude(x => x.DepartmentTeams)
                    .Include(x => x.BusinessUnitLead)
                    .Select(x => BusinessUnitLookupModel.Create(x))
                    .ToListAsync();
            return new SuccessResultModel<BusinessUnitListViewModel>(new BusinessUnitListViewModel
            {
                BusinessUnits = businessUnits
            });
        }

        public async Task<ResultModel> MoveDepartment(MoveDepartmentViewModel model)
        {
            foreach (var item in model.DepartmentIds)
            {
                var department = await _businessUnitContext.Departments.FirstOrDefaultAsync( x => x.Id == item);

                if (department == null)
                    return new NotFoundResultModel();

                var notDeletableUnitId = _businessUnitContext.BusinessUnits.First(x => !x.IsDeleted).Id;

                if (model.BusinessUnitId == null || model.BusinessUnitId == Guid.Empty)
                {
                    department.BusinessUnitId = notDeletableUnitId;

                    _businessUnitContext.Departments.Update(department);

                    return await _businessUnitContext.PushAsync();
                }

                department.BusinessUnitId = model.BusinessUnitId;

                _businessUnitContext.Departments.Update(department);
            }

            return await _businessUnitContext.PushAsync();
        }

        public async Task<ResultModel> RemoveDepartment(RemoveDepartmentViewModel model)
        {
            var notDeletableBusinessUnit = _businessUnitContext.BusinessUnits.First(x => !x.IsDeleted).Id;

            foreach (var item in model.DepartmentIds)
            {
                var entity = await _businessUnitContext.Departments.FindAsync(item);
                if (entity == null)
                    new NotFoundResultModel();
                entity.BusinessUnitId = notDeletableBusinessUnit;

                _businessUnitContext.Departments.Update(entity);
            }

            return await _businessUnitContext.PushAsync();
        }

        public async Task<ResultModel> RenameBusinessUnit(RenameBusinessUnitViewModel model)
        {
            var entity = await _businessUnitContext.BusinessUnits.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (entity == null)
                new NotFoundResultModel();

            entity.Name = model.Name;


            _businessUnitContext.BusinessUnits.Update(entity);

            return await _businessUnitContext.PushAsync();
        }

        public async Task<ResultModel> UpdateBusinessUnit(UpdateBusinessUnitViewModel model)
        {
            var entity = await _businessUnitContext.BusinessUnits.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (entity == null)
                return new NotFoundResultModel();

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.BusinessUnitLeadId = model.BusinessUnitLeadId;
            entity.Address = model.Address;

            _businessUnitContext.BusinessUnits.Update(entity);

            return await _businessUnitContext.PushAsync();
        }
    }
}
