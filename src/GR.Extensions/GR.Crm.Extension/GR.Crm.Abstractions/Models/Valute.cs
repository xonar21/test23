using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Crm.Abstractions.Models
{
    public class Valute
    {
        public int NumCode { get; set; }

        public string CharCode { get; set; }

        public int Nominal { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }
    }
}
