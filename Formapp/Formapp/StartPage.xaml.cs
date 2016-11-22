using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DBapp
{
    public partial class StartPage : ContentPage
    {
        private questionDB _questionDB;
        private answerDB _answerDB;
        int[] questionares ={0};
        static public string ip = "http://84.24.118.152";
        public user User;

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
            string url = $"/QuestionApp/login.php?User={USER}&Pass={PASS}";
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
            string url = $"/QuestionApp/lists_get.php?ID={ID}";
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            Uri uri = new Uri(ip + url);

            var response = await client.GetAsync(uri);
            string[] lijsten = new string[0];

            if (response.IsSuccessStatusCode)
            {
                List<lijst> Data;
                string raw = await response.Content.ReadAsStringAsync();
                Data = JsonConvert.DeserializeObject<List<lijst>>(raw);
                lijsten = new string[Data.Count];

                for (int i=0; i<Data.Count; i++)
                {
                    lijsten[i] = Data[i].Naam.Substring(4);
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
            _questionDB = new questionDB();
            _questionDB.empty();
            _answerDB = new answerDB();
            _answerDB.empty();

            for (var i = 0; i < lijsten.Count(); i++)
            {
                _questionDB.AddQuestions(lijsten[i]);
            }
        }

        private void OnItemTapped(object sender, EventArgs args)
        {
            string name = listAcces.SelectedItem.ToString();
            Navigation.PushModalAsync(new Question(name, _questionDB, _answerDB,User.ScreenName));
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
