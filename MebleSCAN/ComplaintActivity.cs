using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
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
        private ProgressBar progressBar;
        private bool reject;
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
            ComplaintDownload();
            stub.LayoutResource = Resource.Layout.complaint;
            stub.Inflate();
            infiniteCycle = FindViewById<HorizontalInfiniteCycleViewPager>(Resource.Id.horizontal_viewpager);
            textView = FindViewById<TextView>(Resource.Id.complaintDescription);
            listView = FindViewById<ListView>(Resource.Id.complaintProgressListView);
            acceptBtn = FindViewById<Button>(Resource.Id.complaintAccept);
            rejectBtn = FindViewById<Button>(Resource.Id.complaintReject);
            progressBar = FindViewById<ProgressBar>(Resource.Id.complaintProgressBar);
            if(GlobalVars.selectedComplaint != null)
            {
                Refresh();
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

        protected override void ActionHooker()
        {
            base.ActionHooker();
            acceptBtn.Click += delegate
            {
                reject = false;
                ComplaintReject();
            };
            rejectBtn.Click += delegate
            {
                reject = true;
                ComplaintReject();
            };
        }

        private void Refresh()
        {
            if (GlobalVars.selectedComplaint.rejected != null)
            {
                acceptBtn.Enabled = false;
                rejectBtn.Enabled = false;
            }
            listView.Adapter = new ComplaintProgressListViewAdapter(this, GlobalVars.selectedComplaint.complaintProgress);
        }

        private async void ComplaintReject()
        {
            RunOnUiThread(() => progressBar.Visibility = ViewStates.Visible);
            await Task.Run( async () =>
            {
                FireBaseConnector connector = new FireBaseConnector();
                int resault = connector.ComplaintReject(reject);
                if (resault != 1)
                {
                    GlobalVars.selectedComplaint.rejected = null;
                    GlobalVars.selectedComplaint.complaintProgress.RemoveAt(GlobalVars.selectedComplaint.complaintProgress.Count - 1);
                    RunOnUiThread(() =>
                    {
                        Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                        alertDialog.SetTitle(GetString(Resource.String.noInternetConnection));
                        alertDialog.SetIcon(Resource.Drawable.ic5c_192x192);
                        alertDialog.SetMessage(GetString(Resource.String.checkConnection));
                        alertDialog.SetNeutralButton(GetString(Resource.String.OKbutton), delegate
                        {
                            alertDialog.Dispose();
                        });
                        alertDialog.Show();
                    });
                }
                else
                {
                    if (reject)
                    {
                        RunOnUiThread(() =>
                        {
                            Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                            alertDialog.SetTitle(GetString(Resource.String.complaintChanged));
                            alertDialog.SetIcon(Resource.Drawable.icon1);
                            alertDialog.SetMessage(GetString(Resource.String.complaintRejected));
                            alertDialog.SetNeutralButton(GetString(Resource.String.OKbutton), delegate
                            {
                                alertDialog.Dispose();
                            });
                            alertDialog.Show();
                        });
                    }
                    else
                    {
                        RunOnUiThread(() =>
                        {
                            Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                            alertDialog.SetTitle(GetString(Resource.String.complaintChanged));
                            alertDialog.SetIcon(Resource.Drawable.icon1);
                            alertDialog.SetMessage(GetString(Resource.String.complaintAccepted));
                            alertDialog.SetNeutralButton(GetString(Resource.String.OKbutton), delegate
                            {
                                alertDialog.Dispose();
                            });
                            alertDialog.Show();
                        });
                    }
                }
            });
            Refresh();
            RunOnUiThread(() => progressBar.Visibility = ViewStates.Invisible);
        }

        private void ComplaintDownload()
        {
            FireBaseConnector connector = new FireBaseConnector();
            GlobalVars.selectedComplaint = connector.GetComplaint(GlobalVars.complaintID);
            if (GlobalVars.selectedComplaint == null)
            {
                if (GlobalVars.lastToastTime == null)
                {
                    GlobalVars.lastToastTime = DateTime.UtcNow;
                    Toast.MakeText(this, GetString(Resource.String.barCodeError), ToastLength.Short).Show();
                }
                else
                {
                    if((DateTime.UtcNow-GlobalVars.lastToastTime).TotalSeconds>2.0)
                    {
                        GlobalVars.lastToastTime = DateTime.UtcNow;
                        Toast.MakeText(this, GetString(Resource.String.barCodeError), ToastLength.Short).Show();
                    }
                }
                Finish();
                StartActivity(typeof(CameraReaderActivity));
            }
        }
    }
}