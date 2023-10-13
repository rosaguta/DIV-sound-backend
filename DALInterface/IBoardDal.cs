namespace DALInterface;

public interface IBoardDal
{
    void CreateBoard(string name);

    void AddFileToBoard(int audiofileid, int boardid, int userid);
     // GetBoards(int userid);
}