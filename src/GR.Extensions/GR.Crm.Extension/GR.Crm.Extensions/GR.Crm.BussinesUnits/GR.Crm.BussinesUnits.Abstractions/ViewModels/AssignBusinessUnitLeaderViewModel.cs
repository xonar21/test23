using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions.ViewModels
{
    public class AssignBusinessUnitLeaderViewModel
    {
        public Guid Id { get; set; }

        [DisplayName("Leader")]
        public Guid? BusinessUnitLeadId { get; set; }
    }
}
