using FertilityCare.UseCase.DTOs.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Feedbacks
{
    public class FeedbackLatestSideManager
    {
        public DoctorDTO Doctor { get; set; }
    
        public IEnumerable<SecondFeedbackLatest> Feedbacks { get; set; }
    }
}
