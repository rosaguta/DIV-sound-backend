using DALInterface;
using DAL;
using TagLib.Riff;
using DTO;
using Logic.Mapper;

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

    public bool CreateBoard(string name, int userid)
    {
        bool created =_BoardDal.CreateBoard(name, userid);
        return created;
    }
    public List<Board> GetBoards(int userid)
    {
        List<BoardDTO> boardsDTO = _BoardDal.GetBoards(userid);
        foreach(BoardDTO boardDTO in boardsDTO) 
        {
            BoardList.Add(boardDTO.ConvertToLogic());
        }
        return BoardList;
    }
}