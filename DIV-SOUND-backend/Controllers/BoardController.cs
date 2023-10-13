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
        public IActionResult CreateBoard(string name)
        {
            BoardCollection boardCollection = new BoardCollection ();
            bool created  = boardCollection.CreateBoard(name);
            if (created) { return Ok(); }
            return BadRequest();
        }
    }
}
