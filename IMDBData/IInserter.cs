using IMDBData.Models;
using System.Data.SqlClient;

namespace IMDBData
{
    public interface IInserter
    {
        object CheckObjectForNull(int? value);

       
        void Insert(List<Title> titles, SqlConnection sqlConn, SqlTransaction sqlTransaction);
    }
}