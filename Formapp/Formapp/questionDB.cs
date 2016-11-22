using System;
using SQLite;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;

namespace DBapp
{
    public class questionDB
    {
        private SQLiteConnection _connection;
        string ip = StartPage.ip;
        string[] types = { "Open", "Ok", "Yes/No", "Multiple", "Picture" };

        public questionDB()
        {
            _connection = DependencyService.Get<ISQLite>().GetConnection();
            _connection.CreateTable<question>();
        }

        public async void AddQuestions(string name)
        {
            string url = $"/QuestionApp/question_get.php?DB=qst_{name}";

            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            Uri uri = new Uri(ip + url);

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<question> items = JsonConvert.DeserializeObject<List<question>>(content);
                foreach (question a in items)
                {
                a.Type = types[Int32.Parse(a.Type)];
                a.Name = name;
                _connection.Insert(a);
                }
            }
        }

        public question GetQuestion(string name,int id)
        {
            return _connection.Table<question>().First(t => t.Name == name && t.ID == id);
        }

        public int GetFirst(string name)
        {
            return _connection.Table<question>().First(t => t.Name == name).ID;
        }

        public void empty()
        {
            _connection.DeleteAll<question>();
        }
    }
}