using IMDBData.Models;
using System.Data.SqlClient;

namespace IMDBData
{
    public interface IInserter
    {
    
        void Insert(List<Title> titles, SqlConnection sqlConn, SqlTransaction sqlTransaction);
    }
}