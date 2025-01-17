﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using AndroidX.AppCompat.App;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IT123_MP
{
    [Activity(Label = "Transaction History Page")]
    public class ViewTransHistory : AppCompatActivity
    {
        ListView mainList, headerList;
        Button btn_cancel;
        String userMobileNum = "";

        DatabaseClass command = new DatabaseClass();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ViewTransHistoryLayout);

            userMobileNum = Intent.GetStringExtra("UserMobileNum");

            btn_cancel = FindViewById<Button>(Resource.Id.button2);

            btn_cancel.Click += this.BackInfoPage;

            var recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            var getTransData = command.GetUserTransHistory(userMobileNum);

            if (getTransData == null || getTransData.Count == 0)
            {
                Toast.MakeText(this, "No Transaction Record Yet", ToastLength.Long).Show();
            }
            else
            {
                   recyclerView.SetAdapter(new transactionHistoryRows(getTransData));
            }

        }

        protected void BackInfoPage(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(MenuPage));
            i.PutExtra("UserMobileNum", userMobileNum);
            StartActivity(i);
            Finish();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}