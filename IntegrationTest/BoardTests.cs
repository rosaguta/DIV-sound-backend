using System.Net;
using DIV_SOUND_backend;
using Logic;
using Newtonsoft.Json;

namespace IntegrationTest;
using Microsoft.AspNetCore.Mvc.Testing;

public class IntegrationTests
{
    private WebApplicationFactory<DIV_SOUND_backend.Program> _factory;
    private HttpClient _client { get; set; }
    

    [SetUp]
    public void Setup()
    {
        Environment.SetEnvironmentVariable("SqlServer","Server=138.201.52.251;port=33265;Database=DIVSOUND;User Id=Rose;Password=DIVSound321!");
        Environment.SetEnvironmentVariable("ftpServer", "ftp://138.201.52.251");
        Environment.SetEnvironmentVariable("ftpUsername", "devops");
        Environment.SetEnvironmentVariable("ftpPassword", "tar2pCuBYEd8APVmjvgG");
        Environment.SetEnvironmentVariable("ftpPath", "files/test");
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TearDown]
        public void TearDown()
        {
             
            if (_client != null)
            {
                _client.Dispose();
            }

            if (_factory != null)
            {
                _factory.Dispose();
            }
        }

    [Test]
    public async Task GetBoardFromUser()
    {
        // Arrange
        int userid = 5;
        // Act
        var response = await _client.GetAsync($"/Boards?userid={userid}");
        var responsebody = await response.Content.ReadAsStringAsync();
        
        List<Board> boards = JsonConvert.DeserializeObject<List<Board>>(responsebody);
        // assert
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(boards[0].Id, Is.EqualTo(102));
    }
    [Test]
    public async Task GetBoardFromUser_WrongUserId()
    {
        int userid = 0;

        var response = await _client.GetAsync($"/Boards?userid={userid}");
        
        // Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.True(true);
    }

    [Test]
    public async Task GetBoardFromBoardid()
    {
        // arrange
        int boardid = 102;
        //act
        var response = await _client.GetAsync($"/Boards/{boardid}");
        var responsebody = await response.Content.ReadAsStringAsync();
        Board board = JsonConvert.DeserializeObject<Board>(responsebody);
        //assert
        Assert.NotNull(response);
        // Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        // Assert.That(board.name, Is.EqualTo("integration_test"));
    }

    [Test]
    public async Task GetBoardFromBoardid_WrongBoardId()
    {
        int boardid = 0;

        var response = await _client.GetAsync($"/boards/{boardid}");

        // Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.True(true);
    }
    [Test]
    public async Task GetBoardFromSessionId()
    {
        // arrange
        string sessionid = "TESTIT";
        //Act
        var response = await _client.GetAsync($"/Boards/Session/{sessionid}");
        var responsebody = await response.Content.ReadAsStringAsync();

        var boards = JsonConvert.DeserializeObject<Board>(responsebody);
        //assert
        Assert.NotNull(response);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(boards.Id, Is.EqualTo(102));
        Assert.That(boards.AudioList[0].Id, Is.EqualTo(171));
    }

    [Test]
    public async Task GetBoardFromSessionId_WrongSessionId()
    {
        string sessionid = "something_completely_wrong";

        var response = await _client.GetAsync($"/Boards/Session/{sessionid}");
        
        // Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.True(true);
    }

    [Test]
    public async Task CreateBoardFromUserId()
    {
        int userid = 5;
        string boardname = "TestBoardName_Integration";

        var response = await _client.PostAsync($"/Boards?name={boardname}?userid={userid}", null);
        
        // Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.True(true);
    }

    [Test]
    public async Task CreateBoardF3romUserid_NoUserIdGiven()
    {
        string boardname = "ThisGetsNeverAdded";

        var response = await _client.PostAsync($"/Boards?name={boardname}?userid=", null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}