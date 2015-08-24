using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPhoneApp
{
    public class DayTimespan
    {
        public DayTimespan(TimeSpan morningBeginning, TimeSpan morningEnding, TimeSpan afternoonBeginning, TimeSpan afternoonEnding)
        {
            this.MorningBeginningTimespan = morningBeginning;
            this.MorningEndingTimespan = morningEnding;
            this.AfternoonBeginningTimespan = afternoonBeginning;
            this.AfternoonEndingTimespan = afternoonEnding;
        }
        public TimeSpan MorningBeginningTimespan { get; set; }

        public TimeSpan MorningEndingTimespan { get; set; }

        public TimeSpan AfternoonBeginningTimespan { get; set; }

        public TimeSpan AfternoonEndingTimespan { get; set; }
    }
}
