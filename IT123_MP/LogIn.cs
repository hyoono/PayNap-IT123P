using System;
using System.IO;
using System.Net;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace IT123_MP
{
    [Activity(Label = "Log In Page")]
    public class LogIn : AppCompatActivity

    {
        CheckBox showPass;
        EditText mobile_num, password;
        Button btn_login, btn_cancel;
        String mobileNum = "", user_pass = "", res = "";
        HttpWebResponse response;
        HttpWebRequest request;

        DatabaseClass command = new DatabaseClass();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.LogIn);

            mobile_num = FindViewById<EditText>(Resource.Id.editText1);
            password = FindViewById<EditText>(Resource.Id.editText2);

            btn_login = FindViewById<Button>(Resource.Id.button1);
            btn_cancel = FindViewById<Button>(Resource.Id.button2);

            showPass = FindViewById<CheckBox>(Resource.Id.checkBox1);
            password.InputType = Android.Text.InputTypes.TextVariationPassword | Android.Text.InputTypes.ClassText;

            showPass.CheckedChange += this.ShowPassword;
            btn_login.Click += this.LogInClicked;
            btn_cancel.Click += this.BackInfo;
        }

        public void LogInClicked(object sender, EventArgs e)
        {
            mobileNum = mobile_num.Text;
            user_pass = password.Text;

            bool validData = Validations();

            if (validData)
            {
                request = (HttpWebRequest)WebRequest.Create("http://172.18.13.160/MoneySendingApp/Functions/check_login.php?mobile_num=" + mobileNum + "&user_password=" + user_pass);
                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                res = reader.ReadToEnd();
                Toast.MakeText(this, res, ToastLength.Long).Show();

                if (res.Contains("Success!"))
                {
                    Intent i = new Intent(this, typeof(MenuPage));
                    i.PutExtra("UserMobileNum", mobileNum);  //baka gamitin ung name sa kabilang Activity
                    StartActivity(i);
                    mobile_num.Text = String.Empty;
                    password.Text = String.Empty;
                }
            }
        }
        protected bool Validations()
        {
            bool stringEmpty = command.EmptyValidation(mobileNum + "," + user_pass);
            bool isNumValid = command.NumberValidation(mobileNum);
            bool userExist = command.SearchUserExist(mobileNum);
            
            if (stringEmpty)
                { Toast.MakeText(this, "Please fill all the details.", ToastLength.Long).Show(); return false; }

            else if (!isNumValid)
            { Toast.MakeText(this, "Invalid Mobile Number", ToastLength.Long).Show(); return false; }

            else if (!userExist)
            { Toast.MakeText(this, "Account Doesn't Exist.", ToastLength.Long).Show(); return false; }

            return true;
        }
        protected void ShowPassword(object sender, EventArgs e)
        {
            if (showPass.Checked)
            {
                password.InputType = Android.Text.InputTypes.ClassText;
            }
            if (!(showPass.Checked))
            {
                password.InputType = Android.Text.InputTypes.TextVariationPassword |
                          Android.Text.InputTypes.ClassText;
            }
        }
        protected void BackInfo(object sender, EventArgs e)
        {
            Finish();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}