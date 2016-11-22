using System;
using SQLite;

namespace DBapp
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}