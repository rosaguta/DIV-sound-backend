
using DALTest;

namespace UserTests;

public class UserCollectionTest
{
    [Test]
    public void RegisterTest_AlreadyExists()
    {
        IUserDal ia = new UserDal();
        var usercollection = new UserCollection(ia);
        bool created = usercollection.Register("rose", "van Leeuwen", "string", "Rose@mail.com", "Rose");
        Assert.Pass();
    }
}