using DALInterface;
using DAL;

namespace Factory
{
    public static class Factory
    {
        public static IUserDal GetUserDal()
        {
            return new UserDal();
        }
        public static IAudiofileDal GetAudiofileDal()
        {
            return new AudiofileDal();
        }

        public static IBoardDal GetBoardDal()
        {
            return new BoardDal();
        }
    }
}