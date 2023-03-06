using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Leads.Abstractions.ViewModels
{
    public class UploadFileViewModel
    {
        public Guid LeadId { get; set; }

        public IFormFile File { get; set; }
    }
}
