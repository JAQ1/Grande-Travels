﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrandeTravels.Models;

namespace GrandeTravels.ViewModels
{
    public class PackageDetailsViewModel
    {
        public Package Package { get; set; }
        public string Comment { get; set; }
        public int CommentCount { get; set; }
        public IEnumerable<Feedback> PackageFeedback { get; set; }
        public IEnumerable<Package> OtherPackages { get; set; }
    }
}
