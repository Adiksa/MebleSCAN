using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Vision;
using Android.Gms.Vision.Barcodes;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using System.IO;
using static Android.Gms.Vision.Detector;
using System.Threading.Tasks;
using Android.Support.V7.App;
using MiniGun.Models;
using Android.Widget;
using System.Collections.Generic;

namespace MebleSCAN
{
    [Activity(Label = "CameraReaderActivity",Theme = "@style/AppTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class CameraReaderActivity : AppCompatActivity, ISurfaceHolderCallback, IProcessor
    {
        private SurfaceView cameraView;
        private CameraSource cameraSource;
        private BarcodeDetector barcodeDetector;
        private ProgressBar progressBar;
        private string lastScaned;

        private const int RequestCameraPermissionID = 1001;

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestCameraPermissionID:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            if (ActivityCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != Permission.Granted)
                            {
                                // Require permission
                                ActivityCompat.RequestPermissions(this, new string[]
                                    {
                                        Manifest.Permission.Camera
                                    }, RequestCameraPermissionID);
                                return;
                            }
                            try
                            {
                                cameraSource.Start(cameraView.Holder);
                            }
                            catch (IOException ie)
                            {
                                //ShowAlert(GetString(Resource.String.base_alert_error_camera_head), ie.Message.ToString(), ErrorType.Error);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CameraReaderLayout);

            var permissions = new string[] { Manifest.Permission.Camera };

            ComponentsLocalizer();
            ConfigureViewItems();
            ActionHooker();

            //Get screen size 
            var metrics = Resources.DisplayMetrics;
            var height = metrics.HeightPixels;
            var width = metrics.WidthPixels;

            barcodeDetector = new BarcodeDetector.Builder(this).SetBarcodeFormats(BarcodeFormat.QrCode | BarcodeFormat.Code128 | BarcodeFormat.DataMatrix).Build();
            cameraSource = new CameraSource.Builder(this, barcodeDetector).SetRequestedPreviewSize(height, width).SetAutoFocusEnabled(true).SetRequestedFps(30).Build();
            cameraView.Holder.AddCallback(this);
            barcodeDetector.SetProcessor(this);
        }

        private void ComponentsLocalizer()
        {
            cameraView = (SurfaceView)FindViewById(Resource.Id.cameraPreview);
            progressBar = FindViewById<ProgressBar>(Resource.Id.cameraReaderProgressBar);
        }

        private void ConfigureViewItems() { }
        private void ActionHooker() { }

        public async void ReceiveDetections(Detections detections)
        {
            SparseArray barcodes = detections.DetectedItems;
            if (barcodes.Size() != 0 && lastScaned != ((Barcode)barcodes.ValueAt(0)).DisplayValue)
            {
                lastScaned = ((Barcode)barcodes.ValueAt(0)).DisplayValue;
                RunOnUiThread(() => progressBar.Visibility = ViewStates.Visible);
                await Task.Run(() => SaveScanAndDismiss(lastScaned));
                RunOnUiThread(() => progressBar.Visibility = ViewStates.Invisible);
            }
        }

        public void SaveScanAndDismiss(string barcode)
        {
            GlobalVars.complaintID = barcode.Substring(barcode.IndexOf('{') + 1, barcode.Length - barcode.IndexOf('{') - 2);
            Log.Debug("complaint:", GlobalVars.complaintID);
            StartActivity(typeof(ComplaintActivity));
            Finish();
        }

        public void Release() { }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height) { }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            if (ActivityCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != Permission.Granted)
            {
                // Require permission
                ActivityCompat.RequestPermissions(this, new string[]
                    {
                        Manifest.Permission.Camera
                    }, RequestCameraPermissionID);
                return;
            }
            try
            {
                cameraSource.Start(cameraView.Holder);
            }
            catch (IOException ie)
            {
                //ShowAlert(GetString(Resource.String.base_alert_error_camera_head), ie.Message.ToString(), ErrorType.Error);
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            cameraSource.Stop();
        }
    }
}