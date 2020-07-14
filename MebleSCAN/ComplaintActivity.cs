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
    [Activity(Label = "ComplaintActivity")]
    public class ComplaintActivity : BaseWithMenu
    {
        private HorizontalInfiniteCycleViewPager infiniteCycle;
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
            List<string> photos = new List<string>();
            photos.Add(GlobalVars.selectedComplaint.photo);
            photos.Add(GlobalVars.selectedComplaint.photo);
            photos.Add(GlobalVars.selectedComplaint.photo);
            infiniteCycle.Adapter = new InfiniteCycleAdapter(photos, this);
        }

    }
}