using DALInterface;
using DAL;

namespace Factory
{
    public static class Factory
    {
        // public static IAudiofileDal getAudioFileDal()
        // {
        //     return new a
        // }
        public static IUserDal GetUserDal()
        {
            return new UserDal();
        }
        public static IAudiofileDal GetAudiofileDal()
        {
            return new AudiofileDal();
        }
    }
}