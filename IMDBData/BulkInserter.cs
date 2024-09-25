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
            DataTable titleTable = new DataTable();

            DataColumn tconstCol = new DataColumn("tconst", typeof(string));
            DataColumn primaryTitleCol = new DataColumn("PrimaryTitle", typeof(string));
            DataColumn originalTitleCol = new DataColumn("OriginalTitle", typeof(string));
            DataColumn isAdultCol = new DataColumn("IsAdult", typeof(bool));
            DataColumn startYearCol = new DataColumn("StartYear", typeof(int));
            DataColumn endYearCol = new DataColumn("EndYear", typeof(int));
            DataColumn runTimeinMinutesCol = new DataColumn("RunTimeinMinutes", typeof(int));

            titleTable.Columns.Add(tconstCol);
            titleTable.Columns.Add(primaryTitleCol);
            titleTable.Columns.Add(originalTitleCol);
            titleTable.Columns.Add(isAdultCol);
            titleTable.Columns.Add(startYearCol);
            titleTable.Columns.Add(endYearCol);
            titleTable.Columns.Add(runTimeinMinutesCol);

            foreach (Title title in titles)
            {
                DataRow row = titleTable.NewRow();
                FillParameter(row, "Tconst", title.Tconst);
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


        }

        public void FillParameter(DataRow row,
            string columnName,
            object? value)
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
