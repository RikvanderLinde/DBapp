using Xamarin.Forms;

namespace Formapp
{
    public partial class Question : ContentPage
    {
        string name = "";
        int index = 1;
        questionDB _database;
        public Question(string Name, questionDB database)
        {
            InitializeComponent();
            name = Name;
            _database = database;
            MakeQuestion(index);
        }
        
        private void MakeQuestion(int index)
        {
            question quest = new question();
            quest = _database.GetQuestion(name,index);

            Entry ent = new Entry();

            //boxQuestion.Text = quest.Question;
            //boxInfo.Text = quest.Info;

            //"Open", "Ok", "Yes/No", "Multiple", "Picture"
            StackLayout layout = new StackLayout();

            //Premade layout
            Label question = new Label();
            question.Text = quest.Question;
            layout.Children.Add(question);

            Label info = new Label();
            question.Text = quest.Info;
            layout.Children.Add(info);

            //Special Layout
            if (quest.Type == "Open")
            {
                Button btn = new Button();
                btn.Clicked += (sender, e) => MakeQuestion(quest.Next1);
                btn.Text = quest.Answer1;
                layout.Children.Add(btn);
            }

            if (quest.Type == "Ok")
            {
                Button btn = new Button();
                btn.Clicked += (sender, e) => MakeQuestion(quest.Next1);
                btn.Text = quest.Answer1;
                layout.Children.Add(btn);
            }

            if (quest.Type == "Yes/No")
            {
                Button btn1 = new Button();
                btn1.Clicked += (sender, e) => MakeQuestion(quest.Next1);
                btn1.Text = quest.Answer1;
                layout.Children.Add(btn1);

                Button btn2 = new Button();
                btn2.Clicked += (sender, e) => MakeQuestion(quest.Next2);
                btn2.Text = quest.Answer2;
                layout.Children.Add(btn2);
            }

            if (quest.Type == "Multiple")
            {
                Button btn1 = new Button();
                btn1.Clicked += (sender, e) => MakeQuestion(quest.Next1);
                btn1.Text = quest.Answer1;
                layout.Children.Add(btn1);

                Button btn2 = new Button();
                btn2.Clicked += (sender, e) => MakeQuestion(quest.Next2);
                btn2.Text = quest.Answer2;
                layout.Children.Add(btn2);

                Button btn3 = new Button();
                btn3.Clicked += (sender, e) => MakeQuestion(quest.Next3);
                btn3.Text = quest.Answer3;
                layout.Children.Add(btn3);
            }

            if (quest.Type == "Picture")
            {
                Button btn = new Button();
                btn.Clicked += (sender, e) => MakeQuestion(quest.Next1);
                btn.Text = quest.Answer1;
                layout.Children.Add(btn);
            }
            //Set layout
            
            //Resources["StackLayoutQuestion"] = layout;
        }
    }
}
