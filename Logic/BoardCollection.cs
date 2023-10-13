using DALInterface;
using DAL;
using TagLib.Riff;

namespace Logic;

public class BoardCollection
{
    public List<Board> BoardList { get; set; }
    IBoardDal _BoardDal;

    public BoardCollection()
    {
        BoardList = new List<Board>();
        _BoardDal = Factory.Factory.GetBoardDal();
    }

    public void CreateBoard(string name)
    {
        _BoardDal.CreateBoard(name);
    }
}