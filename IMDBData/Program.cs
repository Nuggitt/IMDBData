
using IMDBData;
using IMDBData.Models;
using System.Data.SqlClient;

int lineCount = 0;
List<Title> titles = new List<Title>();
string filePath = "C:/IMDBData/title.basics.tsv";

foreach (string line in File.ReadLines(filePath))
{
    Console.WriteLine("Linenumber" + line);

    if (lineCount == 50000)
    {
        break;
    }

    string[] splitLine = line.Split("\t");
    if (splitLine.Length != 9)
    {
        throw new Exception("Invalid line: " + line);
    }
    string tconst = splitLine[0];
    string primaryTitle = splitLine[2];
    string originalTitle = splitLine[3];
    //Kan det måske være andet end 1 eller 0?
    bool isAdult = splitLine[4] == "1";
    int? startYear = ParseInt(splitLine[5]);
    int? endYear = ParseInt(splitLine[6]);
    int? runTimeMinutes = ParseInt(splitLine[7]);

    Title newTitle = new Title()
    {
        Tconst = tconst,
        PrimaryTitle = primaryTitle,
        OriginalTitle = originalTitle,
        IsAdult = isAdult,
        StartYear = startYear,
        EndYear = endYear,
        RunTimeMinutes = runTimeMinutes
    };

    titles.Add(newTitle);


    lineCount++;



}

Console.WriteLine("List of titles length" + titles.Count);

SqlConnection sqlConnection = new SqlConnection("server=localhost;database=IMDB;" + "user id=sa;password=Holger1208!;TrustServerCertificate=True");

sqlConnection.Open();
SqlTransaction transaction = sqlConnection.BeginTransaction();

IInserter inserter = new NormalInserter();
Console.WriteLine("Tast 1 for Normal\r\nTast 2 for prepared\r\n 3 for BulkInserter");
string input = Console.ReadLine();

switch (input)
{
    case "1":
        inserter = new NormalInserter();
        break;
    case "2":
        inserter = new PreparedInserter();
        break;
    case "3":
        inserter = new BulkInserter();
        break;
    default:
        throw new Exception("Du er en hat");
}
inserter = new PreparedInserter();

DateTime before = DateTime.Now;

try
{

    inserter.Insert(titles, sqlConnection, transaction);
    transaction.Commit();
    //transaction.Rollback();
}
catch (SqlException e)
{
    Console.WriteLine(e.Message);
    transaction.Rollback();
}


DateTime after = DateTime.Now;

sqlConnection.Close();

Console.WriteLine("Completed in miliseconds" + (after - before).TotalMilliseconds);

int? ParseInt(string value)
{
    if (value.ToLower() == "\\n") //checks if its \n
    {
        return null;
    }

    if (int.TryParse(value, out int result))
    {
        return result;
    }
    else
    {
        // Handle the case where the value is not a valid integer
        return null; // or throw an exception if appropriate
    }
}