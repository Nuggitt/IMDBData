
int lineCount = 0;
string filePath = "C:/IMDBData/title.basics.tsv";

foreach (string line in File.ReadLines(filePath))
{
    Console.WriteLine(line);

    if (lineCount++ == 50000)
    {
        break;
    }

    string[] splitLine = line.Split("\t");
    if (splitLine.Length != 9)
    {
        throw new Exception("Invalid line: " + line);
    }
    string tconst = splitLine[0];
    string primaryTitle = splitLine[1];
    string originalTitle = splitLine[2];
    bool isAdult = splitLine[3] == "1";

    lineCount++;
}