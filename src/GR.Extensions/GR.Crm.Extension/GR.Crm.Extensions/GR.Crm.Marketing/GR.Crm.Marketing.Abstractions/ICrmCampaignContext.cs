using GR.Crm.Abstractions;
using GR.Crm.Marketing.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Marketing.Abstractions
{
    public interface ICrmCampaignContext : ICrmContext
    {
        /// <summary>
        /// Campaigns
        /// </summary>
        DbSet<Campaign> Campaigns { get; set; }

        /// <summary>
        /// Campaigns and Marketing Lists Joining Table
        /// </summary>
        DbSet<CampaignMarketingList> CampaignsMarketingLists { get; set; }

    }
}
