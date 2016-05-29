using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDCWebApp.Utilities
{
    public static class TimeConversionUtils
    {
        public static void GetDate(string dateString, out string date)
        {
            try
            {
                var dateObj = DateTime.Parse(dateString);
                date = dateObj.ToShortDateString();
            }
            catch (Exception)
            {
                date = null;
              //  throw;
               
            }
            
            
        }

        public static string[] GetStartAndEndTime(string timeString)
        {
            string[] ret = new string[2];
            var times = timeString.Split('-');
            ret[0] = times[0].Remove(times[0].Length - 1);
            ret[1] = times[1].Remove(0, 1);
            return ret;
        }
    }
}