using System.Collections.Generic;
using System.Linq;

namespace MazeGenerator.Methods
{
    public class SectionExit:BaseEnterExit
    {
        public SectionExit(int nlevel, LevelInfo prevLevel, LevelInfo level) : base(nlevel, prevLevel, level)
        {
        }

        public void GenerateSectionExit()
        {
            if (Nlevel >= 1 && Nlevel < 9)
                UpDownSection();
            if (Nlevel == 9)
                RightSection();
        }

        private void RightSection()
        {
            Dictionary<int, int> right = new Dictionary<int, int>();
            for (int i = _level.LevelData.GetLength(1) - 1; i > 1; i--)
            {
                for (int j = 0; j <= _level.LevelData.GetLength(0) - 1; j++)
                {
                    if (_level.LevelData[j, i] == GenSettings.WallNumber &&
                        _level.LevelData[j, i-1] == GenSettings.FloorNumber &&
                        _level.LevelData[j-1, i] == GenSettings.WallNumber &&
                        _level.LevelData[j+1, i] == GenSettings.WallNumber &&
                        _level.LevelData[j-1, i - 1] == GenSettings.FloorNumber &&
                        _level.LevelData[j + 1, i - 1] == GenSettings.FloorNumber
                        )
                    {
                        right.Add(j, i);
                    }
                }
                if (right.Count > 0)
                    break;
            }

            if (right.Count > 0)
            {
                int x = GenSettings.Rand.Next(right.Count - 1);
                _level.LevelData[ right.ElementAt(x).Key, right.ElementAt(x).Value] = GenSettings.SectionExitNumber;
                _level.SectionExit = new Point( right.ElementAt(x).Value, right.ElementAt(x).Key);
            }
        }

        private void UpDownSection()
        {
            Dictionary<int, int> top = new Dictionary<int, int>();
            int j = (Nlevel % 2 == 1) ? (_level.LevelData.GetLength(0) - 1) : 0;
            int d = (Nlevel % 2 == 1) ? -1 : 1;
            for (; j >= 0 && j <= _level.LevelData.GetLength(0) - 1; j = j + d)
            {
                for (int i = _level.LevelData.GetLength(1) - 2; i > 1; i--)
                {
                    if (Nlevel % 2 == 1 &&
                        _level.LevelData[j, i] == GenSettings.WallNumber &&
                        _level.LevelData[j - 1, i] == GenSettings.FloorNumber &&
                        _level.LevelData[j, i + 1] == GenSettings.WallNumber &&
                        _level.LevelData[j, i - 1] == GenSettings.WallNumber &&
                        _level.LevelData[j - 1, i+1] == GenSettings.FloorNumber &&
                        _level.LevelData[j - 1, i -1] == GenSettings.FloorNumber)
                    {
                        top.Add(i, j);
                    }
                    else if (Nlevel % 2 == 0 &&
                             _level.LevelData[j, i] == GenSettings.WallNumber &&
                             _level.LevelData[j + 1, i] == GenSettings.FloorNumber &&
                             _level.LevelData[j, i + 1] == GenSettings.WallNumber &&
                             _level.LevelData[j, i - 1] == GenSettings.WallNumber &&
                             _level.LevelData[j, i - 1] == GenSettings.WallNumber &&
                             _level.LevelData[j + 1, i + 1] == GenSettings.FloorNumber &&
                             _level.LevelData[j + 1, i - 1] == GenSettings.FloorNumber)
                    {
                        top.Add(i, j);
                    }
                }
                if (top.Count > 0)
                    break;
            }

            if (top.Count > 0)
            {
                int x = GenSettings.Rand.Next(top.Count - 1);
                _level.LevelData[top.ElementAt(x).Value, top.ElementAt(x).Key] = GenSettings.SectionExitNumber;
                _level.SectionExit = new Point(top.ElementAt(x).Key, top.ElementAt(x).Value);
            }
        }
    }
}
