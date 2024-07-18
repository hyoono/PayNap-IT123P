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
    [Activity(Label = "Sign Up Page")]
    public class SignUpPage : AppCompatActivity
    {
        CheckBox showPass;
        EditText mobile_num, user_name, password, confirm_pass;
        Button btn_sign_up, btn_cancel;
        String mobileNum = "", userName = "", userPass = "", passConfirm = "";
        Double accBalance;

        DatabaseClass command = new DatabaseClass();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.SignUpPageLayout);

            mobile_num = FindViewById<EditText>(Resource.Id.editText1);
            user_name = FindViewById<EditText>(Resource.Id.editText2);
            password = FindViewById<EditText>(Resource.Id.editText3);
            confirm_pass = FindViewById<EditText>(Resource.Id.editText4);

            btn_sign_up = FindViewById<Button>(Resource.Id.button1);
            btn_cancel = FindViewById<Button>(Resource.Id.button2);

            showPass = FindViewById<CheckBox>(Resource.Id.checkBox1);
            password.InputType = Android.Text.InputTypes.TextVariationPassword | Android.Text.InputTypes.ClassText;
            confirm_pass.InputType = Android.Text.InputTypes.TextVariationPassword | Android.Text.InputTypes.ClassText;

            showPass.CheckedChange += this.ShowPassword;
            btn_sign_up.Click += this.SaveUserRecord;
            btn_cancel.Click += this.BackInfoPage;
        }
        protected void SaveUserRecord(object sender, EventArgs e)
        {
            mobileNum = mobile_num.Text;
            userName = user_name.Text;
            userPass = password.Text;
            passConfirm = confirm_pass.Text;
            accBalance = 0;     //default balance

            bool validData = Validations();

            if (validData)
            {
                command.QueryCommand("http://192.168.165.158/MoneySendingApp/Functions/add_user_info.php?mobile_num=" + mobileNum + "&user_name=" + userName + "&user_password=" + passConfirm + "&acc_balance=" + accBalance);
                Toast.MakeText(this, "Sign Up Successfully!", ToastLength.Long).Show();

                mobile_num.Text = String.Empty;
                user_name.Text = String.Empty;
                password.Text = String.Empty;
                confirm_pass.Text = String.Empty;
            }
        }
        protected bool Validations()
        {
            bool stringEmpty = command.EmptyValidation(mobileNum + "," + userName + "," + userPass + "," + passConfirm);
            bool isNumValid = command.NumberValidation(mobileNum);
            bool userExist = command.SearchUserExist(mobileNum);

            if (stringEmpty)
            { Toast.MakeText(this, "Please fill all the details.", ToastLength.Long).Show(); return false; }

            else if (!isNumValid)
            { Toast.MakeText(this, "Invalid Mobile Number", ToastLength.Long).Show(); return false; }

            else if (userExist)
            { Toast.MakeText(this, "Mobile Number Already Exist.", ToastLength.Long).Show(); return false; }

            else if (userPass != passConfirm)
            { Toast.MakeText(this, "Passwords must be matched.", ToastLength.Long).Show(); return false; }

            return true;
        }
        protected void ShowPassword(object sender, EventArgs e)
        {
            if (showPass.Checked)
            {
                password.InputType = Android.Text.InputTypes.ClassText;
                confirm_pass.InputType = Android.Text.InputTypes.ClassText;
            }
            if (!(showPass.Checked))
            {
                password.InputType = Android.Text.InputTypes.TextVariationPassword |
                          Android.Text.InputTypes.ClassText;
                confirm_pass.InputType = Android.Text.InputTypes.TextVariationPassword |
                          Android.Text.InputTypes.ClassText;
            }
        }
        protected void BackInfoPage(object sender, EventArgs e)
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