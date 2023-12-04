using DTO;

namespace DALInterface;

public interface IBoardDal
{
    bool CreateBoard(string name, int? userid);

    bool AddFileToBoard(int audiofileid, int boardid, int userid);
    List<BoardDTO> GetBoards(int userid);
    bool RemoveFileFromBoard(int boardid, int audiofileid, int userid);
    bool DeleteBoard(int boardid);
    BoardDTO GetBoard(int boardid);
    string CreateRoomSessionId(string sessionid, int boardid);
    BoardDTO GetBoardFromSession(string sessionid);
}