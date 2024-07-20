using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static AndroidX.RecyclerView.Widget.RecyclerView;

namespace IT123_MP
{
    internal class transactionHistoryRows : RecyclerView.Adapter
    {
        private readonly List<string[]> _items;

        public transactionHistoryRows(List<string[]> items)
        {
            _items = items;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.transactionRowLayout, parent, false);
            return new MyViewHolder(itemView);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as MyViewHolder;
            var item = _items[position];

            viewHolder.Column1.Text = item[0];
            viewHolder.Column2.Text = item[1];
            viewHolder.Column3.Text = item[2];
            viewHolder.Column4.Text = item[3];
            // Bind more columns as needed
        }

        public override int ItemCount => _items.Count;

        public class MyViewHolder : RecyclerView.ViewHolder
        {
            public TextView Column1 { get; }
            public TextView Column2 { get; }
            public TextView Column3 { get; }
            public TextView Column4 { get; }
            // Add more TextView properties for additional columns

            public MyViewHolder(View itemView) : base(itemView)
            {
                Column1 = itemView.FindViewById<TextView>(Resource.Id.column1);
                Column2 = itemView.FindViewById<TextView>(Resource.Id.column2);
                Column3 = itemView.FindViewById<TextView>(Resource.Id.column3);
                Column4 = itemView.FindViewById<TextView>(Resource.Id.column4);
                // Initialize more TextView properties for additional columns
            }
        }
    }
}