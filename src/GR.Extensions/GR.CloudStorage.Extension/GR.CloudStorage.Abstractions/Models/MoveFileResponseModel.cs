﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GR.CloudStorage.Abstractions.Models
{
    public class MoveFileResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ParentResponseReference ParentReference { get; set; }
    }

    public class ParentResponseReference
    {
        public Guid Id { get; set; }

        public Guid DriveId { get; set; }

        public string Path { get; set; }
    }
}
