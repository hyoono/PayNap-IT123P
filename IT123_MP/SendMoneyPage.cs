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
    [Activity(Label = "Send Money")]
    public class SendMoneyPage : AppCompatActivity
    {
        TextView viewBalance;
        EditText res_mobile_num, send_amount;
        Button btn_send_money, btn_cancel;
        String resMobileNum = "", userMobileNum = "", amount = "";
        Double sendAmount = 0;
        double userAccBalance = 0, newUserBalance = 0;

        DatabaseClass command = new DatabaseClass();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.SendMoneyLayout);

            userMobileNum = Intent.GetStringExtra("UserMobileNum");

            viewBalance = FindViewById<TextView>(Resource.Id.textView9);

            res_mobile_num = FindViewById<EditText>(Resource.Id.editText1);
            send_amount = FindViewById<EditText>(Resource.Id.editText2);

            btn_send_money = FindViewById<Button>(Resource.Id.button1);
            btn_cancel = FindViewById<Button>(Resource.Id.button2);

            CheckBalance();

            btn_send_money.Click += this.SendMoneyBtn;
            btn_cancel.Click += this.BackToMenu;
        }
        protected void SendMoneyBtn(object sender, EventArgs e)
        {
            resMobileNum = res_mobile_num.Text;
            amount = send_amount.Text;

            bool validData = Validations();

            if (validData)
            {
                double resAccBalance = command.GetUserBalance(resMobileNum);
                newUserBalance = userAccBalance - sendAmount;
                double newResBalance = resAccBalance + sendAmount;
                command.QueryCommand("http://192.168.165.158/MoneySendingApp/Functions/update_user_balance.php?user_mobile_num=" + userMobileNum + "&user_acc_balance=" + newUserBalance);
                command.QueryCommand("http://192.168.165.158/MoneySendingApp/Functions/update_user_balance.php?user_mobile_num=" + resMobileNum + "&user_acc_balance=" + newResBalance);

                RecordTransaction(newResBalance);

                Toast.MakeText(this, "User Transaction Complete!", ToastLength.Long).Show();

                CheckBalance();
            }
        }
        protected void RecordTransaction(double newResBalance)
        {
            command.RecordTransaction(userMobileNum, newUserBalance.ToString(), "SEND MONEY");
            command.RecordTransaction(resMobileNum, newResBalance.ToString(), "RECEIVE MONEY");

            res_mobile_num.Text = String.Empty;
            send_amount.Text = String.Empty;
        }
        protected bool Validations()
        {
            bool stringEmpty = command.EmptyValidation(resMobileNum + "," + sendAmount);
            bool isNumValid = command.NumberValidation(resMobileNum);
            bool userExist = command.SearchUserExist(resMobileNum);

            if (stringEmpty)
            { Toast.MakeText(this, "Please fill all the details.", ToastLength.Long).Show(); return false; }

            else if (!isNumValid)
            { Toast.MakeText(this, "Invalid Mobile Number.", ToastLength.Long).Show(); return false; }

            else if (userMobileNum == resMobileNum)
            { Toast.MakeText(this, "You can't send money to yourself.", ToastLength.Long).Show(); return false; }

            else if (!userExist)
            { Toast.MakeText(this, "Receiver Doesn't Exist.", ToastLength.Long).Show(); return false; }

            try
            { sendAmount = double.Parse(amount); }
            catch
            { Toast.MakeText(this, "Invalid Amount.", ToastLength.Long).Show(); }

            if (userAccBalance < sendAmount)
            { Toast.MakeText(this, "Not Enough Balance, Please Try Again.", ToastLength.Long).Show(); return false; }

            if (sendAmount > 10000)
            { Toast.MakeText(this, "NOTE: There is a 10,000 limit per send money transaction, Please try again.", ToastLength.Long).Show(); return false; }

            return true;
        }
        protected void CheckBalance()
        {
            userAccBalance = command.GetUserBalance(userMobileNum);
            if (userAccBalance == 0)
            {
                Toast.MakeText(this, "TIP: Please add money first to use this feature", ToastLength.Long).Show();
            }
            viewBalance.Text = "PHP " + userAccBalance.ToString("N");
        }
        protected void BackToMenu(object sender, EventArgs e)
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