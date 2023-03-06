using GR.Crm.Abstractions;
using GR.Crm.Marketing.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Marketing.Abstractions
{
    public interface ICrmMarketingListContext : ICrmContext
    {

        /// <summary>
        /// Marketing Lists
        /// </summary>
        DbSet<MarketingList> MarketingLists { get; set; }

        /// <summary>
        /// Marketing List and Organizations Joining Table
        /// </summary>
        DbSet<MarketingListOrganization> MarketingListOrganizations { get; set; }
    }
}
