using System.Collections.Generic;
using System.Linq;

namespace MazeGenerator
{
    public class SecretRoomConstructor
    {
        private readonly List<SectionInfo> _sections;

        public SecretRoomConstructor(List<SectionInfo> sections)
        {
            _sections = sections;
        }

        public void GenerateSecretRooms(int section, int level)
        {
            SecretRoomInfo room = new SecretRoomInfo();
            room.MainRoom = _sections.ElementAt(section).Levels.ElementAt(level);

            if(level>0)//всегда предыдущая комната
                room.SecondRoom = _sections.ElementAt(section).Levels.ElementAt(level-1);
            else if (level == 0 && section > 0)
                room.SecondRoom = _sections.ElementAt(0).Levels.ElementAt(section - 1);
            else
                room.SecondRoom = null;

            //всегда следущая комната
            room.ThirdRoom = _sections.ElementAt(section).Levels.ElementAt(level+1);

            if (section == 0)
                room.FourthRoom = _sections.ElementAt(level + 1).Levels.ElementAt(0);
            else
                room.FourthRoom = null;

            room.GlobalPosition = new Point(room.MainRoom.GlobalLevelPosition.X - GenSettings.Grow,
                room.MainRoom.GlobalLevelPosition.Y - GenSettings.Grow);
            room.Width = (int) GenSettings.Width + GenSettings.Grow * 2;
            room.Height = (int) GenSettings.Height + GenSettings.Grow * 2;
            room.RoomData = new int?[room.Height, room.Width];
            room.SecretRoomObject = room.MainRoom.SecretRoomEnterObject;
            for (int i = 1; i < room.RoomData.GetLength(1)-1; i++)
            {
                for (int j = 1; j < room.RoomData.GetLength(0)-1; j++)
                {
                    room.RoomData[j, i] = GenSettings.SecretRoomEnterNumber;
                }
            }

            for (int i = 0; i < room.MainRoom.LevelData.GetLength(1); i++)
            {
                for (int j = 0; j < room.MainRoom.LevelData.GetLength(0); j++)
                {
                    if (room.MainRoom.LevelData != null)
                    {
                        DeleteEnter(room.MainRoom, room.MainRoom.LevelData, room, new Point(i,j), 
                                new Point(i + room.MainRoom.GlobalLevelPosition.X - room.GlobalPosition.X,
                                    j + room.MainRoom.GlobalLevelPosition.Y - room.GlobalPosition.Y));
                    }
                    if (room.SecondRoom != null && room.SecondRoom.LevelData!=null)
                    {
                        DeleteEnter(room.SecondRoom, room.MainRoom.LevelData, room, new Point(i, j),
                            new Point(i + room.SecondRoom.GlobalLevelPosition.X - room.GlobalPosition.X,
                                j + room.SecondRoom.GlobalLevelPosition.Y - room.GlobalPosition.Y));
                    }
                    if (room.ThirdRoom.LevelData != null)
                    {
                        DeleteEnter(room.ThirdRoom, room.MainRoom.LevelData, room, new Point(i, j),
                            new Point(i + room.ThirdRoom.GlobalLevelPosition.X - room.GlobalPosition.X,
                                j + room.ThirdRoom.GlobalLevelPosition.Y - room.GlobalPosition.Y));
                    }
                    if (room.FourthRoom != null && room.FourthRoom.LevelData != null)
                    {
                        DeleteEnter(room.FourthRoom, room.MainRoom.LevelData, room, new Point(i, j),
                            new Point(i + room.FourthRoom.GlobalLevelPosition.X - room.GlobalPosition.X,
                                j + room.FourthRoom.GlobalLevelPosition.Y - room.GlobalPosition.Y));
                    }
                }
            }
            for (int i = 0; i < room.RoomData.GetLength(1); i++)
            {
                for (int j = 0; j < room.RoomData.GetLength(0); j++)
                {
                    if (room.SecondRoom != null && (room.SecondRoom.SecretRoom != null && room.SecondRoom.SecretRoom.RoomData != null))
                    {
                        DeleteEnter(null, room.RoomData, room, new Point(i, j),
                            new Point(i + room.SecondRoom.SecretRoom.GlobalPosition.X - room.GlobalPosition.X,
                                j + room.SecondRoom.SecretRoom.GlobalPosition.Y - room.GlobalPosition.Y));
                    }
                    if (room.ThirdRoom != null && (room.ThirdRoom.SecretRoom != null && room.ThirdRoom.SecretRoom.RoomData != null))
                    {
                        DeleteEnter(null, room.RoomData, room, new Point(i, j),
                            new Point(i + room.ThirdRoom.SecretRoom.GlobalPosition.X - room.GlobalPosition.X,
                                j + room.ThirdRoom.SecretRoom.GlobalPosition.Y - room.GlobalPosition.Y));
                    }
                    if (room.FourthRoom != null && (room.FourthRoom.SecretRoom != null && room.FourthRoom.SecretRoom.RoomData != null))
                    {
                        DeleteEnter(null, room.RoomData, room, new Point(i, j),
                            new Point(i + room.FourthRoom.SecretRoom.GlobalPosition.X - room.GlobalPosition.X,
                                j + room.FourthRoom.SecretRoom.GlobalPosition.Y - room.GlobalPosition.Y));
                    }
                }
            }
            CreateRoom(room);
            room.MainRoom.SecretRoom = room;
            room.MainRoom.ListOfSecretRoomObjects = room.ArrayToList();
        }

        private void DeleteEnter(LevelInfo level, int?[,] levelData, SecretRoomInfo room, Point local, Point global)
        {
            if (global.Y < room.RoomData.GetLength(1)
                && global.X < room.RoomData.GetLength(0)
                && global.Y >= 0
                && global.X >= 0
                && levelData[local.Y, local.X] != null)
            {
                room.RoomData[global.Y, global.X] = null;
                if (level != room.MainRoom)
                {
                    if (global.Y < room.RoomData.GetLength(0) - 1)
                        room.RoomData[global.Y+1, global.X] = null;
                    if (global.Y > 0)
                        room.RoomData[global.Y-1,global.X] = null;
                    if (global.X < room.RoomData.GetLength(1) - 1)
                        room.RoomData[global.Y,global.X+1] = null;
                    if (global.X > 0)
                        room.RoomData[global.Y,global.X-1] = null;
                    if (global.Y > 0 && global.X > 0)
                        room.RoomData[global.Y-1,global.X-1] = null;
                    if (global.Y < room.RoomData.GetLength(0) - 1 && global.X > 0)
                        room.RoomData[global.Y+1,global.X-1] = null;
                    if (global.Y > 0 && global.X < room.RoomData.GetLength(1) - 1)
                        room.RoomData[global.Y-1,global.X+1] = null;
                    if (global.Y < room.RoomData.GetLength(0) - 1 && global.X < room.RoomData.GetLength(1) - 1)
                        room.RoomData[global.Y+1,global.X+1] = null;
                }
            }
        }
    

        private void CreateRoom(SecretRoomInfo room)
        {
            room.RoomData = ClearSmallRooms(room.RoomData);
            //int?[,] Data=new int?[room.RoomData.GetLength(1), room.RoomData.GetLength(0)];
            while (true)
            {
                int i = GenSettings.Rand.Next(room.MainRoom.LevelData.GetLength(1)-1);
                int j = GenSettings.Rand.Next(room.MainRoom.LevelData.GetLength(0) - 1);
                if (room.MainRoom.LevelData[j, i] == GenSettings.WallNumber &&
                    j > 1 && i > 1 && j < room.RoomData.GetLength(0) - 2 && i < room.RoomData.GetLength(1) - 2 &&
                    ((room.MainRoom.LevelData[j, i - 1] == GenSettings.FloorNumber &&
                      room.RoomData[j+GenSettings.Grow, i + 1 + GenSettings.Grow] == GenSettings.SecretRoomEnterNumber) ||
                     (room.MainRoom.LevelData[j, i + 1] == GenSettings.FloorNumber &&
                      room.RoomData[j + GenSettings.Grow, i - 1 + GenSettings.Grow] == GenSettings.SecretRoomEnterNumber) ||
                     (room.MainRoom.LevelData[j + 1, i] == GenSettings.FloorNumber &&
                      room.RoomData[j - 1 + GenSettings.Grow, i + GenSettings.Grow] == GenSettings.SecretRoomEnterNumber) ||
                     (room.MainRoom.LevelData[j - 1, i] == GenSettings.FloorNumber &&
                      room.RoomData[j + 1 + GenSettings.Grow, i + GenSettings.Grow] == GenSettings.SecretRoomEnterNumber)))
                {
                    //Debug.Log(i + " " + j);
                    room.MainRoom.SecretRoomEnter(i,j);
                    int n=0;
                    List<Point> floorRoom=new List<Point>();
                    FillRoom(i + GenSettings.Grow, j + GenSettings.Grow, room.RoomData, floorRoom, ref n);
                    ClearGarbage(room.RoomData);
                    WallCreator(room.RoomData,room.MainRoom.LevelData);
                    //room.MainRoom.LevelData[j, i] = GenSettings.SecretRoomEnterNumber;
                    break;
                }
            }
        }

        private int?[,] ClearSmallRooms(int?[,] data)
        {
            int?[,] complete= new int?[data.GetLength(0), data.GetLength(1)];
            int?[,] buffer=new int?[data.GetLength(0), data.GetLength(1)];
            for (int i = 0; i < data.GetLength(1); i++)
            {
                for (int j = 0; j < data.GetLength(0); j++)
                {
                    if (data[j, i] != null)
                    {
                        int n = 0;
                        Clear(data, buffer, j, i, ref n);
                        if (n >= GenSettings.SecretRoomFloorCount)
                        {
                            for (int x = 0; x < data.GetLength(1); x++)
                            {
                                for (int y = 0; y < data.GetLength(0); y++)
                                {
                                    if (buffer[y, x] != null)
                                    {
                                        complete[y, x] = buffer[y, x];
                                        buffer[y, x] = null;
                                    }
                                }
                            }
                        }
                        else
                        {
                            buffer = new int?[data.GetLength(0), data.GetLength(1)];
                        }
                    }
                }
            }
            return complete;

        }

        private void Clear(int?[,]data, int?[,]buffer, int y, int x, ref int n)
        {
            buffer[y, x] = data[y, x];
            data[y, x] = null;
            n++;
            if (y - 1 > 0 && data[y - 1, x] !=null)
                Clear(data, buffer, y - 1, x, ref n);
            if (x + 1 < data.GetLength(1) && data[y, x + 1] !=null)
                Clear(data, buffer, y , x+1, ref n);
            if (x - 1 > 0 && data[y, x - 1] != null)
                Clear(data, buffer, y, x - 1, ref n);
            if (y + 1 < data.GetLength(0) && data[y + 1, x] !=null)
                Clear(data, buffer, y+1, x , ref n);
        }

        private void FillRoom(int x, int y, int?[,]data, List<Point> roomFloor, ref int n)
        {
            while (n < GenSettings.SecretRoomFloorCount)
            {
                if (x > 1 && data[y, x - 1] == GenSettings.SecretRoomEnterNumber)
                {
                    n++;
                    roomFloor.Add(new Point(x - 1, y));
                    data[y, x - 1] = GenSettings.FloorNumber;
                }
                if (y > 1 && data[y-1, x ] == GenSettings.SecretRoomEnterNumber)
                {
                    n++;
                    roomFloor.Add(new Point(x , y-1));
                    data[y-1, x ] = GenSettings.FloorNumber;
                }
                if (y <data.GetLength(0)-2 && data[y + 1, x] == GenSettings.SecretRoomEnterNumber)
                {
                    n++;
                    roomFloor.Add(new Point(x, y + 1));
                    data[y + 1, x] = GenSettings.FloorNumber;
                }
                if (x < data.GetLength(1) - 2 && data[y, x + 1] == GenSettings.SecretRoomEnterNumber)
                {
                    n++;
                    roomFloor.Add(new Point(x + 1, y));
                    data[y, x + 1] = GenSettings.FloorNumber;
                }

                Point m;
                //Debug.Log("Count "+ roomFloor.Count+ " "+ n);
                if (roomFloor.Count == 0)
                    return;
                if (roomFloor.Count>0)
                     m= roomFloor.ElementAt(GenSettings.Rand.Next(roomFloor.Count-1));
                else
                    m = roomFloor.ElementAt(0);
                roomFloor.Remove(m);
                FillRoom(m.X, m.Y, data,roomFloor,ref n);
            }
        }

        private void ClearGarbage(int?[,] data)
        {
            for (int i = 0; i < data.GetLength(1); i++)
            {
                for (int j = 0; j < data.GetLength(0); j++)
                {
                    if (data[j, i] == GenSettings.SecretRoomEnterNumber)
                        data[j, i] = null;
                }
            }
        }

        private void WallCreator(int?[,] roomData, int?[,] mainLevel )
        {
            for (int j = 1; j < roomData.GetLength(0) - 1; j++)
            {
                for (int i = 1; i < roomData.GetLength(1) - 1; i++)
                {
                    if (roomData[j, i] == 0)
                    {
                        if (roomData[j + 1, i + 1] == null && CheckWall(i + 1, j + 1, mainLevel))
                            roomData[j + 1, i + 1] = GenSettings.WallNumber;
                        if (roomData[j + 1, i] == null && CheckWall(i, j + 1, mainLevel))
                            roomData[j + 1, i] = GenSettings.WallNumber;
                        if (roomData[j + 1, i - 1] == null && CheckWall(i - 1, j + 1, mainLevel))
                            roomData[j + 1, i - 1] = GenSettings.WallNumber;
                        if (roomData[j, i - 1] == null && CheckWall(i - 1, j, mainLevel))
                            roomData[j, i - 1] = GenSettings.WallNumber;
                        if (roomData[j - 1, i - 1] == null && CheckWall(i - 1, j - 1, mainLevel))
                            roomData[j - 1, i - 1] = GenSettings.WallNumber;
                        if (roomData[j - 1, i] == null && CheckWall(i , j - 1, mainLevel))
                            roomData[j - 1, i] = GenSettings.WallNumber;
                        if (roomData[j - 1, i + 1] == null && CheckWall(i + 1, j - 1, mainLevel))
                            roomData[j - 1, i + 1] = GenSettings.WallNumber;
                        if (roomData[j, i + 1] == null && CheckWall(i + 1, j , mainLevel))
                            roomData[j, i + 1] = GenSettings.WallNumber;
                    }
                }
            }
        }

        private bool CheckWall(int x, int y, int?[,] data)
        {
            bool check = !(x-GenSettings.Grow>=0 && y - GenSettings.Grow >= 0 && x - GenSettings.Grow < data.GetLength(1) && y - GenSettings.Grow < data.GetLength(0)&&
                           data[y - GenSettings.Grow,x - GenSettings.Grow]== GenSettings.WallNumber);
            return check;
        }

    }
}
