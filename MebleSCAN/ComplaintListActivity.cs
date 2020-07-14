using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MebleSCAN
{
    [Activity(Label = "ComplaintList")]
    public class ComplaintListActivity : BaseWithMenu
    {
        private ListView listView;
        private List<Complaint> complaintList;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.base_with_menu);
            ComponentsLocalizer();
            ActionHooker();
            RefreshComplaintList();
        }
        protected override void ComponentsLocalizer()
        {
            base.ComponentsLocalizer();
            stub.LayoutResource = Resource.Layout.complaint_list;
            stub.Inflate();
            listView = FindViewById<ListView>(Resource.Id.complaintListView);
        }

        protected override void ActionHooker()
        {
            base.ActionHooker();
            listView.ItemClick += ListView_ItemClick;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            GlobalVars.selectedComplaint = complaintList[e.Position];
            StartActivity(typeof(ComplaintActivity));
        }

        private void RefreshComplaintList()
        {
            FireBaseConnector connector = new FireBaseConnector();
            complaintList = connector.GetComplaints();
            if(complaintList != null)
            {
                ComplaintListViewAdapter adapter = new ComplaintListViewAdapter(this, complaintList);
                listView.Adapter = adapter;
            }
        }

    }
}