using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Configuration;
using System.Data;
using Dapper;

namespace FuHello
{
    public static class SQLiteHelper
    {
        public static string conStr = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;

        private static SQLiteConnection Connection;

        private static SQLiteConnection GetSQLiteConnection()
        {
            if (Connection == null || Connection.State != ConnectionState.Open)
            {
                Connection = new SQLiteConnection(conStr);
                Connection.Open();
            }

            return Connection;
        }

        private static SQLiteConnection GetNewSQLiteConnection()
        {
            return new SQLiteConnection(conStr);
        }

        public static int ExecuteNoNQuery(string sql, params SQLiteParameter[] sParameters)
        {
            using (SQLiteConnection con = new SQLiteConnection(conStr))
            {
                con.Open();
                SQLiteCommand cmd = con.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(sParameters);
                return cmd.ExecuteNonQuery();
            }
        }

        public static T ExecuteScalar<T>(string sql, params SQLiteParameter[] sParameters)
        {
            using (SQLiteConnection con = new SQLiteConnection(conStr))
            {
                con.Open();
                SQLiteCommand cmd = new SQLiteCommand(sql, con);
                cmd.Parameters.AddRange(sParameters);
                return (T)cmd.ExecuteScalar();
            }
        }

        public static DataTable GetDataTable(string sql, params SQLiteParameter[] sParameters)
        {
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, GetSQLiteConnection());
            adapter.SelectCommand.Parameters.AddRange(sParameters);
            DataTable td = new DataTable();
            adapter.Fill(td);
            return td;
        }

        public static SQLiteDataReader ExecuteReader(string sql, params SQLiteParameter[] sParameters)
        {
            SQLiteConnection con = new SQLiteConnection(conStr);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, con);
            cmd.Parameters.AddRange(sParameters);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static List<T> GetDataTable2<T>(string sql, params SQLiteParameter[] sParameters)
        {
            var res = GetSQLiteConnection().Query<T>(sql, sParameters).ToList();
            
            return res;
        }
    }
}