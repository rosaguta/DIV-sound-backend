using Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost]
        [Route("/Boards/{Boardid}")]
        public void AddFiles(int userid, int boardid, int AudiofileId) 
        {

        }
    }
}
