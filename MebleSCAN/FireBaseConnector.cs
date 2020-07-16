using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FireSharp.Config;
using FireSharp.Interfaces;

namespace MebleSCAN
{
    class FireBaseConnector
    {
        private readonly IFirebaseConfig fcon;
        private readonly IFirebaseClient client;
        public bool connection = false;
        public FireBaseConnector()
        {
            fcon = new FirebaseConfig()
            {
                AuthSecret = "KVFiAucq7n7LKlaubP47a30fXKeNDopS1u2nL1NU",
                BasePath = "https://fragmenttest-343f9.firebaseio.com/"
            };
            try
            {
                client = new FireSharp.FirebaseClient(fcon);
            }
            catch
            {
            }
        }
        public int testCon()
        {
            var response = Task.Run(() => client.GetAsync("test"));
            try
            {
                if (response.Result.ResultAs<int>() == 1)
                {
                    this.connection = true;
                    return 1;
                }
                else this.connection = false;
            }
            catch
            {
                this.connection = false;
                return 0;
            }
            return 0;
        }

        public int checkLogin(UserLogins login)
        {
            try
            {
                this.testCon();
                var resault = client.Get("Login/" + login.login);
                UserLogins res = resault.ResultAs<UserLogins>();
                if (res != null)
                {
                    if (login.login == res.login && login.userPassword == res.userPassword)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return -1;
            }
        }

        public List<Complaint> GetComplaints()
        {
            try
            {
                this.testCon();
                var resault = client.Get("Complaint");
                return resault.ResultAs<List<Complaint>>();
            }
            catch
            {
                return null;
            }
        }

        public int ComplaintReject(bool reject)
        {
            try
            {
                this.testCon();
                if(reject)
                {
                    GlobalVars.selectedComplaint.rejected = "1";
                    GlobalVars.selectedComplaint.complaintProgress.Add(DateTime.Now.ToString() + " - Reklamacja odrzucona.");
                }
                else
                {
                    GlobalVars.selectedComplaint.rejected = "0";
                    GlobalVars.selectedComplaint.complaintProgress.Add(DateTime.Now.ToString() + " - Reklamacja zaakceptowana.");
                }
                var setter = client.Set("Complaint/" + GlobalVars.selectedComplaint.id + "/rejected", GlobalVars.selectedComplaint.rejected);
                setter = client.Set("Complaint/" + GlobalVars.selectedComplaint.id + "/complaintProgress", GlobalVars.selectedComplaint.complaintProgress);
                return 1;
            }
            catch
            {
                return -1;
            }
        }
        public Complaint GetComplaint(string complaintID)
        {
            try
            {
                this.testCon();
                var resault = client.Get("Complaint/" + complaintID + "/");
                if (resault.ResultAs<Complaint>() != null)
                {
                    return resault.ResultAs<Complaint>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}