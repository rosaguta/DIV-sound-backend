namespace DALInterface;

public interface IBoardDal
{
    bool CreateBoard(string name);

    void AddFileToBoard(int audiofileid, int boardid, int userid);
     // GetBoards(int userid);
}