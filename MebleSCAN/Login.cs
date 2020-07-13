using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using System.Threading.Tasks;
using Android.Views;

namespace MebleSCAN
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class Login : AppCompatActivity
    {
        private EditText userLogin;
        private EditText userPassword;
        private Button loginBtn;
        private ProgressBar progressBar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.login);
            ComponentsLocalizer();
            ActionHooker();
        }

        private void ActionHooker()
        {
            loginBtn.Click += delegate
            {
                LoginCheck();
            };
        }

        private void ComponentsLocalizer()
        {
            userLogin = FindViewById<EditText>(Resource.Id.login);
            userPassword = FindViewById<EditText>(Resource.Id.password);
            loginBtn = FindViewById<Button>(Resource.Id.loginButton);
            progressBar = FindViewById<ProgressBar>(Resource.Id.LoginsProgressBar);
        }

        private async Task LoginCheck()
        {
            await Task.Run(() => this.RunOnUiThread(() =>
            {
                progressBar.Visibility = ViewStates.Visible;
            }));
            UserLogins login = new UserLogins()
            {
                login = userLogin.Text,
                userPassword = userPassword.Text
            };
            FireBaseConnector connection = new FireBaseConnector();
            if (login.login.Length > 0 && login.userPassword.Length > 0)
            {
                var res = connection.checkLogin(login);
                await Task.Run(() => this.RunOnUiThread(() =>
                {
                    progressBar.Visibility = ViewStates.Invisible;
                }));
                if (res == 2)
                {
                    Finish();
                    
                }
                if (res == 1)
                {
                    StartActivity(typeof(MainActivity));
                    Finish();

                }
                if (!connection.connection)
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
                }
                if (res == 0)
                {
                    Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                    alertDialog.SetTitle(GetString(Resource.String.loginError));
                    alertDialog.SetIcon(Resource.Drawable.ic4c_192x192);
                    alertDialog.SetMessage(GetString(Resource.String.correctLogin));
                    alertDialog.SetNeutralButton(GetString(Resource.String.OKbutton), delegate
                    {
                        alertDialog.Dispose();
                    });
                    alertDialog.Show();
                }
            }
            else
            {
                Android.Support.V7.App.AlertDialog.Builder alertDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                alertDialog.SetTitle(GetString(Resource.String.loginError));
                alertDialog.SetIcon(Resource.Drawable.ic4c_192x192);
                alertDialog.SetMessage(GetString(Resource.String.emptyLogin));
                alertDialog.SetNeutralButton(GetString(Resource.String.OKbutton), delegate
                {
                    alertDialog.Dispose();
                });
                alertDialog.Show();
            }
            await Task.Run(() => this.RunOnUiThread(() =>
            {
                progressBar.Visibility = ViewStates.Invisible;
            }));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}