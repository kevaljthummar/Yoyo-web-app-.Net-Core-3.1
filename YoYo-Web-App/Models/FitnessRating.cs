using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YoYo_Web_App.Models
{
    public class FitnessRating
    {
        public int AccumulatedShuttleDistance { get; set; }
        public int SpeedLevel { get; set; }
        public int ShuttleNo { get; set; }
        public double Speed { get; set; }  
        public double LevelTime { get; set; }
        public string CommulativeTime { get; set; }
        public string StartTime { get; set; }
        public double ApproxVo2Max { get; set; }
    }
}
