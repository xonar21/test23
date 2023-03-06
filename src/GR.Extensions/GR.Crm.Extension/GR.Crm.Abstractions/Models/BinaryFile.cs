using GR.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Abstractions.Models
{
    public class BinaryFile : BaseModel
    {
        public byte[] DataFiles { get; set; }
    }
}
