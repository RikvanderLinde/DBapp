using SQLite;

namespace DBapp
{
    public class answer
    {
        [PrimaryKey, AutoIncrement]
        public int NR { get; set; }

        public string Name { get; set; }
        public int ID { get; set; }
        public string Answer { get; set; }
    }
}
