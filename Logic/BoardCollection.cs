using DALInterface;
using DAL;
using TagLib.Riff;
using DTO;
using Logic.Mapper;
using FFmpeg.AutoGen;

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
    public bool AddFilesToBoard(int userid, int audiofile, int boardid)
    {
        bool updated = _BoardDal.AddFileToBoard(audiofile, boardid, userid);
        return updated;
    }
    public bool RemoveFileFromBoard(int boardid, int audiofileid, int userid)
    {
        bool deleted = _BoardDal.RemoveFileFromBoard(boardid, audiofileid, userid);
        return deleted;
    }
    public bool DeleteBoard(int boardid)
    {
        bool deleted = _BoardDal.DeleteBoard(boardid);
        return deleted;
    }
}