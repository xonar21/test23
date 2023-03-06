using System.Linq;
using GR.Crm.Abstractions.Models;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace GR.Crm.Leads.Infrastructure.Extensions
{
    public static class LeadsExtensions
    {
        /// <summary>
        /// Include lead dependencies
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static IQueryable<Lead> BuildLeadsQuery(this ILeadContext<Lead> context)
        {
            return context.Leads
                .Include(x => x.PipeLine)
                .Include(x => x.Stage)
                .Include(x => x.LeadState)
                .Include(x => x.Organization)
                .Include(x => x.Team)
                .ThenInclude(x => x.TeamMembers)
                .ThenInclude(x => x.TeamRole)
                .Include(x => x.Currency)
                .Include(i => i.Contact)
                .Include(i => i.Source)
                .Include(i => i.Contacts)
                .Include(i => i.ProductOrServiceList)
                .ThenInclude(i => i.ProductOrService)
                .Include(i => i.ProductOrServiceList)
                .ThenInclude(i => i.ProductType)
                .Include(i => i.ProductOrServiceList)
                .ThenInclude(i => i.TechnologyType)
                .Include(i => i.ProductOrServiceList)
                .ThenInclude(i => i.ServiceType)
                .Include(i => i.ProductOrServiceList)
                .ThenInclude(i => i.DevelopmentVariation)
                .Include(i => i.ProductOrServiceList)
                .ThenInclude(i => i.ConsultancyVariation)
                .Include(i => i.ProductOrServiceList)
                .ThenInclude(i => i.QAVariation)
                .Include(i => i.ProductOrServiceList)
                .ThenInclude(i => i.DesignVariation)
                .Include(i => i.ProductOrServiceList)
                .ThenInclude(i => i.DevelopementFramework)
                .Include(i => i.ProductOrServiceList)
                .ThenInclude(i => i.PMFramework);

        }
    }
}
