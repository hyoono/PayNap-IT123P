using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using Android.Content;
using Android.Graphics.Drawables;

namespace IT123_MP
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button signUp, logIn;
        ImageView logo;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            Window.DecorView.SetBackgroundResource(Resource.Drawable.gradientBG);

            logIn = FindViewById<Button>(Resource.Id.button1);
            signUp = FindViewById<Button>(Resource.Id.button2);
            logo = FindViewById<ImageView>(Resource.Id.imageView1);
            

            logo.SetImageResource(Resource.Drawable.paynaplogo);

            signUp.Click += this.UserSignUp;
            logIn.Click += this.UserLogIn;
        }
        protected void UserSignUp (object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(SignUpPage));
            StartActivity(i);
        }
        protected void UserLogIn(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(LogIn));
            StartActivity(i);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public override void OnBackPressed()
        {
            FinishAffinity();
        }

    }
}