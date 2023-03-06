using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GR.Crm.BussinesUnits.Abstractions.Models
{
    public class BusinessUnitLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public string Address { get; set; }

        public List<DepartmentLookupByParentModel> DepartmentLookups { get; set; }

        public Dictionary<Guid, string> Departments { get; set; }

        #region Business Unit Lead
        public Guid? BusinessUnitLeadId { get; set; }

        public string BusinessUnitLeadFullName { get; set; }

        #endregion

        public static Expression<Func<BusinessUnit, BusinessUnitLookupModel>> Projection
        {
            get
            {
                return businessUnit => new BusinessUnitLookupModel
                {
                    Id = businessUnit.Id,
                    Name = businessUnit.Name,
                    Address = businessUnit.Address,
                    Description = businessUnit.Description,
                    IsDeleted = businessUnit.IsDeleted,
                    BusinessUnitLeadId = businessUnit.BusinessUnitLeadId,
                    BusinessUnitLeadFullName = businessUnit.BusinessUnitLead != null
                        ? businessUnit.BusinessUnitLead.FirstName + " " + businessUnit.BusinessUnitLead.LastName
                        : "Not Specified",
                    DepartmentLookups = businessUnit.Departments.Select(x => DepartmentLookupByParentModel.Create(x)).ToList(),
                    Departments = businessUnit.Departments != null
                        ? businessUnit.Departments.ToDictionary(d => d.Id, d => d.Name)
                        : new Dictionary<Guid, string>()

                };
            }
        }

        public static BusinessUnitLookupModel Create(BusinessUnit businessUnit)
        {
            return Projection.Compile().Invoke(businessUnit);
        }
    }
}
