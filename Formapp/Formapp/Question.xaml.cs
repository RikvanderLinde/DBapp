using Xamarin.Forms;

namespace DBapp
{
    public partial class Question : ContentPage
    {
        string name = "";
        questionDB _questionDB;
        answerDB _answerDB;
        string ScreenName;

        public Question(string Name, questionDB qst,answerDB ans, string User)
        {
            InitializeComponent();
            name = Name;
            _questionDB = qst;
            _answerDB = ans;
            ScreenName = User;
            int index = _questionDB.GetFirst(name);
            MakeQuestion(index);
        }
        
        private void GetAnswer(int question, int next,string answer)
        {
            _answerDB.Addanswer(name, question, answer);
            MakeQuestion(next);
        }

        private async void Send()
        {
            _answerDB.transmit(ScreenName, name);
            await Navigation.PopModalAsync();
        }

        private void MakeQuestion(int index)
        {
            question quest = new question();
            if (index > 0)
            {
                quest = _questionDB.GetQuestion(name, index);
            }
            else
            {
                quest.Type = "Send";
            }
            // 0    ,  1  ,  2      ,  3        ,  4
            //"Open", "Ok", "Yes/No", "Multiple", "Picture"
            StackLayout layout = new StackLayout();

            //Premade layout
            Label question = new Label();
            question.Text = quest.Question;
            layout.Children.Add(question);

            Label info = new Label();
            info.Text = quest.Description;
            layout.Children.Add(info);

            //Special Layout
            switch (quest.Type)
            {
                case "Send":
                    {
                        Button btn = new Button();
                        btn.Clicked += (sender, e) => Send();
                        btn.Text = "Send results";
                        layout.Children.Add(btn);
                        break;
                    }
                case "Ok":
                    {
                        Button btn = new Button();
                        btn.Clicked += (sender, e) => GetAnswer(quest.ID, quest.Next1, "Nothing");
                        btn.Text = "Ok";
                        layout.Children.Add(btn);
                        break;
                    }
                case "Yes/No":
                    {
                        Button btn1 = new Button();
                        btn1.Clicked += (sender, e) => GetAnswer(quest.ID, quest.Next1,"Yes");
                        btn1.Text = "Yes";
                        layout.Children.Add(btn1);

                        Button btn2 = new Button();
                        btn2.Clicked += (sender, e) => GetAnswer(quest.ID, quest.Next2,"No");
                        btn2.Text = "No";
                        layout.Children.Add(btn2);
                        break;
                    }
                case "Open":
                    {
                        Entry Entry = new Entry();
                        Entry.Placeholder = quest.Answer1;
                        layout.Children.Add(Entry);

                        Button btn = new Button();
                        btn.Clicked += (sender, e) => GetAnswer(quest.ID, quest.Next1,Entry.Text);
                        btn.Text = "Send";
                        layout.Children.Add(btn);
                        break;
                    }
                case "Multiple":
                    {
                        Button btn1 = new Button();
                        btn1.Clicked += (sender, e) => GetAnswer(quest.ID, quest.Next1, quest.Answer1);
                        btn1.Text = quest.Answer1;
                        layout.Children.Add(btn1);

                        Button btn2 = new Button();
                        btn2.Clicked += (sender, e) => GetAnswer(quest.ID, quest.Next2, quest.Answer2);
                        btn2.Text = quest.Answer2;
                        layout.Children.Add(btn2);

                        Button btn3 = new Button();
                        btn3.Clicked += (sender, e) => GetAnswer(quest.ID, quest.Next3, quest.Answer3);
                        btn3.Text = quest.Answer3;
                        layout.Children.Add(btn3);
                        break;
                    }
                case "Picture":
                    {
                        Button btn = new Button();
                        btn.Clicked += (sender, e) => GetAnswer(quest.ID, quest.Next1,"Link to picture");
                        btn.Text = quest.Answer1;
                        layout.Children.Add(btn);
                        break;
                    }
                default:
                    break;
            }
            //Set layout
            this.Content = layout;
        }
    }
}
