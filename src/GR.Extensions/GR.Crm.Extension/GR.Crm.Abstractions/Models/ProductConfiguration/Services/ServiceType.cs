﻿using System;
using System.ComponentModel.DataAnnotations;
using GR.Core;

namespace GR.Crm.Abstractions.Models.ProductConfiguration.Services
{
    public class ServiceType:BaseModel
    {
        /// <summary>
        /// Service type name
        /// </summary>
        [Required]
        public virtual string Name { get; set; }
    }
}
