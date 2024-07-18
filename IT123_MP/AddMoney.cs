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
    [Activity(Label = "@string/app_name")]
    public class AddMoney : AppCompatActivity

    {
        TextView balanceDisplay;
        EditText add_input;
        Button btn_add, btn_cancel, btn_back;
        String userMobileNum = "", add_money = "";
        double addAmount = 0;
        double userAccBalance = 0, newAccBalance = 0;

        DatabaseClass command = new DatabaseClass();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Add_Money);

            userMobileNum = Intent.GetStringExtra("UserMobileNum");

            add_input = FindViewById<EditText>(Resource.Id.editText1);

            btn_add = FindViewById<Button>(Resource.Id.button1);

            balanceDisplay = FindViewById<TextView>(Resource.Id.textView3);

            btn_add.Click += this.AddClicked;

            btn_back = FindViewById<Button>(Resource.Id.button2);

            btn_back.Click += this.BackInfo;


            GetUserBalance();
        }

        public void AddClicked(object sender, EventArgs e)
        {
            add_money = add_input.Text;

            bool validData = Validations();

            if (validData)
            {

                newAccBalance = userAccBalance + addAmount;
                command.QueryCommand("http://192.168.165.158/MoneySendingApp/Functions/update_user_balance.php?user_mobile_num=" + userMobileNum + "&user_acc_balance=" + newAccBalance);

                command.RecordTransaction(userMobileNum, newAccBalance.ToString(), "ADD MONEY");

                GetUserBalance();

                Toast.MakeText(this, $"User Transaction Complete! {userMobileNum}, {newAccBalance}", ToastLength.Long).Show();
                add_input.Text = String.Empty;
            }
        }
        protected void GetUserBalance()
        {
            userAccBalance = command.GetUserBalance(userMobileNum);
            balanceDisplay.Text = "PHP " + userAccBalance.ToString("N");
        }
        protected void BackInfo(object sender, EventArgs e)
        {
            Finish();
        }
        protected bool Validations()
        {
            bool stringEmpty = command.EmptyValidation(add_money);


            if (stringEmpty)
            { Toast.MakeText(this, "Please an amount.", ToastLength.Long).Show(); return false; }

            try
            { addAmount = double.Parse(add_money); }
            catch
            { Toast.MakeText(this, "Invalid Amount.", ToastLength.Long).Show(); return false; }

            if (addAmount > 8000)
            { Toast.MakeText(this, "NOTE: There is a 8,000 limit per add money transaction, Please try again.", ToastLength.Long).Show(); return false; }

            return true;

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}