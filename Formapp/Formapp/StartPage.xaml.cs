using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Data.Json;
using Xamarin.Forms;

namespace Formapp
{
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
            
        }

        public class question
        {
            public string Question { get; set; }
            public string Type { get; set; }
            public string Info { get; set; }
            public string Answer1 { get; set; }
            public string Next1 { get; set; }
            public string Answer2 { get; set; }
            public string Next2 { get; set; }
            public string Answer3 { get; set; }
            public string Next3 { get; set; }
        }

        public async Task<question> RefreshDataAsync(string url)
        {
            HttpClient client;
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            Uri uri = new Uri("http://" + url);
            var response = await client.GetAsync(uri);
            question item = new question();

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                
                item = JsonConvert.DeserializeObject<question>(content);
                return item;
            }
            return item;
        }

        private void OnButtonClicked(object sender, EventArgs args)
        {
            GetData();
        }

        private async void GetData()
        {
            //Dit is een waarde voor de database
            string id = "1";
            string url = $"172.20.42.202/app/question_get.php?ID={id}";
            
            question data = (await RefreshDataAsync(url));

            //this.FindByName<Entry>("boxQuestion").Text = data.Question;
            //boxQuestion.Text = data.Question;

            Debug.WriteLine(data);
        }

    }
}
