﻿using System;
using System.Collections.Generic;

namespace ACheckAPI.Models
{
    public partial class Floor
    {
        public string FloorId { get; set; }
        public string BuildingId { get; set; }
        public string FloorName { get; set; }
        public string FloorNumber { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
        public string Creater { get; set; }
        public string UpdatedAt { get; set; }
        public string Updater { get; set; }
        public bool? Active { get; set; }

        public virtual Building Building { get; set; }
    }
}
