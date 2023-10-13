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

    public bool CreateBoard(string name)
    {
        bool created =_BoardDal.CreateBoard(name);
        return created;
    }
}