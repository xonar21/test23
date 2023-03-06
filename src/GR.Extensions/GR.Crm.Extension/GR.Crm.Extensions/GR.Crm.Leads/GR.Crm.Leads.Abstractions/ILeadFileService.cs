using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Core.Helpers;
using GR.Crm.Leads.Abstractions.ViewModels.LeadFileViewModels;

namespace GR.Crm.Leads.Abstractions
{
    public interface ILeadFileService
    {


        /// <summary>
        /// Add lead files 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
         Task<ResultModel> AddLeadFilesAsync(LeadFileViewModel model);


        /// <summary>
        /// Get lead file by lead id
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetLeadFileViewModel>>> GetAllLeadFilesByLeadIdAsync(Guid? leadId);

        /// <summary>
        /// Get lead file by id
        /// </summary>
        /// <param name="leadFileId"></param>
        /// <returns></returns>
        Task<ResultModel<GetLeadFileViewModel>> GetLeadFileByIdAsync(Guid? leadFileId);

        /// <summary>
        /// Delete lead file
        /// </summary>
        /// <param name="leadFileId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteLeadFileAsync(Guid leadFileId);

    }
}
