using System.Text.Json.Nodes;
using DALInterface;
using MySql.Data.MySqlClient;
using DTO;
namespace DAL;

public class BoardDal : IBoardDal
{
    public bool CreateBoard(string name, int userid)
    {
        string? connectionstring = Getconnectionstring();
        var conn = new MySqlConnection(connectionstring);
        conn.Open();
        string query = "INSERT INTO board (name, userid) VALUES (@Name, @Userid)";
        using (var cmd = new MySqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Userid", userid);
            int rowsaffected = cmd.ExecuteNonQuery();
            return rowsaffected > 0;
        }
    }

    public bool AddFileToBoard(int fileid, int boardid, int userid)
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
            int rowsaffected = cmd.ExecuteNonQuery();
            return rowsaffected > 0;
        }
    }
    public List<BoardDTO> GetBoards(int userid)
    {
        List<BoardDTO> boards = new List<BoardDTO>();
        string? connectionstring = Getconnectionstring();
        var conn = new MySqlConnection(connectionstring);
        conn.Open();
        //string query = "SELECT * FROM board WHERE userid = @Uploaderid";
        string query = "SELECT board.id AS board_id, board.name AS board_name, " +
            "audiofile.id, audiofile.name, audiofile.duration, audiofile.uploaddate, audiofile.uploaderid, audiofile.url " +
            "FROM board " +
            "JOIN user_board_audio ON board.id = user_board_audio.boardid " +
            "JOIN audiofile ON user_board_audio.audioid = audiofile.id " +
            "WHERE user_board_audio.userid = @Userid " +
            "ORDER BY boardid ASC";
        MySqlCommand command = new MySqlCommand(query, conn);
        command.Parameters.AddWithValue("@Userid", userid);
        MySqlDataReader reader = command.ExecuteReader();
        int lastBoardId = -1;
        BoardDTO boarddto = new BoardDTO();
        while (reader.Read())
        {
            int currentBoardId = reader.GetInt32("board_id");

            if (currentBoardId != lastBoardId)
            {
                boarddto = new BoardDTO();
                boarddto.name = reader.GetString("board_name");
                boarddto.Id = currentBoardId;
                boards.Add(boarddto);
                lastBoardId = currentBoardId;
            }

            AudiofileDTO audiofiledto = new AudiofileDTO();
            audiofiledto.Id = reader.GetInt32("id");
            audiofiledto.Filename = reader.GetString("name");
            //audiofiledto.Duration = reader.GetDouble("duration");
            audiofiledto.Uploaddate = reader.GetDateTime("uploaddate");
            audiofiledto.Uploaderid = reader.GetInt32("uploaderid");
            audiofiledto.url = reader.GetString("url");
            boarddto.AudioList.Add(audiofiledto);
        }
        return boards;
    }
    public bool RemoveFileFromBoard(int boardid, int audiofileid, int userid)
    {
        string? connectionstring = Getconnectionstring();
        var conn = new MySqlConnection(connectionstring);
        conn.Open();
        string query = "DELETE FROM board_user_audio WHERE boardid = @Boardid AND audioid = @Audioid AND userid = @Userid";
        using (var cmd = new MySqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@Userid", userid);
            cmd.Parameters.AddWithValue("@Audioid", audiofileid);
            cmd.Parameters.AddWithValue("@Boardid", boardid);
            int rowsaffected = cmd.ExecuteNonQuery();
            return rowsaffected > 0;
        }
    }
    public bool DeleteBoard(int boardid) 
    {
        string? connectionstring = Getconnectionstring();
        var conn = new MySqlConnection(connectionstring);
        conn.Open();
        string query = "DELETE FROM board WHERE id = @Boardid";
        using (var cmd = new MySqlCommand(query,conn))
        {
            cmd.Parameters.AddWithValue("@Boardid", boardid);
            int rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected > 0;
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