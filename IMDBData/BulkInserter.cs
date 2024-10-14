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
    public class BulkInserter : IInserter
    {
        public object CheckObjectForNull(int? value)
        {
            throw new NotImplementedException();
        }

        public void Insert(List<Title> titles, SqlConnection sqlConn, SqlTransaction sqlTransaction)
        {
            // Inserting titles
            DataTable titleTable = new DataTable();

            DataColumn tconstCol = new DataColumn("tconst", typeof(string));
            DataColumn titleTypeCol = new DataColumn("TitleType", typeof(string));
            DataColumn primaryTitleCol = new DataColumn("PrimaryTitle", typeof(string));
            DataColumn originalTitleCol = new DataColumn("OriginalTitle", typeof(string));
            DataColumn isAdultCol = new DataColumn("IsAdult", typeof(bool));
            DataColumn startYearCol = new DataColumn("StartYear", typeof(int));
            DataColumn endYearCol = new DataColumn("EndYear", typeof(int));
            DataColumn runTimeMinutesCol = new DataColumn("RunTimeMinutes", typeof(int));

            titleTable.Columns.Add(tconstCol);
            titleTable.Columns.Add(titleTypeCol);
            titleTable.Columns.Add(primaryTitleCol);
            titleTable.Columns.Add(originalTitleCol);
            titleTable.Columns.Add(isAdultCol);
            titleTable.Columns.Add(startYearCol);
            titleTable.Columns.Add(endYearCol);
            titleTable.Columns.Add(runTimeMinutesCol);

            foreach (Title title in titles)
            {
                DataRow row = titleTable.NewRow();
                FillParameter(row, "Tconst", title.Tconst);
                FillParameter(row, "TitleType", title.TitleType);
                FillParameter(row, "PrimaryTitle", title.PrimaryTitle);
                FillParameter(row, "OriginalTitle", title.OriginalTitle);
                FillParameter(row, "IsAdult", title.IsAdult);
                FillParameter(row, "StartYear", title.StartYear);
                FillParameter(row, "EndYear", title.EndYear);
                FillParameter(row, "RunTimeMinutes", title.RunTimeMinutes);
                titleTable.Rows.Add(row);
            }

            SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConn, SqlBulkCopyOptions.Default, sqlTransaction);
            bulkCopy.DestinationTableName = "dbo.Titles";
            bulkCopy.WriteToServer(titleTable);

            //---------------------------------------------- GENRES ----------------------------------------------

            DataTable genreTable = new DataTable();

            DataColumn genreIDCol = new DataColumn("GenreID", typeof(int));
            DataColumn genreCol = new DataColumn("Genre", typeof(string));

            genreTable.Columns.Add(genreIDCol);
            genreTable.Columns.Add(genreCol);

            // HashSet to avoid duplicate genres
            HashSet<string> uniqueGenres = new HashSet<string>();
            int genreIDCounter = 1;

            // Assuming Title.Genres is a comma-separated string of genres
            foreach (Title title in titles)
            {
                if (!string.IsNullOrWhiteSpace(title.Genre))
                {
                    string[] genres = title.Genre.Split(',');

                    foreach (string genre in genres.Select(g => g.Trim()).Distinct())
                    {
                        if (!uniqueGenres.Contains(genre))
                        {
                            DataRow row = genreTable.NewRow();
                            row["GenreID"] = genreIDCounter++;
                            row["Genre"] = genre;
                            genreTable.Rows.Add(row);
                            uniqueGenres.Add(genre);
                        }
                    }
                }
            }

            SqlBulkCopy bulkCopyGenre = new SqlBulkCopy(sqlConn, SqlBulkCopyOptions.Default, sqlTransaction);
            bulkCopyGenre.DestinationTableName = "dbo.Genres";
            bulkCopyGenre.WriteToServer(genreTable);

            //---------------------------------------------- TITLEGENRES ----------------------------------------------



            DataTable titleGenreTable = new DataTable();

            DataColumn titleGenreTconstCol = new DataColumn("Tconst", typeof(string));
            DataColumn titleGenreIDCol = new DataColumn("GenreID", typeof(int));

            titleGenreTable.Columns.Add(titleGenreTconstCol);
            titleGenreTable.Columns.Add(titleGenreIDCol);

            foreach(Title title in titles)
            {
                DataRow row = titleGenreTable.NewRow();
                FillParameter(row, "Tconst", title.Tconst);
                FillParameter(row, "GenreID", title.GenreID);
                titleGenreTable.Rows.Add(row);

            }

            SqlBulkCopy bulkCopyTitleGenre = new SqlBulkCopy(sqlConn, SqlBulkCopyOptions.Default, sqlTransaction);
            bulkCopy.DestinationTableName = "dbo.TitleGenres";
            bulkCopy.WriteToServer(titleTable);


        }


        public void FillParameter(DataRow row, string columnName, object? value)
        {
            if (value != null)
            {
                row[columnName] = value;
            }
            else
            {
                row[columnName] = DBNull.Value;
            }
        }
    }
}
