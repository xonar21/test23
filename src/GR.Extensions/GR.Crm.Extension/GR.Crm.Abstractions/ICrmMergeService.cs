using GR.Core.Helpers;
using GR.Crm.Abstractions.ViewModels.MergeViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GR.Crm.Abstractions
{
    public interface ICrmMergeService
    {
        /// <summary>
        /// Merge Organizatins
        /// </summary>
        /// <returns></returns>
        Task<ResultModel> MergeOrganizationsAsync(MergeOrganizationsViewModel mergeOrganizationsViewModel);


        /// <summary>
        /// Merge Contacts
        /// </summary>
        /// <returns></returns>
        Task<ResultModel> MergeContactsAsync(MergeContactsViewModel mergeContactsViewModel);


        /// <summary>
        /// Merge Leads
        /// </summary>
        /// <returns></returns>
        Task<ResultModel> MergeLeadsAsync(MergeLeadsViewModel mergeLeadsViewModel, IUrlHelper Url);
    }
}
