using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityCare.UseCase.DTOs.Feedbacks
{
    public class StatisticsFeedbackDTO
    {
        public int NumberOf5Start { get; set; }
        public int NumberOf4Start { get; set; }
        public int NumberOf3Start { get; set; }
        public int NumberOf2Start { get; set; }
        public int NumberOf1Start { get; set; }
        public int TotalFeedbacks { get; set; }
    }
}
