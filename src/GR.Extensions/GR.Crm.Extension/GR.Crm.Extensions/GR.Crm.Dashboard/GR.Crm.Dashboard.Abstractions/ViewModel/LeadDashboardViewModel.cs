using System;
using System.Collections.Generic;
using NPOI.SS.Formula.Functions;

namespace GR.Crm.Dashboard.Abstractions.ViewModel
{
    public class LeadDashboardViewModel
    {
        /// <summary>
        /// PipeLine name
        /// </summary>
        public virtual string PipeLine { get; set; } 
        
        /// <summary>
        /// Source name
        /// </summary>
        public virtual string Stage { get; set; }

        /// <summary>
        /// Stage name
        /// </summary>
        public virtual string Source { get; set; }

        /// <summary>
        /// Product type name
        /// </summary>
        public virtual string ProductType { get; set; }

        /// <summary>
        /// Technology type name
        /// </summary>
        public virtual string TechnologyType { get; set; }

        /// <summary>
        /// Lead count
        /// </summary>
        public virtual int LeadCount { get; set; } = 0;

        /// <summary>
        /// Lead count percentage
        /// </summary>
        public virtual double LeadCountPercentage { get; set; } = 0;

        /// <summary>
        /// Lead value sum
        /// </summary>
        public virtual decimal LeadSum { get; set; } = 0;

        /// <summary>
        /// Lead value sum in percentage
        /// </summary>
        public virtual double LeadSumPercentage { get; set; } = 0;


        /// <summary>
        /// Lead sum Progress
        /// </summary>
        public virtual decimal LeadSumProgress { get; set; }


        /// <summary>
        /// Lead state Won
        /// </summary>
        public virtual int WonLead { get; set; } = 0;

        /// <summary>
        /// Lead WonLead Progress
        /// </summary>
        public virtual decimal WonLeadProgress { get; set; }

        /// <summary>
        /// Lead WonLead evolution
        /// </summary>
        public virtual IEnumerable<DashboardEvolutionValues> WonLeadEvolution { get; set; }

        /// <summary>
        /// Lead value sum
        /// </summary>
        public virtual decimal WonLeadSum { get; set; } = 0;


        /// <summary>
        /// Lead value sum
        /// </summary>
        public virtual double WonLeadSumPercentage { get; set; } = 0;

        /// <summary>
        /// Lead WonLead Progress
        /// </summary>
        public virtual decimal WonLeadSumProgress { get; set; }


        /// <summary>
        /// Won lead values percentage
        /// </summary>
        public virtual double WonLeadCountPercentage { get; set; }


        /// <summary>
        /// Lead state New
        /// </summary>
        public virtual int NewLead { get; set; } = 0;

        /// <summary>
        /// Lead NewLead Progress
        /// </summary>
        public virtual decimal NewLeadProgress { get; set; }

        /// <summary>
        /// Lead NewLead Evolution
        /// </summary>
        public virtual IEnumerable<DashboardEvolutionValues> NewLeadEvolution { get; set; }


        /// <summary>
        /// Lead state New
        /// </summary>
        public virtual int LostLead { get; set; } = 0;

        /// <summary>
        /// Lead NewLead Progress
        /// </summary>
        public virtual decimal LostLeadProgress { get; set; }

        /// <summary>
        /// Lead LostLead Evolution
        /// </summary>
        public virtual IEnumerable<DashboardEvolutionValues> LostLeadEvolution { get; set; }

        /// <summary>
        /// Lost lead money
        /// </summary>
        public virtual decimal LostLeadSum { get; set; } = 0;

        /// <summary>
        /// Lost lead values percentage
        /// </summary>
        public virtual double LostLeadCountPercentage { get; set; }

        public virtual IList<LeadsByUsersViewModel> LeadsByUsers { get; set; } = new List<LeadsByUsersViewModel>();

        /// <summary>
        /// Month
        /// </summary>
        public virtual string Month { get; set; }

        /// <summary>
        /// Lost vs Won leads values
        /// </summary>
        public virtual IList<decimal> LostAndWonValues { get; set; } = new List<decimal>();

    }



    public class DashboardEvolutionValues
    {
        public virtual object GroupKey { get; set; }

        public virtual int Value { get; set; }
    }
}
