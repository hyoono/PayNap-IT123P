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
    [Activity(Label = "Edit Profile Page")]
    public class EditProfile : AppCompatActivity
    {
        CheckBox changeName, showPass, changePass;
        EditText mobileNum, edit_acc_name, user_pass, edit_pass, confirm_pass;
        Button btn_confirm, btn_cancel;
        String userMobileNum = "", accName = "", userPass = "", newPass = "", confirmPass = "";
        String[] userData;

        DatabaseClass command = new DatabaseClass();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.EditProfileLayout);

            userMobileNum = Intent.GetStringExtra("UserMobileNum");

            mobileNum = FindViewById<EditText>(Resource.Id.editText1);
            edit_acc_name = FindViewById<EditText>(Resource.Id.editText2);
            user_pass = FindViewById<EditText>(Resource.Id.editText3);
            edit_pass = FindViewById<EditText>(Resource.Id.editText4);
            confirm_pass = FindViewById<EditText>(Resource.Id.editText5);

            changeName = FindViewById<CheckBox>(Resource.Id.checkBox1);
            changePass = FindViewById<CheckBox>(Resource.Id.checkBox2);
            showPass = FindViewById<CheckBox>(Resource.Id.checkBox3);

            btn_confirm = FindViewById<Button>(Resource.Id.button1);
            btn_cancel = FindViewById<Button>(Resource.Id.button2);

            edit_acc_name.Enabled = false;
            user_pass.Enabled = false;
            edit_pass.Enabled = false;
            confirm_pass.Enabled = false;
            showPass.Enabled = false;

            ReloadUserName();

            changeName.CheckedChange += this.ChangeName;
            changePass.CheckedChange += this.ChangePassword;
            showPass.CheckedChange += this.ShowPassword;

            btn_confirm.Click += this.ConfirmChanges;
            btn_cancel.Click += this.BackToMenu;
        }
        protected void ConfirmChanges(object sender, EventArgs e)
        {
            accName = edit_acc_name.Text;
            userPass = user_pass.Text;
            newPass = edit_pass.Text;
            confirmPass = confirm_pass.Text;

            if (changeName.Checked)
            {
                if (accName == String.Empty)
                { Toast.MakeText(this, "Please enter a new name", ToastLength.Long).Show(); return; }
                else
                {
                    command.QueryCommand("http://172.18.13.160/MoneySendingApp/Functions/update_user_name.php?user_mobile_num=" + userMobileNum + "&user_name=" + accName);
                    Toast.MakeText(this, "Username has been updated!", ToastLength.Long).Show();

                    ReloadUserName();
                    changeName.Checked = false;
                }
            }

            if (changePass.Checked)
            {
                bool validData = Validations();
                if (validData)
                {
                    command.QueryCommand("http://172.18.13.160/MoneySendingApp/Functions/update_user_password.php?user_mobile_num=" + userMobileNum + "&user_password=" + confirmPass);
                    Toast.MakeText(this, "Password has been updated!", ToastLength.Long).Show();

                    changePass.Checked = false;
                }
            }
        }
        protected bool Validations()
        {
            bool stringEmpty = command.EmptyValidation(userPass + "," + newPass + "," + confirmPass);

            if (stringEmpty)
            { Toast.MakeText(this, "Please fill all the details.", ToastLength.Long).Show(); return false; }

            else if (userPass.ToString() != userData[1].ToString())
            { Toast.MakeText(this, $"Incorrect Current Password {userData[1]}, {userPass}", ToastLength.Long).Show(); return false; }

            else if (newPass != confirmPass)
            { Toast.MakeText(this, "New Passwords must be matched.", ToastLength.Long).Show(); return false; }
            return true;
        }
        protected void ChangeName(object sender, EventArgs e)
        {
            if (changeName.Checked)
            {
                edit_acc_name.Enabled = true;
                changePass.Enabled = false;
            }
            if (!(changeName.Checked))
            {
                edit_acc_name.Text = userData[0];
                edit_acc_name.Enabled = false;
                changePass.Enabled = true;
            }
        }
        protected void ChangePassword(object sender, EventArgs e)
        {
            if (changePass.Checked)
            {
                changeName.Enabled = false;
                user_pass.Enabled = true;
                edit_pass.Enabled = true;
                confirm_pass.Enabled = true;
                showPass.Enabled = true;
            }
            if (!(changePass.Checked))
            {
                user_pass.Text = "";
                edit_pass.Text = "";
                confirm_pass.Text = "";
                changeName.Enabled = true;
                user_pass.Enabled = false;
                edit_pass.Enabled = false;
                confirm_pass.Enabled = false;
                showPass.Enabled = false;
            }
        }
        protected void ShowPassword(object sender, EventArgs e)
        {
            if (showPass.Checked)
            {
                edit_pass.InputType = Android.Text.InputTypes.ClassText;
            }
            if (!(showPass.Checked))
            {
                edit_pass.InputType = Android.Text.InputTypes.TextVariationPassword |
                          Android.Text.InputTypes.ClassText;
            }
        }
        protected void ReloadUserName()
        {
            userData = command.GetUserData(userMobileNum);

            mobileNum.Text = userMobileNum;
            edit_acc_name.Text = userData[0];
        }
        protected void BackToMenu(object sender, EventArgs e)
        {
            Intent myIntent = new Intent(this, typeof(MenuPage));
            myIntent.PutExtra("UserMobileNum", userMobileNum);
            SetResult(Result.Ok, myIntent);
            Finish();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}