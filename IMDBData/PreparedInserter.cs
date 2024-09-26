using IMDBData.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBData
{
    public class PreparedInserter : IInserter
    {
        public PreparedInserter() { }

        public void Insert(List<Title> titles, SqlConnection sqlConn, SqlTransaction sqlTransaction)
        {
            string SqlQuery = "INSERT INTO [dbo].[Titles] ([Tconst], [TitleType], [PrimaryTitle],[OriginalTitle],[IsAdult],[StartYear],[EndYear],[RunTimeMinutes]) " +
                "VALUES (@Tconst" +
                "@TitleType" +
                "@PrimaryTitle" +
                "@OriginalTitle" +
                "@IsAdult" +
                "@StartYear" +
                "@EndYear" +
                "@RunTimeMinutes)";

            SqlCommand sqlCommand = new SqlCommand(SqlQuery, sqlConn, sqlTransaction);


            SqlParameter tconstPar = sqlCommand.Parameters.Add("@Tconst", SqlDbType.VarChar, 50);
            sqlCommand.Parameters.Add("@Tconst", SqlDbType.VarChar, 50);

            SqlParameter titleTypePar = sqlCommand.Parameters.Add("@TitleType", SqlDbType.VarChar, 50);
            sqlCommand.Parameters.Add("@TitleType", SqlDbType.VarChar, 50);

            SqlParameter primaryTitlePar = sqlCommand.Parameters.Add("@PrimaryTitle", SqlDbType.VarChar, 200);
            sqlCommand.Parameters.Add("@PrimaryTitle", SqlDbType.VarChar, 200);

            SqlParameter originalTitlePar = sqlCommand.Parameters.Add("@OriginalTitle", SqlDbType.VarChar, 200);
            sqlCommand.Parameters.Add("@OriginalTitle", SqlDbType.VarChar, 200);

            SqlParameter isAdultPar = sqlCommand.Parameters.Add("@IsAdult", SqlDbType.Bit);
            sqlCommand.Parameters.Add("@IsAdult", SqlDbType.Bit);

            SqlParameter startYearPar = sqlCommand.Parameters.Add("@StartYear", SqlDbType.Int);
            sqlCommand.Parameters.Add("@StartYear", SqlDbType.Int);

            SqlParameter endYearPar = sqlCommand.Parameters.Add("@EndYear", SqlDbType.Int);
            sqlCommand.Parameters.Add("@EndYear", SqlDbType.Int);

            SqlParameter runTimeInMinutesPar = sqlCommand.Parameters.Add("@RunTimeMinutes", SqlDbType.Int);
            sqlCommand.Parameters.Add("@RunTimeMinutes", SqlDbType.Int);


            sqlCommand.Prepare();

            foreach (Title title in titles)
            {
                tconstPar.Value = title.Tconst;
                titleTypePar.Value = title.TitleType;
                primaryTitlePar.Value = title.PrimaryTitle;
                originalTitlePar.Value = title.OriginalTitle;
                isAdultPar.Value = title.IsAdult;
                startYearPar.Value = title.StartYear;
                endYearPar.Value = title.EndYear;
                runTimeInMinutesPar.Value = title.RunTimeMinutes;




                sqlCommand.Parameters.AddWithValue("@Tconst", title.Tconst);
                sqlCommand.Parameters.AddWithValue("@TitleType", title.TitleType);
                sqlCommand.Parameters.AddWithValue("@PrimaryTitle", title.PrimaryTitle);
                sqlCommand.Parameters.AddWithValue("@OriginalTitle", title.OriginalTitle);
                sqlCommand.Parameters.AddWithValue("@IsAdult", title.IsAdult);
                sqlCommand.Parameters.AddWithValue("@StartYear", CheckObjectForNull(title.StartYear));
                sqlCommand.Parameters.AddWithValue("@EndYear", CheckObjectForNull(title.EndYear));
                sqlCommand.Parameters.AddWithValue("@RunTimeMinutes", CheckObjectForNull(title.RunTimeMinutes));


            }
        }

        public Object CheckObjectForNull(int? value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else
                return "" + value;
        }


    }
}
