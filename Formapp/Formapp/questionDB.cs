using System;
using SQLite;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Formapp
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

        public async void AddQuestion(string name)
        {
            int index = 1;
            name = "Dove";
            string url = $"/app/question_get.php?ID={index}&DB={name}";

            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            Uri uri = new Uri(ip + url);

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                Debug.WriteLine(content);
                question item = JsonConvert.DeserializeObject<question>(content);
                item.Type = types[Int32.Parse(item.Type)];
                item.Name = name;
                _connection.Insert(item);
            }
        }

        public question GetQuestion(string name,int id)
        {
            return _connection.Table<question>().FirstOrDefault(t => t.Name == name);
        }
        
        /*
        public IEnumerable<questionDB> GetQuestions()
        {
            var query = _connection.Table<questionDB>();

            return (from t in _connection.QueryAsync   Table<questionDB>()
                    select t).ToList();
        }
        
        public questionDB GetQuestion(int id)
        {
            var query = _connection.Table<questionDB>().Where ();
            return ;
        }
        
        public void DeleteQuestion(int id)
        {
            _connection.Delete<DataBase>(id);
        }

        
        public void AddQuestion(string Question)
        {
            var newQuestion = new DataBase
            {
                Question = Question,
                CreatedOn = DateTime.Now
            };

            _connection.Insert(newQuestion);
        }
        */
    }
}