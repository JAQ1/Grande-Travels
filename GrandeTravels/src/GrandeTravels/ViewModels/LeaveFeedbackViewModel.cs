using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrandeTravels.Models;
using System.ComponentModel.DataAnnotations;

namespace GrandeTravels.ViewModels
{
    public class LeaveFeedbackViewModel
    {

        [Required]
        [RegularExpression(@"^[0-5]", ErrorMessage = "Select rating between 0 & 5")]
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; }
        public Package Package { get; set; }
    }
}
