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
using Com.Gigamole.Infinitecycleviewpager;

namespace MebleSCAN
{
    [Activity(Label = "ComplaintActivity", Theme = "@style/AppTheme.NoActionBar")]
    public class ComplaintActivity : BaseWithMenu
    {
        private HorizontalInfiniteCycleViewPager infiniteCycle;
        private TextView textView;
        private ListView listView;
        private Button acceptBtn;
        private Button rejectBtn;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.base_with_menu);
            ComponentsLocalizer();
            ActionHooker();
        }
        protected override void ComponentsLocalizer()
        {
            base.ComponentsLocalizer();
            stub.LayoutResource = Resource.Layout.complaint;
            stub.Inflate();
            infiniteCycle = FindViewById<HorizontalInfiniteCycleViewPager>(Resource.Id.horizontal_viewpager);
            textView = FindViewById<TextView>(Resource.Id.complaintDescription);
            listView = FindViewById<ListView>(Resource.Id.complaintProgressListView);
            acceptBtn = FindViewById<Button>(Resource.Id.complaintAccept);
            rejectBtn = FindViewById<Button>(Resource.Id.complaintReject);
            if(GlobalVars.selectedComplaint.rejected == null)
            {
                acceptBtn.Enabled = false;
                rejectBtn.Enabled = false;
            }
            listView.Adapter = new ComplaintProgressListViewAdapter(this, GlobalVars.selectedComplaint.complaintProgress);
            textView.Text = GetString(Resource.String.complaintId) + " " + GlobalVars.selectedComplaint.id + "\n\n" +
                GetString(Resource.String.complaintFurnitureId) + " " + GlobalVars.selectedComplaint.furnitureId + "\n\n" +
                GetString(Resource.String.complaintDescription) + " " + GlobalVars.selectedComplaint.description + "\n\n" +
                GetString(Resource.String.complaintSenderName) + " " + GlobalVars.selectedComplaint.senderName;
            List<string> photos = new List<string>();
            photos.Add(GlobalVars.selectedComplaint.photo);
            photos.Add(GlobalVars.selectedComplaint.photo);
            photos.Add(GlobalVars.selectedComplaint.photo);
            infiniteCycle.Adapter = new InfiniteCycleAdapter(photos, this);
        }
    }
}