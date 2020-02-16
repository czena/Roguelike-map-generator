using System.Collections.Generic;
using System.Linq;
using MazeGenerator.Methods;

namespace MazeGenerator
{
    public class LevelConstructor
    {
        private readonly List<LevelInfo> _section;
        private readonly SectionInfo _sectionInfo;
        private RoomConstrcutor _dataGenerator; //первоначальная генерация
        private List<RoomInfo> _leafs; //набор комнат и коридоров после начальной генерации
        private Point _lvlSwap;
        private LevelInfo _prevLevel;

        public LevelConstructor(SectionInfo sectionInfo)
        {
            _sectionInfo = sectionInfo;
            _section = sectionInfo.Levels;
        }

        /// <summary>
        ///     Cвойство data. Объявления доступа (например, объявление
        ///     свойства как public, но затем назначение его private set) делает
        ///     его read-only за пределами класса. Таким образом, данные лабиринта невозможно будет изменять извне.
        /// </summary>
        public int?[,] Data { get; private set; }

        public LevelInfo GenerateNewLevel(LevelInfo prevLevel)
        {
            Data = new int?[GenSettings.Height, GenSettings.Width];
            _dataGenerator = new RoomConstrcutor(GenSettings.Width, GenSettings.Height);
            _prevLevel = prevLevel; //Предыдущий уровень
            _lvlSwap = LevelSwap(); //Сдвиг уровня
            var endCycle = 0;

            while (endCycle < GenSettings.MaxGenerations)
            {
                if (_section.Count  != 9)
                    _leafs = _dataGenerator.Creator();
                else
                    _leafs = _dataGenerator.CreatorBossRoom(_sectionInfo.SectionN);
                ArrayFill();
                LevelInfo newLevel = ClearSmallRooms();
                newLevel.Level = _section.Count + 1;
                if (newLevel.Level == 10 || (newLevel.NFloors > GenSettings.MinFloors && newLevel.NFloors < GenSettings.MaxFloors))
                {

                        newLevel.WallCreator();
                        //Debug.Log(newLevel.MaxXPosition + " " + newLevel.MinXPosition);
                    if (newLevel.Correct)
                    {
                        int center;
                        if (_sectionInfo.SectionN == 2 && newLevel.Level > 3 ||
                            _sectionInfo.SectionN == 3 && newLevel.Level > 3 ||
                            _sectionInfo.SectionN == 4 && newLevel.Level > 5 ||
                            _sectionInfo.SectionN == 5 && newLevel.Level > 5 ||
                            _sectionInfo.SectionN == 6 && newLevel.Level > 5 ||
                            _sectionInfo.SectionN == 7 && newLevel.Level > 5 ||
                            _sectionInfo.SectionN == 8 && newLevel.Level > 3 ||
                            _sectionInfo.SectionN == 9 && newLevel.Level > 3 ||
                            (_sectionInfo.SectionN == 10 && newLevel.Level > 0 && newLevel.Level!=10))
                            center = _sectionInfo.GlobalCenterY;
                        else
                            center = _sectionInfo.GlobalCenterX;
                        var enter = new EnterLevel(newLevel.Level, _prevLevel, newLevel, _sectionInfo.SectionN, center);
                        enter.GenerateEnter();
                    }
                    if (newLevel.Correct && CollisionCheck(newLevel) == false) newLevel.Correct = false;

                    if (newLevel.Correct && newLevel.Level != 10)
                    {
                        var exit = new ExitLevel(newLevel.Level, prevLevel != null ? _prevLevel : null,
                            newLevel, _sectionInfo.SectionN);
                        exit.GenerateExit();
                    }

                    if (newLevel.Correct && newLevel.Level != 10)
                        newLevel.Correct = newLevel.Path.CorrectPath(newLevel);
                    if (_sectionInfo.SectionN == 1 && newLevel.Level != 10)
                        if (newLevel.Correct)
                        {
                            var sectionExit = new SectionExit(newLevel.Level,
                                prevLevel != null ? _prevLevel : null, newLevel);
                            sectionExit.GenerateSectionExit();
                        }
                    if (!newLevel.Correct)
                    {
                        endCycle++;
                        continue;
                    }

                    if (newLevel.Level == 1)
                    {
                        if (_sectionInfo.SectionN == 1)
                            newLevel.GlobalLevelPosition = new Point(_lvlSwap.X, -newLevel.Enter.Y - _lvlSwap.Y);
                        else
                            newLevel.GlobalLevelPosition = new Point(_prevLevel.GlobalLevelPosition.X
                                                                     + _prevLevel.SectionExit.X
                                                                     + _lvlSwap.X - newLevel.Enter.X,
                                _prevLevel.GlobalLevelPosition.Y
                                + _prevLevel.SectionExit.Y + _lvlSwap.Y
                                - newLevel.Enter.Y);
                    }
                    else
                    {
                        newLevel.GlobalLevelPosition = new Point(
                            _prevLevel.GlobalLevelPosition.X + _prevLevel.Exit.X + _lvlSwap.X - newLevel.Enter.X,
                            _prevLevel.GlobalLevelPosition.Y + _prevLevel.Exit.Y - newLevel.Enter.Y + _lvlSwap.Y);
                    }
                    if (newLevel.Correct) return newLevel;
                }

                endCycle++;
            }
            return null;
        }

        public void SetSectionsObjects(LevelInfo level)
        {
            level.FloorObject = _sectionInfo.GameManager.ResManager.GetFloor(_sectionInfo.SectionN);
            level.WallObject= _sectionInfo.GameManager.ResManager.GetWall(_sectionInfo.SectionN);
            level.EnterObject= _sectionInfo.GameManager.ResManager.GetStartPoint();
            level.ExitObject= _sectionInfo.GameManager.ResManager.GetEndPoint();
            level.PlayerSpawner = _sectionInfo.GameManager.ResManager.GetPlayerSpawner();
            level.SectionEndObject= _sectionInfo.GameManager.ResManager.GetSectionPointsEnd();
            level.SecretRoomEnterObject= _sectionInfo.GameManager.ResManager.GetSecretRoom();
            level.SectionStartObject= _sectionInfo.GameManager.ResManager.GetSectionPointsStart();

            level.Enemies.EnemiesPrefab = _sectionInfo.Enemies;

            //level.FloorObject = _sectionInfo.Floor;
            //level.WallObject = _sectionInfo.Wall;
            //level.EnterObject = _sectionInfo.StartPoint;
            //level.ExitObject = _sectionInfo.EndPoint;
            //level.PlayerSpawner = _sectionInfo.PlayerSpawner;
            //level.SectionEndObject = _sectionInfo.SectionPoint;
            //level.SecretRoomEnterObject = _sectionInfo.SecretRoomPoint;
            //level.SectionStartObject = _sectionInfo.SectionStartPoint;
        }

        private void ArrayFill()
        {
            foreach (var leaf in _leafs)
            {
                if (leaf.LeftChild == null && leaf.RightChild == null)
                    for (var j = leaf.Room.StartPosition.Y; j < leaf.Room.Size.Y + leaf.Room.StartPosition.Y; j++)
                    for (var i = leaf.Room.StartPosition.X; i < leaf.Room.Size.X + leaf.Room.StartPosition.X; i++)
                    {
                        if (j == leaf.Room.StartPosition.Y ||
                            i == leaf.Room.StartPosition.X ||
                            j == leaf.Room.StartPosition.Y + leaf.Room.Size.Y - 1 ||
                            i == leaf.Room.StartPosition.X + leaf.Room.Size.X - 1)
                            Data[j, i] = null;

                        if (j == 0 || i == 0 || j == Data.GetLength(0) - 1 || i == Data.GetLength(1) - 1)
                            Data[j, i] = null;
                        else
                            Data[j, i] = GenSettings.FloorNumber;
                    }

                if (leaf.Halls != null)
                    foreach (var hall in leaf.Halls)
                        for (var j = hall.StartPosition.Y;
                            j < hall.Size.Y + hall.StartPosition.Y && j < Data.GetLength(0) - 1 && j > 0;
                            j++)
                        for (var i = hall.StartPosition.X;
                            i < hall.Size.X + hall.StartPosition.X && i < Data.GetLength(1) - 1 && i > 0;
                            i++)
                            Data[j, i] = GenSettings.FloorNumber;

            }

            //for (int i = 0; i < Data.GetLength(1); i++)
            //{
            //    for (int j = 0; j < Data.GetLength(0); j++)
            //    {
            //        Debug.Log(Data[j, i]);
            //    }
            //}
        }


        private LevelInfo ClearSmallRooms() //убираем маленькие комнаты
        {
            var rooms = new List<LevelInfo>();
            while (true)
            {
                int i = 0, j;
                for (j = 0; j < Data.GetLength(0); j++)
                {
                    for (i = 0; i < Data.GetLength(1); i++)
                        if (Data[j, i] == GenSettings.FloorNumber)
                            break;

                    if (i < Data.GetLength(1))
                        if (Data[j, i] == GenSettings.FloorNumber)
                            break;
                }

                if (j != Data.GetLength(0) && i != Data.GetLength(1))
                {
                    var level = new LevelInfo(0, Data);
                    RoomParse(level, j, i);
                    rooms.Add(level);
                }
                else
                {
                    break;
                }
            }

            var max = rooms.Max(x => x.NFloors);
            return rooms.FirstOrDefault(x => x.NFloors == max);
        }

        private void RoomParse(LevelInfo levelInfo, int j, int i)
        {
            if (Data[j, i] == 0)
            {
                levelInfo.NFloors++;
                Data[j, i] = null;
                levelInfo.LevelData[j, i] = GenSettings.FloorNumber;
                if (j - 1 > 0 && Data[j - 1, i] == GenSettings.FloorNumber)
                    RoomParse(levelInfo, j - 1, i);
                if (i + 1 < Data.GetLength(1) && Data[j, i + 1] == GenSettings.FloorNumber)
                    RoomParse(levelInfo, j, i + 1);
                if (i - 1 > 0 && Data[j, i - 1] == GenSettings.FloorNumber)
                    RoomParse(levelInfo, j, i - 1);
                if (j + 1 < Data.GetLength(0) && Data[j + 1, i] == GenSettings.FloorNumber)
                    RoomParse(levelInfo, j + 1, i);
            }
        }

        private Point LevelSwap()
        {
            if (_section.Count != 0 || _sectionInfo.SectionN != 1)
            {
                Point exitPrev;
                if (_section.Count == 0 && _sectionInfo.SectionN != 1)
                    exitPrev = _prevLevel.SectionExit;
                else
                    exitPrev = _prevLevel.Exit;

                if (exitPrev.X - 1 > -1 && _prevLevel.LevelData[exitPrev.Y, exitPrev.X - 1] == GenSettings.FloorNumber)
                    return new Point(1, 0);
                if (exitPrev.Y + 1 < _prevLevel.LevelData.GetLength(0) &&
                    _prevLevel.LevelData[exitPrev.Y + 1, exitPrev.X] == GenSettings.FloorNumber)
                    return new Point(0, -1);
                if (_prevLevel.LevelData[exitPrev.Y, exitPrev.X + 1] == GenSettings.FloorNumber)
                    return new Point(-1, 0);
                if (_prevLevel.LevelData[exitPrev.Y - 1, exitPrev.X] == GenSettings.FloorNumber)
                    return new Point(0, 1);
            }

            return new Point(0, 0);
        }

        private bool CollisionCheck(LevelInfo level)
        {
            if (_prevLevel == null)
                return true;
            Point exit = level.Level == 1 ? new Point(_prevLevel.SectionExit.X, _prevLevel.SectionExit.Y) : new Point(_prevLevel.Exit.X, _prevLevel.Exit.Y);
            var startX = exit.X + _lvlSwap.X - level.Enter.X;
            var startY = exit.Y + _lvlSwap.Y - level.Enter.Y;
            for (var i = 0; i < _prevLevel.LevelData.GetLongLength(1); i++)
            for (var j = 0; j < _prevLevel.LevelData.GetLongLength(0); j++)
                if (_prevLevel.LevelData[j, i] != null)
                {
                    var jCurrent = j - startY;
                    var iCurrent = i - startX;
                    if (jCurrent >= 0 && jCurrent < level.LevelData.GetLongLength(0) &&
                        iCurrent >= 0 && iCurrent < level.LevelData.GetLongLength(1))
                        if (level.LevelData[jCurrent, iCurrent] != null)
                            return false;
                }

            return true;
        }
    }
}