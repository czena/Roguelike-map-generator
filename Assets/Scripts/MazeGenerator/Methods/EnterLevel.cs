using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGenerator.Methods
{
    public class EnterLevel : BaseEnterExit
    {
        private Dictionary<int, int> _enterList;
        private Point _exit;


        public EnterLevel(int nlevel, LevelInfo prevLevel, LevelInfo level, int nsection, int globalcenter) : base(
            nlevel, prevLevel, level, nsection, globalcenter)
        {
        }

        public void GenerateEnter()
        {
            _exit = _prevLevel == null ? new Point(0, 0) : Nlevel == 1 ? _prevLevel.SectionExit : _prevLevel.Exit;
            _enterList = new Dictionary<int, int>();
            switch (_nsection)
            {
                case 1:
                    if(Nlevel!=10)
                        EnterFieldLeftFirstSection();
                    else
                        EnterFieldDown();
                    break;
                case 2:
                    if (Nlevel < 4)
                        EnterFieldDown();
                    else if (Nlevel > 3)
                        EnterFieldRight();
                    break;
                case 3:
                    if (Nlevel < 4)
                        EnterFieldUp();
                    else if (Nlevel > 3)
                        EnterFieldRight();
                    break;
                case 4:
                    if (Nlevel < 6)
                        EnterFieldDown();
                    else if (Nlevel > 5)
                        EnterFieldRight();
                    break;
                case 5:
                    if (Nlevel < 6)
                        EnterFieldUp();
                    else if (Nlevel > 5)
                        EnterFieldRight();
                    break;
                case 6:
                    if (Nlevel < 6)
                        EnterFieldDown();
                    else if (Nlevel > 5)
                        EnterFieldLeft();
                    break;
                case 7:
                    if (Nlevel < 6)
                        EnterFieldUp();
                    else if (Nlevel > 5)
                        EnterFieldLeft();
                    break;
                case 8:
                    if (Nlevel < 4)
                        EnterFieldDown();
                    else if (Nlevel > 3)
                        EnterFieldLeft();
                    break;
                case 9:
                    if (Nlevel < 4)
                        EnterFieldUp();
                    else if (Nlevel > 3)
                        EnterFieldLeft();
                    break;
                case 10:
                    if (Nlevel > 0)
                        EnterFieldLeft();
                    break;
            }
        }

        private void EnterFieldUp()
        {
            _level.GlobalMaxXPosition = _globalcenter + GenSettings.OtherLevelsWidth / 2;
            _level.GlobalMinXPosition = _globalcenter - GenSettings.OtherLevelsWidth / 2;
            var dmax = _exit.X + _prevLevel.GlobalLevelPosition.X - _level.GlobalMinXPosition + _level.MinYPosition;
            var dmin = _level.LevelData.GetLength(1) -
                       (_level.GlobalMaxXPosition - (_exit.X + _prevLevel.GlobalLevelPosition.X)) - (_level.LevelData.GetLength(1) - _level.MaxXPosition);
            dmax = dmax > _level.LevelData.GetLength(1) ? _level.LevelData.GetLength(1) : dmax;
            dmin = dmin < 0 ? 0 : dmin;
            for (var j = _level.LevelData.GetLength(0) - 1; j > 0; j--)
            {
                for (var i = dmin; i < dmax; i++)
                    if (_level.LevelData[j, i] == 1 && _level.LevelData[j - 1, i] == 0)
                        _enterList.Add(i, j);

                if (_enterList.Count > 0)
                    break;
            }

            if (_enterList.Count > 0)
            {
                int x;
                if (_nsection == 3 && Nlevel == 3 ||
                    _nsection == 5 && Nlevel == 5)
                    x = _enterList.Count - 1;
                else
                    x = GenSettings.Rand.Next(_enterList.Count - 1);
                _level.LevelData[_enterList.ElementAt(x).Value, _enterList.ElementAt(x).Key] = GenSettings.EnterNumber;
                _level.Enter = new Point(_enterList.ElementAt(x).Key, _enterList.ElementAt(x).Value);
            }
            else
            {
                _level.Correct = false;
            }
        }

        private void EnterFieldLeft()
        {
            _level.GlobalMaxYPosition = _globalcenter + GenSettings.OtherLevelsHeight / 2;
            _level.GlobalMinYPosition = _globalcenter - GenSettings.OtherLevelsHeight / 2;
            var dmax = _exit.Y + _prevLevel.GlobalLevelPosition.Y - _level.GlobalMinYPosition + _level.MinYPosition;
            var dmin = _level.LevelData.GetLength(0) -
                       (_level.GlobalMaxYPosition - (_exit.Y + _prevLevel.GlobalLevelPosition.Y)) - (_level.LevelData.GetLength(0) - _level.MaxYPosition);
            dmax = dmax > _level.LevelData.GetLength(0) ? _level.LevelData.GetLength(0) : dmax;
            dmin = dmin < 0 ? 0 : dmin;
            for (var i = 0; i < _level.LevelData.GetLength(0) - 1; i++)
            {
                for (var j = dmin; j < dmax; j++)
                    if (_level.LevelData[j, i] == 1 && _level.LevelData[j, i + 1] == 0)
                        _enterList.Add(j, i);
                if (_enterList.Count > 0)
                    break;
            }

            if (_enterList.Count > 0)
            {
                int x;
                x = GenSettings.Rand.Next(_enterList.Count - 1);
                _level.LevelData[_enterList.ElementAt(x).Key, _enterList.ElementAt(x).Value] = GenSettings.EnterNumber;
                _level.Enter = new Point(_enterList.ElementAt(x).Value, _enterList.ElementAt(x).Key);
            }
            else
            {
                _level.Correct = false;
            }
        }


        private void EnterFieldRight()
        {
            _level.GlobalMaxYPosition = _globalcenter + GenSettings.OtherLevelsHeight / 2;
            _level.GlobalMinYPosition = _globalcenter - GenSettings.OtherLevelsHeight / 2;
            var dmax = _exit.Y + _prevLevel.GlobalLevelPosition.Y - _level.GlobalMinYPosition + _level.MinYPosition;
            var dmin = _level.LevelData.GetLength(0) -(_level.GlobalMaxYPosition - (_exit.Y + _prevLevel.GlobalLevelPosition.Y)) - (_level.LevelData.GetLength(0) - _level.MaxYPosition);
            dmax = dmax > _level.LevelData.GetLength(0) ? _level.LevelData.GetLength(0) : dmax;
            dmin = dmin < 0 ? 0 : dmin;
            for (var i = _level.LevelData.GetLength(0) - 1; i > 1; i--)
            {
                for (var j = dmin; j < dmax; j++)
                    if (_level.LevelData[j, i] == 1 && _level.LevelData[j, i - 1] == 0)
                        _enterList.Add(j, i);
                if (_enterList.Count > 0)
                    break;
            }

            if (_enterList.Count > 0)
            {
                int x;
                x = GenSettings.Rand.Next(_enterList.Count - 1);
                _level.LevelData[_enterList.ElementAt(x).Key, _enterList.ElementAt(x).Value] = GenSettings.EnterNumber;
                _level.Enter = new Point(_enterList.ElementAt(x).Value, _enterList.ElementAt(x).Key);
            }
            else
            {
                _level.Correct = false;
            }
        }


        private void EnterFieldDown() //вход снизу
        {
            _level.GlobalMaxXPosition = _globalcenter + GenSettings.OtherLevelsWidth / 2;
            _level.GlobalMinXPosition = _globalcenter - GenSettings.OtherLevelsWidth / 2;
            var dmax = _exit.X + _prevLevel.GlobalLevelPosition.X - _level.GlobalMinXPosition+_level.MinXPosition;
            var dmin = _level.LevelData.GetLength(1) - (_level.GlobalMaxXPosition - (_exit.X + _prevLevel.GlobalLevelPosition.X)) - (_level.LevelData.GetLength(1) - _level.MaxXPosition);
            //Debug.Log(dmin+" "+dmax);
            dmax = dmax > _level.LevelData.GetLength(1) ? _level.LevelData.GetLength(1) : dmax;
            dmin = dmin < 0 ? 0 : dmin;
            for (var j = 0; j < _level.LevelData.GetLength(0) - 1; j++)
            {
                for (var i = dmin; i < dmax; i++)
                    if (_level.LevelData[j, i] == 1 && _level.LevelData[j + 1, i] == 0)
                        _enterList.Add(i, j);

                if (_enterList.Count > 0)
                    break;
            }

            if (_enterList.Count > 0)
            {
                //Debug.Log(_enterList.Count);
                int x;
                if (_nsection == 2 && Nlevel == 3 ||
                    _nsection == 4 && Nlevel == 5)
                    x = _enterList.Count - 1;
                else
                    x = GenSettings.Rand.Next(_enterList.Count - 1);
                _level.LevelData[_enterList.ElementAt(x).Value, _enterList.ElementAt(x).Key] = GenSettings.EnterNumber;
                _level.Enter = new Point(_enterList.ElementAt(x).Key, _enterList.ElementAt(x).Value);
            }
            else
            {
                _level.Correct = false;
            }
        }

        //Точка входа
        private void EnterFieldLeftFirstSection()
        {
            Nlevel--;
            var upDown = Nlevel % 2; //0или 1
            var maxj = _level.LevelData.GetLength(0) / 2 * (int) Math.Pow(2, upDown);
            int i = 0, j = maxj / 2 * upDown;

            while (j < maxj)
            {
                if (_level.LevelData[j, i] == 1 && _level.LevelData[j, i + 1] == 0)
                    _enterList.Add(j, i);
                j++;
                if (j == maxj)
                {
                    if (_enterList.Count == 0)
                    {
                        j = maxj / 2 * upDown;
                        i++;
                        if (i == _level.LevelData.GetLength(1))
                            break;
                    }
                    else if (_enterList.Count > 0)
                    {
                        var x = GenSettings.Rand.Next(_enterList.Count - 1);
                        _level.LevelData[_enterList.ElementAt(x).Key, _enterList.ElementAt(x).Value] = GenSettings.EnterNumber;
                        _level.Enter = new Point(_enterList.ElementAt(x).Value, _enterList.ElementAt(x).Key);
                        break;
                    }
                }
            }

            if (_enterList.Count == 0 || i == _level.LevelData.GetLength(1))
            {
                _level.Correct = false;
            }
            else if (Nlevel == 0)
            {
                if (_level.MaxYPosition - _level.Enter.Y > GenSettings.HalfWidthSectionOne - 1)
                    _level.Correct = false;
            }
            else if (Nlevel % 2 == 1)
            {
                if (_prevLevel.GlobalLevelPosition.Y + _prevLevel.Exit.Y - _level.Enter.Y - _level.MinYPosition <
                    -GenSettings.HalfWidthSectionOne + 1
                    || _level.MaxYPosition < 0)
                    _level.Correct = false;
            }
            else if (Nlevel % 2 == 0)
            {
                if (_prevLevel.GlobalLevelPosition.Y + _prevLevel.Exit.Y - _level.Enter.Y + _level.MaxYPosition >
                    GenSettings.HalfWidthSectionOne - 1
                    || _level.MinYPosition > 0)
                    _level.Correct = false;
            }
        }
    }
}