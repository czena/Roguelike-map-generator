namespace MazeGenerator.Methods
{
    /// <summary>
    /// Поиск короткого пути
    /// </summary>
    public class ShortPath
    {
        public int Path { get; set; }
        private bool _add = true;
        private int _x;
        private int _y;
        private int _step;
        private int _width;
        private int _height;
        private int[,] _cMap;

        public bool CorrectPath(LevelInfo level)
        {
            FindWave(level);
            if (Path < GenSettings.MinPath)
                return false;
            else
                return true;
        }

        private void FindWave(LevelInfo level)
        {
            _width = (int) level.LevelData.GetLongLength(1);
            _height = (int) level.LevelData.GetLongLength(0);
            _cMap = new int[_height, _width];
            for (_y = 0; _y < _height; _y++)
            for (_x = 0; _x < _width; _x++)
            {
                if (level.LevelData[_y, _x] == GenSettings.WallNumber || level.LevelData[_y, _x] == null)
                    _cMap[_y, _x] = -2; //стена
                else
                    _cMap[_y, _x] = -1; //индикатор еще не ступали сюда
            }

            _cMap[level.Exit.Y, level.Exit.X] = 0; //Начинаем с финиша
            while (_add)
            {
                _add = false;
                for (_y = 0; _y < _height; _y++)
                for (_x = 0; _x < _width; _x++)
                {
                    if (_cMap[_y, _x] == _step)
                    {
                        //Ставим значение шага+1 в соседние ячейки (если они проходимы)
                        if (_x - 1 >= 0 && _cMap[_y, _x - 1] == -1)
                            _cMap[_y, _x - 1] = _step + 1;
                        if (_y - 1 >= 0 && _cMap[_y - 1, _x] == -1)
                            _cMap[_y - 1, _x] = _step + 1;
                        if (_x + 1 < _width && _cMap[_y, _x + 1] == -1)
                            _cMap[_y, _x + 1] = _step + 1;
                        if (_y + 1 < _height && _cMap[_y + 1, _x] == -1)
                            _cMap[_y + 1, _x] = _step + 1;
                    }
                }

                _step++;
                _add = true;

                if (_cMap[level.Enter.Y, level.Enter.X] != -1) //решение найдено
                    _add = false;
                if (_step > _height * _width) //решение не найдено
                    _add = false;
            }
            Path = _cMap[level.Enter.Y, level.Enter.X];
        }
        public void DisplayPath(LevelInfo level)
        { 
        //Отрисовываем карты
            int path = Path;
            _x = level.Enter.X;
            _y = level.Enter.Y;
            for (path = path - 1; path > -1; path--)
            {
                if (_x - 1 >= 0 && _cMap[_y, _x - 1] == path)
                {
                    level.LevelData[_y, _x - 1] = 4;
                    _x--;
                }
                else if (_x + 1 < level.LevelData.GetLongLength(1) && _cMap[_y, _x + 1] == path)
                {
                    level.LevelData[_y, _x + 1] = 4;
                    _x++;
                }
                else if (_y + 1 < level.LevelData.GetLongLength(0) && _cMap[_y + 1, _x] == path)
                {
                    level.LevelData[_y + 1, _x] = 4;
                    _y++;
                }
                else if (_y - 1 >= 0 && _cMap[_y - 1, _x] == path)
                {
                    level.LevelData[_y - 1, _x] = 4;
                    _y--;
                }
            }
        }
    }
}
