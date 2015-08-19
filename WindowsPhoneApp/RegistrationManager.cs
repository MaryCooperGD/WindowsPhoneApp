using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPhoneApp
{
    public static class RegistrationManager
    {
        /* Tags for the dictionary. Maybe an enumeration is better*/
        public static readonly string Monday = "mon";
        public static readonly string Tuesday = "tue";
        public static readonly string Wednesday = "wed";
        public static readonly string Thursday = "thu";
        public static readonly string Friday = "fri";
        public static readonly string Saturday = "sat";
        public static readonly string Sunday = "sun";
        

        /*Save data*/
        public static string username;
        public static string password;
        public static string LocalName;
        public static string address;
        public static string type;
        public static string description;
        public static double Lat;
        public static double Lng;
        public static IDictionary<string, TimeSpan> daytime = new Dictionary<string,TimeSpan>(); /* Instead of timespan, we need a custom class to assign 4 times for each day */

        /*Private, singleton pattern*/
       /* private RegistrationManager() { }*/
    
    }
}
