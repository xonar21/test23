using GR.Crm.Abstractions;
using GR.Crm.Organizations.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Organizations.Abstractions
{
    public interface ICrmOrganizationContext : ICrmContext
    {

        /// <summary>
        /// Organizations
        /// </summary>
        DbSet<Organization> Organizations { get; set; }

        /// <summary>
        /// Organization Stages
        /// </summary>
        DbSet<OrganizationStage> OrganizationStages { get; set; }

        /// <summary>
        /// Organization States
        /// </summary>
        DbSet<OrganizationState> OrganizationStates { get; set; }

        /// <summary>
        /// Organization States Stages
        /// </summary>
        DbSet<OrganizationStateStage> OrganizationStatesStages { get; set; }
        /// <summary>
        /// Organizations
        /// </summary>
        DbSet<OrganizationAddress> OrganizationAddresses { get; set; }

        /// <summary>
        /// City
        /// </summary>
        DbSet<City> Cities { get; set; }

        /// <summary>
        /// Regions
        /// </summary>
        DbSet<Region> Regions { get; set; }

        /// <summary>
        /// Regions
        /// </summary>
        DbSet<CrmCountry> Countries { get; set; }

        /// <summary>
        /// Organizations
        /// </summary>
        DbSet<Industry> Industries { get; set; }

        /// <summary>
        /// Employees
        /// </summary>
        DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// Contacts
        /// </summary>
        DbSet<Contact> Contacts { get; set; }

        /// <summary>
        /// Contact web profile
        /// </summary>
        DbSet<ContactWebProfile> ContactWebProfiles { get; set; }

        /// <summary>
        /// web profile
        /// </summary>
        DbSet<WebProfile> WebProfiles { get; set; }

        ///<summary>
        ///Phone list
        /// </summary>
        DbSet<PhoneList> PhoneLists { get; set; }

        /// <summary>
        /// Revenues
        /// </summary>
        DbSet<Revenue> Revenues { get; set; }

    }
}
