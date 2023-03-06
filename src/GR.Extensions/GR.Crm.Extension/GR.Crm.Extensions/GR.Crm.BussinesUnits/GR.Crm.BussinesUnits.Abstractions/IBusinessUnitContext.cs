using GR.Core.Abstractions;
using GR.Crm.BussinesUnits.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions
{
    public interface IBusinessUnitContext : IDbContext
    {
        DbSet<ApplicationUser> ApplicationUsers { get; set; }

        DbSet<BusinessUnit> BusinessUnits { get; set; }

        DbSet<Department> Departments { get; set; }

        DbSet<DepartmentTeam> DepartmentTeams { get; set; }

        DbSet<Grading> Gradings { get; set; }

        DbSet<JobDepartmentTeam> JobDepartmentTeams { get; set; }

        DbSet<BusinessJobPosition> BusinessJobPositions { get; set; }

        DbSet<JobPositionGrading> JobPositionGradings { get; set; }

        DbSet<UserDepartmentTeam> UserDepartmentTeams { get; set; }
    }
}
