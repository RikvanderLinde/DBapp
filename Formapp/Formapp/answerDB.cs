using System;
using SQLite;
using Xamarin.Forms;
using System.Net.Http;
using System.Diagnostics;

namespace DBapp
{
    public class answerDB
    {
        private SQLiteConnection _connection;
        string ip = StartPage.ip;

        public answerDB()
        {
            _connection = DependencyService.Get<ISQLite>().GetConnection();
            _connection.CreateTable<answer>();
        }

        public void Addanswer(string DBname,int id, string answer)
        {
            answer a = new answer();
            a.ID = id;
            a.Name = DBname;
            a.Answer = answer;
            _connection.Insert(a);
            getAll(DBname);
        }

        public void getAll(string DBname)
        {
            var query = _connection.Table<answer>().Where(t => t.Name == DBname);
            Debug.WriteLine("All the things :");
            foreach (var item in query)
            {
                Debug.WriteLine(item.NR+item.Name+item.Answer);
            }
            
        }
        public void empty()
        {
            _connection.DeleteAll<answer>();
        }

        public void clear(string DBname)
        {
            /*
            var query = _connection.Table<answer>().Where(t => t.Name == DBname);
            int[] answers= { };
            for (int i = 0; i < query.Count(); i++)
            {
                _connection.Delete<answer>(query.ElementAt(i).NR);
            }
            */
        }

        public async void transmit(string user,string DBname)
        {
            DateTime Time = DateTime.Now.ToLocalTime();
            var query = _connection.Table<answer>().Where(t => t.Name == DBname);

            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            foreach (var answer in query)
            {
                string url = $"/QuestionApp/answer_set.php?DB=qst_{DBname}&User={user}&Date={Time.ToString()}&Question={answer.ID}&Answer={answer.Answer}";

                Uri uri = new Uri(ip + url);

                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("Inserted"))
                    {
                        Debug.WriteLine("Inserted answer!");
                    }
                    else
                    {
                        Debug.WriteLine("Didnt insert answer...");
                    }
                }
            }
            empty();
        }
    }
}