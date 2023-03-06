using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Responses;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Leads.Abstractions.ViewModels.LeadFileViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Leads.Infrastructure
{
    public class LeadFileService: ILeadFileService
    {

        #region Injections

        /// <summary>
        /// Inject context
        /// </summary>
        private readonly ILeadContext<Lead> _context;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        #endregion
        

        public LeadFileService(ILeadContext<Lead> context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        
        /// <summary>
        /// Add lead files 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> AddLeadFilesAsync(LeadFileViewModel model)
        {
            if(model == null || !model.Files.Any())
                return new InvalidParametersResultModel();
            
            var lead = await _context.Leads.FirstOrDefaultAsync(x => x.Id == model.LeadId);

            if(lead == null)
                return new NotFoundResultModel();

            foreach (var file in model.Files)
            { 
                await _context.LeadFiles.AddAsync(new LeadFile
                {
                    LeadId = model.LeadId,
                    Name = file.FileName,
                    File = await GetBytes(file)
                });
            }

            return await _context.PushAsync();
        }

        /// <summary>
        /// Get lead file by lead id
        /// </summary>
        /// <param name="leadId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetLeadFileViewModel>>> GetAllLeadFilesByLeadIdAsync(Guid? leadId)
        {
            if(leadId == null)
                return new InvalidParametersResultModel<IEnumerable<GetLeadFileViewModel>>();

            var leads = await _context.LeadFiles
                .Include(i => i.Lead)
                .Where(x => x.LeadId == leadId).ToListAsync();

            return new SuccessResultModel<IEnumerable<GetLeadFileViewModel>>(_mapper.Map<IEnumerable<GetLeadFileViewModel>>(leads));

        }

        /// <summary>
        /// Get lead file by id
        /// </summary>
        /// <param name="leadFileId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetLeadFileViewModel>> GetLeadFileByIdAsync(Guid? leadFileId)
        {
            if(leadFileId == null)
                return  new InvalidParametersResultModel<GetLeadFileViewModel>();


            var lead = await _context.LeadFiles.FirstOrDefaultAsync(x => x.Id == leadFileId);

            if(lead == null)
                return new NotFoundResultModel<GetLeadFileViewModel>();

            return new SuccessResultModel<GetLeadFileViewModel>(_mapper.Map<GetLeadFileViewModel>(lead));

        }


        /// <summary>
        /// Delete lead file
        /// </summary>
        /// <param name="leadFileId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteLeadFileAsync(Guid leadFileId) =>
            await _context.RemovePermanentRecordAsync<LeadFile>(leadFileId);



        #region Helpers

        public static async Task<byte[]> GetBytes(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }


        #endregion

    }
}
