using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions.Models
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<BusinessUnit> BusinessUnits { get; set; }

        public ICollection<Department> Department { get; set; }

        public ICollection<DepartmentTeam> DepartmentTeam { get; set; }

        public ICollection<UserDepartmentTeam> UserDepartmentTeams { get; set; }
    }
}
