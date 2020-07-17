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

namespace MebleSCAN
{
    [Activity(Label = "ComplaintList", Theme = "@style/AppTheme.NoActionBar")]
    public class ComplaintListActivity : BaseWithMenu
    {
        private ListView listView;
        private List<Complaint> complaintList;
        private ProgressBar progressBar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.base_with_menu);
            ComponentsLocalizer();
            ActionHooker();
            RefreshComplaintList();
            navigationView.SetNavigationItemSelectedListener(this);
        }
        protected override void ComponentsLocalizer()
        {
            base.ComponentsLocalizer();
            stub.LayoutResource = Resource.Layout.complaint_list;
            stub.Inflate();
            listView = FindViewById<ListView>(Resource.Id.complaintListView);
            progressBar = FindViewById<ProgressBar>(Resource.Id.complaintListProgressBar);
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

        private async void RefreshComplaintList()
        {
            RunOnUiThread(() => progressBar.Visibility = ViewStates.Visible);
            await Task.Run(() =>
            {
                FireBaseConnector connector = new FireBaseConnector();
                complaintList = connector.GetComplaints();
                if (complaintList != null)
                {
                    ComplaintListViewAdapter adapter = new ComplaintListViewAdapter(this, complaintList);
                    RunOnUiThread(() => listView.Adapter = adapter);

                }
            });
            RunOnUiThread(() => progressBar.Visibility = ViewStates.Invisible);
        }

    }
}