using System;
using SQLite;

namespace Formapp
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}