using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions.ViewModels
{
    public class UpdateBusinessUnitViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [DisplayName("Status")]
        public bool Active { get; set; }

        [DisplayName("Leader")]
        public Guid? BusinessUnitLeadId { get; set; }

        public string Address { get; set; }
    }
}
