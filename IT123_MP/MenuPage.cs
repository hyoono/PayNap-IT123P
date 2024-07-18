using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using AndroidX.AppCompat.App;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IT123_MP
{
    [Activity(Label = "Menu Page")]
    public class MenuPage : AppCompatActivity
    {

        TextView displayName, yourBalance;
        String userMobileNum;
        String[] userData;
        Button add_button, send_button, transaction_button, edit_button, logout_button;

        DatabaseClass command = new DatabaseClass();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.MenuPage);
            Window.DecorView.SetBackgroundResource(Resource.Drawable.gradientBG);

            userMobileNum = Intent.GetStringExtra("UserMobileNum");

            displayName = FindViewById<TextView>(Resource.Id.textView2);
            yourBalance = FindViewById<TextView>(Resource.Id.textView3);

            add_button = FindViewById<Button>(Resource.Id.button1);
            send_button = FindViewById<Button>(Resource.Id.button2);
            transaction_button = FindViewById<Button>(Resource.Id.button3);
            edit_button = FindViewById<Button>(Resource.Id.button4);
            logout_button = FindViewById<Button>(Resource.Id.button5);
            
            GetBalance(userMobileNum);
            GetUsername(userMobileNum);

            add_button.Click += this.RedirectAddMoney;
            send_button.Click += this.RedirectSendMoney;
            transaction_button.Click += this.RedirectViewTrans;
            edit_button.Click += this.RedirectEditProf;
            logout_button.Click += this.LogOut;
        }
        protected void RedirectAddMoney(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(AddMoney));
            i.PutExtra("UserMobileNum", userMobileNum);  //passing log-in information from one activity to the other
            StartActivity(i);
        }
        protected void RedirectSendMoney(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(SendMoneyPage));
            i.PutExtra("UserMobileNum", userMobileNum);  //passing log-in information from one activity to the other
            StartActivity(i);
        }
        protected void RedirectViewTrans(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(ViewTransHistory));
            i.PutExtra("UserMobileNum", userMobileNum);  //passing log-in information from one activity to the other
            StartActivity(i);
        }
        protected void RedirectEditProf(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(EditProfile));
            i.PutExtra("UserMobileNum", userMobileNum);  //passing log-in information from one activity to the other
            StartActivityForResult(i, 0);
        }
        protected void LogOut(object sender, EventArgs e)
        {
            Finish();
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                string stringRetFromResult = data.GetStringExtra("UserMobileNum");
                GetUsername(stringRetFromResult);
            }
        }
        protected void GetUsername(string mobileNum)
        {
            userData = command.GetUserData(mobileNum);
            displayName.Text = "Welcome, " + userData[0];
        }
        protected void GetBalance(string mobileNum)
        {
            double bal = command.GetUserBalance(mobileNum);
            yourBalance.Text = "PHP " + bal.ToString("N");
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}