using GR.Core.Helpers;
using GR.Crm.BussinesUnits.Abstractions.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GR.Crm.BussinesUnits.Abstractions
{
    public interface IBusinessUnitService
    {
        #region CUD

        Task<ResultModel> ActivateBusinessUnit(ActivateBusinessUnitViewModel model);

        Task<ResultModel> AddDepartmentsToBusinessUnit(AddDepartmenetsToBusinessUnitViewModel model);

        Task<ResultModel<IEnumerable<GetDepartmentViewModel>>> GetAllDepartments();

        Task<ResultModel> AssignBusinessUnitLeader(AssignBusinessUnitLeaderViewModel model);

        Task<ResultModel> CreateBusinessUnit(CreateBusinessUnitViewModel model);

        Task<ResultModel> DeleteBusinessUnit(DeleteBusinessUnitViewModel model);

        Task<ResultModel> RemoveBusinessUnit(Guid Id);

        Task<ResultModel> MoveDepartment(MoveDepartmentViewModel model);

        Task<ResultModel> RemoveDepartment(RemoveDepartmentViewModel model);

        Task<ResultModel> RenameBusinessUnit(RenameBusinessUnitViewModel model);

        Task<ResultModel> UpdateBusinessUnit(UpdateBusinessUnitViewModel model);

        #endregion

        #region Read

        Task<ResultModel<BusinessUnitDetailViewModel>> GetBusinessUnitDetail(GetBusinessUnitDetailViewModel model);

        Task<ResultModel<BusinessUnitListViewModel>> GetBusinessUnitList();

        #endregion
    }
}
