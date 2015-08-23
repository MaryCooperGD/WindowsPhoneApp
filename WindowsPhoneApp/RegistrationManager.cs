using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPhoneApp
{
    public class RegistrationManager
    {
        private RegistrationManager manager = new RegistrationManager();

        public static enum DayOfWeek
        {
            MON,TUE,WED,THU,FRI,SAT,SUN 
        }
        

        /*Save data*/
        public static string username;
        public static string password;
        public static string LocalName;
        public static string address;
        public static string type;
        public static string description;
        public static double Lat;
        public static double Lng;
        private IDictionary<DayOfWeek, DayTimespan> daytime = new Dictionary<DayOfWeek,DayTimespan>();

        /*Private, singleton pattern*/
        private RegistrationManager() { }
    
        public RegistrationManager getInstance()
        {
            return this.manager;
        }

        
    
    }
}
