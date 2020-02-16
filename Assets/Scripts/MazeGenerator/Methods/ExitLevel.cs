using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGenerator.Methods
{
    public class ExitLevel : BaseEnterExit
    {
        private Dictionary<int, int> _exitList;

        public ExitLevel(int nlevel, LevelInfo prevLevel, LevelInfo level, int nsection) : base(nlevel, prevLevel,
            level, nsection)
        {
        }

        public void GenerateExit()
        {
            _exitList = new Dictionary<int, int>();
            switch (_nsection)
            {
                case 1:
                    if (Nlevel >= 1 && Nlevel < 9)
                        ExitUpDownRightField();
                    else if (Nlevel == 9)
                        ExitUpField();
                    break;
                case 2:
                    if (Nlevel < 3)
                        ExitUpField();
                    else if (Nlevel > 2)
                        ExitLeftField();
                    break;
                case 3:
                    if (Nlevel < 3)
                        ExitDownField();
                    else if (Nlevel > 2)
                        ExitLeftField();
                    break;
                case 4:
                    if (Nlevel < 5)
                        ExitUpField();
                    else if (Nlevel > 4)
                        ExitLeftField();
                    break;
                case 5:
                    if (Nlevel < 5)
                        ExitDownField();
                    else if (Nlevel > 4)
                        ExitLeftField();
                    break;
                case 6:
                    if (Nlevel < 5)
                        ExitUpField();
                    else if (Nlevel > 4)
                        ExitRightField();
                    break;
                case 7:
                    if (Nlevel < 5)
                        ExitDownField();
                    else if (Nlevel > 4)
                        ExitRightField();
                    break;
                case 8:
                    if (Nlevel < 3)
                        ExitUpField();
                    else if (Nlevel > 2)
                        ExitRightField();
                    break;
                case 9:
                    if (Nlevel < 3)
                        ExitDownField();
                    else if (Nlevel > 2)
                        ExitRightField();
                    break;
                case 10:
                    if (Nlevel > 0)
                        ExitRightField();
                    break;
            }
        }


        /// <summary>
        ///     Выход снизу уровня
        /// </summary>
        private void ExitDownField()
        {
            for (var j = 0; j < _level.LevelData.GetLength(0) - 1; j++)
            {
                for (var i = _level.LevelData.GetLength(1) - 2; i > 1; i--)
                    if (_level.LevelData[j, i] == GenSettings.WallNumber &&
                        _level.LevelData[j + 1, i] == GenSettings.FloorNumber &&
                        _level.LevelData[j, i + 1] == GenSettings.WallNumber &&
                        _level.LevelData[j, i - 1] == GenSettings.WallNumber)
                        _exitList.Add(i, j);
                if (_exitList.Count > 0)
                    break;
            }

            if (_exitList.Count > 0)
            {
                var x = GenSettings.Rand.Next(_exitList.Count - 1);
                _level.LevelData[_exitList.ElementAt(x).Value, _exitList.ElementAt(x).Key] = GenSettings.ExitNumber;
                _level.Exit = new Point(_exitList.ElementAt(x).Key, _exitList.ElementAt(x).Value);
            }
            else
            {
                _level.Correct = false;
            }
        }

        /// <summary>
        ///     Выход справа уровня
        /// </summary>
        private void ExitRightField()
        {
            int i, j;
            for (i = _level.LevelData.GetLength(1) - 1; i > 1; i--)
            {
                for (j = _level.LevelData.GetLength(0) - 1; j > 1; j--)
                    if (_level.LevelData[j, i] == GenSettings.WallNumber &&
                        _level.LevelData[j, i - 1] == GenSettings.FloorNumber &&
                        _level.LevelData[j + 1, i] == GenSettings.WallNumber &&
                        _level.LevelData[j - 1, i] == GenSettings.WallNumber)
                        _exitList.Add(j, i);
                if (_exitList.Count > 0)
                    break;
            }

            if (_exitList.Count > 0)
            {
                int x;
                if (_nsection == 6 && Nlevel == 5 ||
                    _nsection == 8 && Nlevel == 3)
                    x = 0;
                else if (_nsection == 7 && Nlevel == 5 ||
                         _nsection == 9 && Nlevel == 3)
                    x = _exitList.Count - 1;
                else
                    x = GenSettings.Rand.Next(_exitList.Count - 1);
                _level.LevelData[_exitList.ElementAt(x).Key, _exitList.ElementAt(x).Value] = GenSettings.ExitNumber;
                _level.Exit = new Point(_exitList.ElementAt(x).Value, _exitList.ElementAt(x).Key);
            }
            else
            {
                _level.Correct = false;
            }
        }

        /// <summary>
        ///     Выход слева уровня
        /// </summary>
        private void ExitLeftField()
        {
            int i;
            for (i = 0; i < _level.LevelData.GetLength(1) - 1; i++)
            {
                int j;
                for (j = _level.LevelData.GetLength(0) - 1; j > 1; j--)
                    if (_level.LevelData[j, i] == GenSettings.WallNumber &&
                        _level.LevelData[j, i + 1] == GenSettings.FloorNumber &&
                        _level.LevelData[j + 1, i] == GenSettings.WallNumber &&
                        _level.LevelData[j + 1, i] == GenSettings.WallNumber)
                        _exitList.Add(j, i);
                if (_exitList.Count > 0)
                    break;
            }

            if (_exitList.Count > 0)
            {
                int x;
                if (_nsection == 2 && Nlevel == 3 ||
                    _nsection == 4 && Nlevel == 5)
                    x = 0;
                else if (_nsection == 3 && Nlevel == 3 ||
                         _nsection == 5 && Nlevel == 5)
                    x = _exitList.Count - 1;
                else
                    x = GenSettings.Rand.Next(_exitList.Count - 1);
                _level.LevelData[_exitList.ElementAt(x).Key, _exitList.ElementAt(x).Value] = GenSettings.ExitNumber;
                _level.Exit = new Point(_exitList.ElementAt(x).Value, _exitList.ElementAt(x).Key);
            }
            else
            {
                _level.Correct = false;
            }
        }

        /// <summary>
        ///     Выход сверху уровня
        /// </summary>
        private void ExitUpField()
        {
            int j;
            for (j = _level.LevelData.GetLength(0) - 1; j > 1; j--)
            {
                int i;
                for (i = _level.LevelData.GetLength(1) - 2; i > 1; i--)
                    if (_level.LevelData[j, i] == GenSettings.WallNumber &&
                        _level.LevelData[j - 1, i] == GenSettings.FloorNumber &&
                        _level.LevelData[j, i + 1] == GenSettings.WallNumber &&
                        _level.LevelData[j, i - 1] == GenSettings.WallNumber)
                        _exitList.Add(i, j);
                if (_exitList.Count > 0)
                    break;
            }

            if (_exitList.Count > 0)
            {
                var x = GenSettings.Rand.Next(_exitList.Count - 1);
                _level.LevelData[_exitList.ElementAt(x).Value, _exitList.ElementAt(x).Key] = GenSettings.ExitNumber;
                _level.Exit = new Point(_exitList.ElementAt(x).Key, _exitList.ElementAt(x).Value);
            }
            else
            {
                _level.Correct = false;
            }
        }


        private void ExitCenterUpField()
        {
            var top = new Dictionary<int, int>();
            int j;
            for (j = _level.LevelData.GetLength(0) - 1; j > 1; j--)
            {
                int i;
                for (i = _level.LevelData.GetLength(1) - 2; i > 1; i--)
                    if (_level.LevelData[j, i] == GenSettings.WallNumber &&
                        _level.LevelData[j - 1, i] == GenSettings.FloorNumber &&
                        _level.LevelData[j, i + 1] == GenSettings.WallNumber &&
                        _level.LevelData[j, i - 1] == GenSettings.WallNumber)
                        top.Add(i, j);
                if (top.Count > 0)
                    break;
            }

            var x = GenSettings.Rand.Next(top.Count - 1);
            _level.LevelData[top.ElementAt(x).Value, top.ElementAt(x).Key] = GenSettings.ExitNumber;
            _level.Exit = new Point(top.ElementAt(x).Key, top.ElementAt(x).Value);
        }

        //Точка выхода
        private void ExitUpDownRightField()
        {
            var right = new Dictionary<int, int>();
            Nlevel--;
            var upDown = Nlevel % 2; //0или 1
            var maxj = ( _level.MaxYPosition -  _level.MinYPosition) / 2 * (int) Math.Pow(2, upDown) +
                        _level.MinYPosition;
            int i = _level.LevelData.GetLength(1) - 1, j = maxj / 2 * upDown +  _level.MinYPosition;
            while (j < maxj)
            {
                if (_level.LevelData[j, i] == GenSettings.WallNumber &&
                    _level.LevelData[j, i - 1] == GenSettings.FloorNumber)
                    right.Add(j, i);
                j++;
                if (j == maxj)
                {
                    if (right.Count == 0)
                    {
                        j = maxj / 2 * upDown;
                        i--;
                    }
                    else if (right.Count > 0)
                    {
                        var x = GenSettings.Rand.Next(right.Count - 1);
                        _level.LevelData[right.ElementAt(x).Key, right.ElementAt(x).Value] = GenSettings.ExitNumber;
                        _level.Exit = new Point(right.ElementAt(x).Value, right.ElementAt(x).Key);
                        break;
                    }
                }
            }

            if (right.Count == 0 || i == _level.LevelData.GetLength(1))
            {
                _level.Correct = false;
            }
            else if (Nlevel == 0)
            {
                if (-_level.Enter.Y + _level.Exit.Y < 0 ||
                    -_level.Enter.Y + _level.Exit.Y > _level.LevelData.GetLength(0) / 5)
                    _level.Correct = false;
            }
            else if (Nlevel % 2 == 1)
            {
                if (_prevLevel.GlobalLevelPosition.Y + _prevLevel.Exit.Y - _level.Enter.Y + _level.Exit.Y > 0
                    || _prevLevel.GlobalLevelPosition.Y + _prevLevel.Exit.Y - _level.Enter.Y + _level.Exit.Y <
                    -_level.LevelData.GetLength(0) / 5)
                    _level.Correct = false;
            }
            else if (Nlevel % 2 == 0)
            {
                if (_prevLevel.GlobalLevelPosition.Y + _prevLevel.Exit.Y - _level.Enter.Y + _level.Exit.Y < 0
                    || _prevLevel.GlobalLevelPosition.Y + _prevLevel.Exit.Y - _level.Enter.Y + _level.Exit.Y >
                    _level.LevelData.GetLength(0) / 5)
                    _level.Correct = false;
            }
        }
    }
}