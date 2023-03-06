using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions.ViewModels
{
    public class RenameBusinessUnitViewModel
    {
        public Guid Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }
    }
}
