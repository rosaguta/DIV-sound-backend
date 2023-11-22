using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace DIV_SOUND_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        [HttpPost]
        [Route("/Boards")]
        public IActionResult CreateBoard(string name, int userid)
        {
            BoardCollection boardCollection = new BoardCollection();
            bool created  = boardCollection.CreateBoard(name, userid);
            if (created) { return Ok(); }
            return BadRequest();
        }
        [HttpGet]
        [Route("/Boards")]
        public List<Board> GetBoards(int userid)
        {
            BoardCollection boards = new BoardCollection();
            boards.GetBoards(userid);
            return boards.BoardList;
        }

        [HttpGet]
        [Route("/Boards/{Boardid}")]
        public Board GetBoard(int Boardid)
        {
            BoardCollection boardcoll = new BoardCollection();
            Board board = boardcoll.GetBoard(Boardid);
            return board;
        }
        [HttpPost]
        [Route("/Boards/{Boardid}/{AudiofileId}")]
        public IActionResult AddFiles(int userid, int Boardid, int AudiofileId) 
        {
            BoardCollection boards = new BoardCollection();
            bool created = boards.AddFilesToBoard(userid, AudiofileId, Boardid);
            if (created) { return Ok(); }
            return BadRequest();
        }
        [HttpDelete]
        [Route("/Boards/{boardid}/{audiofileid}")]
        public IActionResult DeleteFileFromBoard(int boardid, int audiofileid, int userid)
        {
            BoardCollection boardCollection = new BoardCollection();
            bool deleted = boardCollection.RemoveFileFromBoard(boardid, audiofileid, userid);
            if (deleted) { return Ok(); }
            return BadRequest();
        }
        [HttpDelete]
        [Route("/Boards/{boardid}")]
        public IActionResult DeleteBoard(int boardid)
        {
            BoardCollection boardCollection = new BoardCollection();
            bool deleted = boardCollection.DeleteBoard(boardid);
            if(deleted) { return Ok(); }
            return BadRequest();
        }

        [HttpPut]
        [Route("/Boards/{boardid}/CreateRoomSessionId")]
        public IActionResult CreateRoomSessionId(int boardid)
        {
            BoardCollection boardCollection = new BoardCollection();
            string roomid = boardCollection.CreateRoomSessionId(boardid);
            if (roomid != "")
            {
                return Ok(roomid);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("/Boards/Session/{SessionId}")]
        public Board GetBoardFromSessionId(string SessionId)
        {
            BoardCollection boardCollection = new BoardCollection();
            Board board = boardCollection.GetBoardFromSession(SessionId);
            return board;
        }


        
    }
}
