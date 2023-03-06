using GR.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GR.Crm.Abstractions
{
    public interface ICrmImportExportService
    {

        /// <summary>
        /// Import
        /// </summary>
        /// <returns></returns>
        Task<ResultModel> ImportAsync(IFormCollection objToImport, IUrlHelper Url);
    }
}
