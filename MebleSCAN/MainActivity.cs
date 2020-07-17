using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MebleSCAN
{
    [Activity(Label = "MainActivity", Theme = "@style/AppTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : BaseWithMenu
    {
        private FrameLayout scan;
        private FrameLayout list;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.base_with_menu);
            ComponentsLocalizer();
            ActionHooker();
            navigationView.SetNavigationItemSelectedListener(this);
        }

        protected override void ComponentsLocalizer()
        {
            base.ComponentsLocalizer();
            stub.LayoutResource = Resource.Layout.main_activity;
            stub.Inflate();
            scan = FindViewById<FrameLayout>(Resource.Id.scanFrameLayout);
            list = FindViewById<FrameLayout>(Resource.Id.listFrameLayout);
        }

        protected override void ActionHooker()
        {
            base.ActionHooker();
            scan.Click += delegate
            {
                StartActivity(typeof(CameraReaderActivity));
            };
            list.Click += delegate
            {
                StartActivity(typeof(ComplaintListActivity));
            };
        }
    }
}