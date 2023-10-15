using DTO;

namespace DALInterface;

public interface IBoardDal
{
    bool CreateBoard(string name, int userid);

    void AddFileToBoard(int audiofileid, int boardid, int userid);
    List<BoardDTO> GetBoards(int userid);
}