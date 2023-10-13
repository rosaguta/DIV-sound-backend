using System.Text.Json.Nodes;
using DALInterface;
using MySql.Data.MySqlClient;

namespace DAL;

public class BoardDal : IBoardDal
{
    public void CreateBoard(string name)
    {
        string? connectionstring = Getconnectionstring();
        var conn = new MySqlConnection(connectionstring);
        conn.Open();
        string query = "INSERT INTO board (name) VALUES (@Name)";
        using (var cmd = new MySqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.ExecuteNonQuery();
        }
    }

    public void AddFileToBoard(int fileid, int boardid, int userid)
    {
        string? connectionstring = Getconnectionstring();
        var conn = new MySqlConnection(connectionstring);
        conn.Open();
        string query = "INSERT INTO user_board_audio (userid, audioid, boardid) VALUES (@Userid, @Audioid, @Boardid)";
        using (var cmd = new MySqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@Userid", userid);
            cmd.Parameters.AddWithValue("@Audioid", fileid);
            cmd.Parameters.AddWithValue("@Boardid", boardid);
            cmd.ExecuteNonQuery();
        }
    }
    private string? Getconnectionstring()
    {
        var jsonfile = "appsettings.json";
        var jsonObject = (JsonObject?)JsonNode.Parse(File.ReadAllText(jsonfile));
        var sqlservervalue = (string?)jsonObject["ConnectionStrings"]["SqlServer"];
        return sqlservervalue;
    }
}