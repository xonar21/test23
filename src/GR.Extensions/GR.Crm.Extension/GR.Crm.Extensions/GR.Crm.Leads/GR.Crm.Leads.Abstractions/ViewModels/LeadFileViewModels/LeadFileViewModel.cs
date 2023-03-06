using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace GR.Crm.Leads.Abstractions.ViewModels.LeadFileViewModels
{
    public class LeadFileViewModel
    {

        public virtual Guid LeadId { get; set; }

        public virtual IEnumerable<IFormFile> Files { get; set; } = new List<IFormFile>();

    }
}
