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
    public partial class Question : ContentPage
    {
        public Question()
        {
            InitializeComponent();
        }

        public class question
        {
            [JsonProperty(PropertyName = "Question")]
            public string Question { get; set; }
            [JsonProperty(PropertyName = "Type")]
            public string Type { get; set; }
            [JsonProperty(PropertyName = "Info")]
            public string Info { get; set; }
            [JsonProperty(PropertyName = "Answer1")]
            public string Answer1 { get; set; }
            [JsonProperty(PropertyName = "Next1")]
            public string Next1 { get; set; }
            [JsonProperty(PropertyName = "Answer2")]
            public string Answer2 { get; set; }
            [JsonProperty(PropertyName = "Next2")]
            public string Next2 { get; set; }
            [JsonProperty(PropertyName = "Answer3")]
            public string Answer3 { get; set; }
            [JsonProperty(PropertyName = "Next3")]
            public string Next3 { get; set; }
        }

        public async Task<question> RefreshDataAsync(string url)
        {
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            Uri uri = new Uri("http://" + url);

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                Debug.WriteLine(content);
                question item = JsonConvert.DeserializeObject<question>(content);
                return item;
            }
            return new question();
        }

        private void OnButtonClicked(object sender, EventArgs args)
        {
            GetData();
        }

        private async void GetData()
        {
            int id = 4;
            string url = $"localhost/app/question_get.php?ID={id}";
            question data = new question();
            data = (await RefreshDataAsync(url));
            //this.dataLabel.Text = data.Question;
        }
    }
}
