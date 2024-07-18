using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.Json;


namespace IT123_MP
{
    public class DatabaseClass
    {
        HttpWebResponse response;
        HttpWebRequest request;
        StreamReader reader;
        String res = "";
        string[] userData = new string[2];
        string[] userTransData;

        public void QueryCommand(string command)
        {
            request = (HttpWebRequest)WebRequest.Create(command);
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            res = reader.ReadToEnd();

            reader.Close();
            response.Close();

        }
        public StreamReader SearchCommand(string mobileNum)
        {
            request = (HttpWebRequest)WebRequest.Create("http://192.168.165.158/MoneySendingApp/Functions/search_user_record.php?mobile_num=" + mobileNum);
            response = (HttpWebResponse)request.GetResponse();
            res = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            return reader;
        }
        public bool SearchUserExist(string mobileNum)
        {
            try
            {
                StreamReader reader = SearchCommand(mobileNum);
                var result = reader.ReadToEnd();
                using JsonDocument doc = JsonDocument.Parse(result);
                JsonElement root = doc.RootElement;
                //JsonElement root = SearchCommand(mobileNum);

                var u1 = root[0];

                reader.Close();
                doc.Dispose();
                return true;
            }
            catch (System.IndexOutOfRangeException)
            {
                return false;
            }
        }
        public string[] GetUserData(string mobileNum)
        {
            StreamReader reader = SearchCommand(mobileNum);
            var result = reader.ReadToEnd();
            using JsonDocument doc = JsonDocument.Parse(result);
            JsonElement root = doc.RootElement;

            var u1 = root[0];

            string userName = u1.GetProperty("user_name").ToString();
            string userPass = u1.GetProperty("user_password").ToString();

            userData[0] = userName;
            userData[1] = userPass;

            reader.Close();
            doc.Dispose();
            return userData;
        }
        public double GetUserBalance(string mobileNum)
        {
            StreamReader reader = SearchCommand(mobileNum);
            var result = reader.ReadToEnd();
            using JsonDocument doc = JsonDocument.Parse(result);
            JsonElement root = doc.RootElement;

            var u1 = root[0];

            double acc_balance = double.Parse(u1.GetProperty("acc_balance").ToString());

            reader.Close();
            return acc_balance;
        }

        public void RecordTransaction(string mobileNum, string newBalance, string transType)
        {
            string timeStamp = DateTime.Now.ToString("yyyy/MM/dd");

            QueryCommand("http://192.168.165.158/MoneySendingApp/Functions/record_transaction.php?user_mobile_num=" + mobileNum
                + "&trans_type=" + transType + "&new_acc_bal=" + newBalance + "&trans_date=" + timeStamp);
        }
        public string[] GetUserTransHistory(string mobileNum)
        {
            string compile = "";

            try
            {
                request = (HttpWebRequest)WebRequest.Create("http://192.168.165.158/MoneySendingApp/Functions/search_trans_history.php?user_mobile_num=" + mobileNum);
                response = (HttpWebResponse)request.GetResponse();
                res = response.ProtocolVersion.ToString();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                var result = reader.ReadToEnd();
                using JsonDocument doc = JsonDocument.Parse(result);
                JsonElement root = doc.RootElement;
                int jsonLength = (root.GetArrayLength());
                userTransData = new string[jsonLength];

                for (var ctr = 0; ctr < jsonLength; ctr++)
                {
                    var u1 = root[ctr];

                    string transID = u1.GetProperty("trans_id").ToString();
                    string transType = u1.GetProperty("trans_type").ToString();
                    string newBalance = u1.GetProperty("new_acc_bal").ToString();
                    string transDate = u1.GetProperty("trans_date").ToString();

                    compile = "000" + transID + "  -  " + transType + "             " + newBalance + "      " + transDate;

                    userTransData[ctr] = compile;
                }
                response.Close();
                reader.Close();
                doc.Dispose();
            }
            catch
            {
                return null;
            }
            return userTransData;
        }
        public bool NumberValidation(string mobileNum)
        {
            try
            {
                bool starting = mobileNum.StartsWith("09");

                if (mobileNum.Length != 11 || !starting)
                { return false; }
                double check = double.Parse(mobileNum);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool EmptyValidation(string data)
        {
            string[] dataContainer = data.Split(',');
            foreach (string field in dataContainer)
            {
                if (field == "")
                {
                    return true;
                }
            }
            return false;
        }
    }
}