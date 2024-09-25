using IMDBData.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace IMDBData
{
    public class NormalInserter : IInserter
    {
        public NormalInserter() { }

        public void Insert(List<Title> titles, SqlConnection sqlConn, SqlTransaction transAction)
        {
            foreach (Title title in titles)
            {
                string SQL = "INSERT INTO [Titles]([TConst]," +
                    "[PrimaryTitle],[OriginalTitle],[IsAdult],[StartYear]," +
                    "[EndYear],[RuntimeMinutes]) " +
                    "VALUES('" + title.Tconst + "'" +
                    ",'" + title.PrimaryTitle.Replace("'", "''") + "'" +
                    ",'" + title.OriginalTitle.Replace("'", "''") + "'" +
                    ",'" + title.IsAdult + "'" +
                    "," + CheckObjectForNull(title.StartYear) +
                    "," + CheckObjectForNull(title.EndYear) +
                    "," + CheckObjectForNull(title.RunTimeMinutes) + ")";

                //throw new Exception(SQL);

                SqlCommand sqlComm = new SqlCommand(SQL, sqlConn, transAction);
                sqlComm.ExecuteNonQuery();
            }
        }

        public object CheckObjectForNull(int? value)
        {
            if (value == null)
            {
                return "NULL";
            }
            return value.ToString();
        }
    }
}
