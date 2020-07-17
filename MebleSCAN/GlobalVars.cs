using System;
using System.Diagnostics.Contracts;

namespace MebleSCAN
{
    class GlobalVars
    {
        public static string login { get; set; }
        public static Complaint selectedComplaint { get; set; }
        public static string complaintID { get; set; }
        public static bool newId { get; set; }
        public static DateTime lastToastTime { get; set; }
        public static bool ToastTimeCheck()
        {
            if(lastToastTime==null)
            {
                lastToastTime = DateTime.UtcNow;
                return true;
            }
            if ((DateTime.UtcNow - lastToastTime).TotalSeconds > 2.0)
                return true;
            return false;
        }
    }
}