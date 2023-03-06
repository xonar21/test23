using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Leads.Abstractions.ViewModels
{
    public class FileViewModel
    {
        public string Id { get; set; }

        public DateTime ModifiedAt { get; set; }


        public DateTime CreateDateTime { get; set; }

        public string Name { get; set; }

        public long Size { get; set; }

        public string WebUrl { get; set; }


        public string DownloadUrl { get; set; }
    }
}
