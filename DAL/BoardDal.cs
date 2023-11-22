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
            conn.Close();
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
            conn.Close();
            return rowsaffected > 0;
        }
    }
    public List<BoardDTO> GetBoards(int userid)
    {
        List<BoardDTO> boards = new List<BoardDTO>();
        string? connectionstring = Getconnectionstring();
        var conn = new MySqlConnection(connectionstring);
        conn.Open();
        string query = "SELECT board.id AS board_id, board.name AS board_name, board.sessionid AS board_session, audiofile.id AS id, audiofile.name AS name, audiofile.url AS url, audiofile.uploaderid as uploaderid, audiofile.uploaddate as uploaddate "+
        "FROM board "+ 
        "LEFT JOIN user_board_audio ON board.id = user_board_audio.boardid "+
        "LEFT JOIN audiofile ON user_board_audio.audioid = audiofile.id "+
        "WHERE board.userid = @Userid "+
        "ORDER BY board_id ASC"

        ;
        MySqlCommand command = new MySqlCommand(query, conn);
        command.Parameters.AddWithValue("@Userid", userid);
        MySqlDataReader reader = command.ExecuteReader();
        int lastBoardId = -1;
        BoardDTO boarddto = null; // Initialize it to null

        while (reader.Read())
        {
            int currentBoardId = reader.GetInt32("board_id");

            if (boarddto == null || currentBoardId != boarddto.Id)
            {
                boarddto = new BoardDTO
                {
                    name = reader.GetString("board_name"),
                    Id = currentBoardId,
                    sessionid = reader.IsDBNull(reader.GetOrdinal("board_session")) ? null : reader.GetString("board_session"),
                    AudioList = new List<AudiofileDTO>()
                };
                boards.Add(boarddto);
            }

            AudiofileDTO audiofiledto = new AudiofileDTO();
            audiofiledto.Id = reader.IsDBNull(reader.GetOrdinal("id")) ? (int?)null : reader.GetInt32("id");
            audiofiledto.Filename = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString("name");
            audiofiledto.Uploaddate = reader.IsDBNull(reader.GetOrdinal("uploaddate")) ? (DateTime?)null : reader.GetDateTime("uploaddate");
            audiofiledto.Uploaderid = reader.IsDBNull(reader.GetOrdinal("uploaderid")) ? (int?)null : reader.GetInt32("uploaderid");
            audiofiledto.url = reader.IsDBNull(reader.GetOrdinal("url")) ? null : reader.GetString("url");

            boarddto.AudioList.Add(audiofiledto);
        }
        conn.Close();
        return boards;
    }

    public BoardDTO GetBoard(int Boardid)
    {
        // BoardDTO boarddto = new BoardDTO();
        string? connectionstring = Getconnectionstring();
        var conn = new MySqlConnection(connectionstring);
        conn.Open();
        string query = "SELECT board.id AS board_id, board.name AS board_name, board.sessionid AS board_session ,audiofile.id AS id, audiofile.name AS name, audiofile.url AS url, audiofile.uploaderid as uploaderid, audiofile.uploaddate as uploaddate "+
                       "FROM board "+ 
                       "LEFT JOIN user_board_audio ON board.id = user_board_audio.boardid "+
                       "LEFT JOIN audiofile ON user_board_audio.audioid = audiofile.id "+
                       "WHERE board.id = @Boardid "+
                       "ORDER BY board_id ASC";
        MySqlCommand command = new MySqlCommand(query, conn);
        command.Parameters.AddWithValue("Boardid", Boardid);
        MySqlDataReader reader = command.ExecuteReader();
        BoardDTO boarddto = null;
        
        while (reader.Read())
        {
           
            int currentBoardId = reader.GetInt32("board_id");

            if (boarddto == null || currentBoardId != boarddto.Id)
            {
                boarddto = new BoardDTO
                {
                    name = reader.GetString("board_name"),
                    Id = currentBoardId,
                    sessionid = reader.IsDBNull(reader.GetOrdinal("board_session")) ? null : reader.GetString("board_session"),
                    AudioList = new List<AudiofileDTO>()
                };
            }
                
            

            AudiofileDTO audiofiledto = new AudiofileDTO();
            audiofiledto.Id = reader.IsDBNull(reader.GetOrdinal("id")) ? (int?)null : reader.GetInt32("id");
            audiofiledto.Filename = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString("name");
            audiofiledto.Uploaddate = reader.IsDBNull(reader.GetOrdinal("uploaddate")) ? (DateTime?)null : reader.GetDateTime("uploaddate");
            audiofiledto.Uploaderid = reader.IsDBNull(reader.GetOrdinal("uploaderid")) ? (int?)null : reader.GetInt32("uploaderid");
            audiofiledto.url = reader.IsDBNull(reader.GetOrdinal("url")) ? null : reader.GetString("url");

            boarddto.AudioList.Add(audiofiledto);
        }
        conn.Close();
        return boarddto;

    }
    public bool RemoveFileFromBoard(int boardid, int audiofileid, int userid)
    {
        string? connectionstring = Getconnectionstring();
        var conn = new MySqlConnection(connectionstring);
        conn.Open();
        string query = "DELETE FROM user_board_audio WHERE boardid = @Boardid AND audioid = @Audioid AND userid = @Userid";
        using (var cmd = new MySqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@Userid", userid);
            cmd.Parameters.AddWithValue("@Audioid", audiofileid);
            cmd.Parameters.AddWithValue("@Boardid", boardid);
            int rowsaffected = cmd.ExecuteNonQuery();
            conn.Close();
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
            conn.Close();
            return rowsAffected > 0;
        }
    }

    public string CreateRoomSessionId(string sessionid, int boardid)
    {
        string? connectionstring = Getconnectionstring();
        var conn = new MySqlConnection(connectionstring);
        conn.Open();
        string query = "UPDATE board SET sessionid = @Sessionid WHERE id = @Boardid";
        using (var cmd = new MySqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@Sessionid", sessionid);
            cmd.Parameters.AddWithValue("@Boardid", boardid);
            bool updated = cmd.ExecuteNonQuery() > 0;
            conn.Close();
            if (updated)
            {
                return sessionid;
            };
            return "";
        }
    }

    public BoardDTO GetBoardFromSession(string sessionid)
    {
        string? connectionstring = Getconnectionstring();
        var conn = new MySqlConnection(connectionstring);
        conn.Open();
        string query = "SELECT board.id AS board_id, board.name AS board_name, board.sessionid AS board_session ,audiofile.id AS id, audiofile.name AS name, audiofile.url AS url, audiofile.uploaderid as uploaderid, audiofile.uploaddate as uploaddate "+
                       "FROM board "+ 
                       "LEFT JOIN user_board_audio ON board.id = user_board_audio.boardid "+
                       "LEFT JOIN audiofile ON user_board_audio.audioid = audiofile.id "+
                       "WHERE board.sessionid = @Sessionid "+
                       "ORDER BY board_id ASC";
        
        MySqlCommand command = new MySqlCommand(query, conn);
        command.Parameters.AddWithValue("@Sessionid", sessionid);
        MySqlDataReader reader = command.ExecuteReader();
        BoardDTO boarddto = null;
        
        while (reader.Read())
        {
           
            int currentBoardId = reader.GetInt32("board_id");

            if (boarddto == null || currentBoardId != boarddto.Id)
            {
                boarddto = new BoardDTO
                {
                    name = reader.GetString("board_name"),
                    Id = currentBoardId,
                    sessionid = reader.IsDBNull(reader.GetOrdinal("board_session")) ? null : reader.GetString("board_session"),
                    AudioList = new List<AudiofileDTO>()
                };
            }
                
            

            AudiofileDTO audiofiledto = new AudiofileDTO();
            audiofiledto.Id = reader.IsDBNull(reader.GetOrdinal("id")) ? (int?)null : reader.GetInt32("id");
            audiofiledto.Filename = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString("name");
            audiofiledto.Uploaddate = reader.IsDBNull(reader.GetOrdinal("uploaddate")) ? (DateTime?)null : reader.GetDateTime("uploaddate");
            audiofiledto.Uploaderid = reader.IsDBNull(reader.GetOrdinal("uploaderid")) ? (int?)null : reader.GetInt32("uploaderid");
            audiofiledto.url = reader.IsDBNull(reader.GetOrdinal("url")) ? null : reader.GetString("url");

            boarddto.AudioList.Add(audiofiledto);
        }
        conn.Close();
        return boarddto;

        
    }
    private string? Getconnectionstring()
    {
        var jsonfile = "appsettings.json";
        var jsonObject = (JsonObject?)JsonNode.Parse(File.ReadAllText(jsonfile));
        var sqlservervalue = (string?)jsonObject["ConnectionStrings"]["SqlServer"];
        return sqlservervalue;
    }
}