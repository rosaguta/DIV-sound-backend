// comment for testing
using DALTest;

namespace UserTests;

public class UserCollectionTest
{
    private IUserDal _testdal { get; set; }
    [SetUp]
    public void SetUp()
    {
        _testdal = new UserDal();
    }
    [Test]
    public void RegisterTest_AlreadyExists()
    {
        var usercollection = new UserCollection(_testdal);
        bool created = usercollection.Register("rose", "van Leeuwen", "string", "Rose@mail.com", "Rose");
        Assert.That(created , Is.EqualTo(false));
    }
}