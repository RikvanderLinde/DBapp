using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Formapp
{
    public partial class StartPage : ContentPage
    {
        int[] questionares ={0 };
        string ip = "http://192.168.1.100";

        public StartPage()
        {
            InitializeComponent();
        }

        public async Task<Boolean> Login(string USER , string PASS)
        {
            string url = $"/app/login.php?User={USER}&Pass={PASS}";
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            Uri uri = new Uri(ip + url);

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string raw = await response.Content.ReadAsStringAsync();

                if (raw.Contains("valid") && !raw.Contains("not"))
                    return true;
                else
                    return false;
            }
            return false;
        }
        /*
        public async Task Vragenlijst(int ID)
        {
            string url = $"/app/lists_get.php?ID={ID}";
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            Uri uri = new Uri(ip + url);

            var response = await client.GetAsync(uri);
            int[] result = {1,2,4 };
            if (response.IsSuccessStatusCode)
            {
                string raw = await response.Content.ReadAsStringAsync();
                //Data = JsonConvert.DeserializeObject<List<lijstitem>>(raw);
                listAcces.ItemsSource = result;
            }
        }*/

        private void OnButtonClicked(object sender, EventArgs args)
        {
            Login();
        }

        private void OnItemTapped(object sender, EventArgs args)
        {
            butLogin.BackgroundColor = Color.Yellow;
        }

        private async void Login()
        {
            //Boolean loggedin = (await Login("Test", "Test"));
            string user = boxUser.Text;
            string pass = boxPass.Text;
            Boolean loggedin = (await Login(user,pass));
            if (loggedin)
            {
                butLogin.BackgroundColor = Color.Green;
                int UserID = 1;
                //await Vragenlijst(UserID);
            }
            else
            {
                butLogin.BackgroundColor = Color.Red;
            }
        }
    }
}
