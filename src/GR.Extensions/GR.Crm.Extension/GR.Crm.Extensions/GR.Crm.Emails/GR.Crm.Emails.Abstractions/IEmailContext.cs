using GR.Crm.Abstractions;
using GR.Crm.Emails.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Emails.Abstractions
{
    public interface IEmailContext : ICrmContext
    {
        DbSet<EmailList> Emails { get; set; }
    }
}
