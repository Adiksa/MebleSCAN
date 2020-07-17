using System;
using System.Diagnostics.Contracts;

namespace MebleSCAN
{
    class GlobalVars
    {
        public static string login { get; set; }
        public static Complaint selectedComplaint { get; set; }
        public static string complaintID { get; set; }
        public static DateTime lastToastTime { get; set; }
    }
}