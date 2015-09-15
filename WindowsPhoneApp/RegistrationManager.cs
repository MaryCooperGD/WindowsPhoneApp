﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;
using System.IO;
using System.Runtime.Serialization.Json;


namespace WindowsPhoneApp
{
    public class RegistrationManager
    {
        private static RegistrationManager manager = new RegistrationManager();

        public static readonly string ParseName = "account";
        public static readonly string Version = "1";

        public enum DayOfWeek
        {
            MON,TUE,WED,THU,FRI,SAT,SUN 
        }
        

        /*Save data*/
        public string username { get; set; }
        public string password { get; set; }
        public string LocalName { get; set; }
        public string address { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        public ParseObject currAccount { get; set; }

        /*At first there was no string in the key field. Actually it is reqired to 
         save the dictionary correctly into PARSE */
        private IDictionary<string, string> daytime = new Dictionary<string,string>();

        /*Private, singleton pattern*/
        private RegistrationManager() { }
    
        public static RegistrationManager getInstance()
        {
            return manager;
        }

        /*Adds a Daytimespan to a specific day of the week. if the day already exixts, returns false*/
        public Boolean addDayTime(string day , DayTimespan time)
        {
            if (daytime.ContainsKey(day))
            {
                return false;
            }

            //Json Serialization

            string obj = Newtonsoft.Json.JsonConvert.SerializeObject(time);

            daytime.Add(day, obj);
            return true;

        }

        /*Modifies a timespan for a certain day of week*/
        public Boolean changeDayTime(string day, DayTimespan time)
        {
            if (daytime.ContainsKey(day))
            {
                daytime.Remove(day);

                //usare serializzazione json
                string obj = Newtonsoft.Json.JsonConvert.SerializeObject(time);
                daytime.Add(day, obj);
                return true;
            }
            return false;
        }

        public void replaceDictionary(IDictionary<string, string> dictionary)
        {
            this.daytime = dictionary;
        }

        /*could be implemented in a safer way, but for university purpose it is enogh*/
        public IDictionary<string, string> getDictionary()
        {
            return this.daytime;
        }

        /*Turns the registration manager instance into a new Parse object to save into the database*/
        public ParseObject getParseObject()
        {
            ParseObject obj = new ParseObject(RegistrationManager.ParseName);
            obj["username"] = username;
            obj["password"] = password;
            obj["localname"] = LocalName;
            obj["address"] = address;
            obj["type"] = type;
            obj["description"] = description;
            obj["lat"] = Lat;
            obj["lng"] = Lng;
            obj["dictionary"] = daytime;

            return obj;
        }

        public void reset(ParseObject account)
        {
            username = account.Get<string>("username");
            password = account.Get<string>("password");
            LocalName = account.Get<string>("localname");
            type = account.Get<string>("type");
            description = account.Get<string>("description");
            address = account.Get<string>("address");
            Lat = account.Get<double>("lat");
            Lng = account.Get<double>("lng");
            replaceDictionary(account.Get<IDictionary<string, string>>("dictionary"));
            currAccount = account;
        }

        
    
    }
}
