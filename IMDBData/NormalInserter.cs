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
                    "[TitleType],[PrimaryTitle],[OriginalTitle],[IsAdult],[StartYear]," +
                    "[EndYear],[RuntimeMinutes]) " +
                    "VALUES('" + title.Tconst + "'" +
                    ",'" + (title.TitleType?.Replace("'", "''")) + "'" +
                    ",'" + (title.PrimaryTitle?.Replace("'", "''")) + "'" +
                    ",'" + (title.OriginalTitle?.Replace("'", "''")) + "'" +
                    ",'" + title.IsAdult + "'" +
                    "," + CheckIntForNull(title.StartYear) +
                    "," + CheckIntForNull(title.EndYear) +
                    "," + CheckIntForNull(title.RunTimeMinutes) + ")";

                //throw new Exception(SQL);

                SqlCommand sqlComm = new SqlCommand(SQL, sqlConn, transAction);
                sqlComm.ExecuteNonQuery();
            }
        }

        public string CheckIntForNull(int? input)
        {
            if (input == null)
            {
                return "NULL";
            }
            else
            {
                return "" + input;
            }
        }
    }
}
