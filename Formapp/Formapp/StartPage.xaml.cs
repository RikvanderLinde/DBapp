using Newtonsoft.Json;
using SQLite;
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
        private questionDB _database;
        int[] questionares ={0};
        static public string ip = "http://192.168.1.101";
        user User;

        public StartPage()
        {
            InitializeComponent();
        }

        public class lijst
        {
            [JsonProperty(PropertyName = "Name")]
            public string Naam { get; set; }
            [JsonProperty(PropertyName = "State")]
            public int State { get; set; }
        }

        public class user
        {
            [JsonProperty(PropertyName = "Validation")]
            public string Validation { get; set; }
            [JsonProperty(PropertyName = "ID")]
            public int ID { get; set; }
            [JsonProperty(PropertyName = "ScreenName")]
            public string ScreenName { get; set; }
            [JsonProperty(PropertyName = "Gender")]
            public string Gender { get; set; }
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
                User = JsonConvert.DeserializeObject<user>(raw);
                if (User.Validation == "True")
                    return true;
                else
                    return false;
            }
            return false;
        }
        
        public async Task<string[]> Vragenlijst(int ID)
        {
            string url = $"/app/lists_get.php?ID={ID}";
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            Uri uri = new Uri(ip + url);

            var response = await client.GetAsync(uri);
            int[] result = {1,2,4 };
            string[] lijsten = new string[0];

            if (response.IsSuccessStatusCode)
            {
                List<lijst> Data;
                string raw = await response.Content.ReadAsStringAsync();
                Data = JsonConvert.DeserializeObject<List<lijst>>(raw);
                lijsten = new string[Data.Count];

                for (int i=0; i<Data.Count; i++)
                {
                    lijsten[i] = Data[i].Naam;
                }
                return lijsten;
            }
            return lijsten;
        }
        
        private void OnButtonClicked(object sender, EventArgs args)
        {
            Login();
        }

        private void MakeStorage(string[] lijsten)
        {
            _database = new questionDB();
            _database.AddQuestion(lijsten[0]);
            _database.AddQuestion(lijsten[1]);
            _database.AddQuestion(lijsten[2]);
        }

        private void OnItemTapped(object sender, EventArgs args)
        {

            string name = listAcces.SelectedItem.ToString();
            Navigation.PushModalAsync(new Question(name, _database));

        }

        private async void Login()
        {
            string user = boxUser.Text;
            string pass = boxPass.Text;

            if (await Login(user, pass))
            {
                butLogin.BackgroundColor = Color.Green;
                int UserID = User.ID;
                string[] lijsten = await Vragenlijst(UserID);
                listAcces.ItemsSource = lijsten;
                MakeStorage(lijsten);
            }
            else
            {
                butLogin.BackgroundColor = Color.Red;
            }
        }
    }
}
