namespace MazeGenerator.Methods
{
    public class BaseEnterExit
    {
        protected int Nlevel;
        protected int _nsection;
        protected int _globalcenter;
        protected LevelInfo _prevLevel;
        protected LevelInfo _level;
        protected BaseEnterExit(int nlevel, LevelInfo prevLevel, LevelInfo level)
        {
            Nlevel = nlevel;
            _prevLevel = prevLevel;
            _level = level;
        }

        protected BaseEnterExit(int nlevel, LevelInfo prevLevel, LevelInfo level, int nsection)
        {
            Nlevel = nlevel;
            _prevLevel = prevLevel;
            _level = level;
            _nsection = nsection;
        }
        protected BaseEnterExit(int nlevel, LevelInfo prevLevel, LevelInfo level, int nsection, int globalcenter)
        {
            Nlevel = nlevel;
            _prevLevel = prevLevel;
            _level = level;
            _nsection = nsection;
            _globalcenter = globalcenter;
        }

    }
}
