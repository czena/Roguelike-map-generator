using System.Collections.Generic;
using UnityEngine;

namespace MazeGenerator
{
    public class SecretRoomInfo
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Point GlobalPosition { get; set; }
        public LevelInfo MainRoom { get; set; }
        public LevelInfo SecondRoom { get; set; }
        public LevelInfo ThirdRoom { get; set; }
        public LevelInfo FourthRoom { get; set; }
        public int?[,] RoomData { get; set; }
        public List<MazePosition> Rooms { get; set; }
        public GameObject SecretRoomObject { get; set; }
        public int SecretRoomFloors { get; set; }

        public SecretRoomInfo()
        {
            Rooms=new List<MazePosition>();
        }

        public List<MazePosition> ArrayToList()
        {
            int rMax = RoomData.GetUpperBound(1);
            int cMax = RoomData.GetUpperBound(0);
            for (int i = 0; i <= rMax; i++)
            {
                for (int j = 0; j <= cMax; j++)
                {
                    if (RoomData[j, i] == GenSettings.FloorNumber)
                        InstantiateObject(MainRoom.FloorObject, GlobalPosition.X + i, GlobalPosition.Y + j);
                    if (RoomData[j, i] == GenSettings.WallNumber)
                        InstantiateObject(MainRoom.WallObject, GlobalPosition.X + i, GlobalPosition.Y + j);
                    if (RoomData[j, i] == GenSettings.SecretRoomEnterNumber)
                        InstantiateObject(SecretRoomObject, GlobalPosition.X + i, GlobalPosition.Y + j);
                }
            }

            return Rooms;
        }
        public void InstantiateObject(GameObject o, int x, int y)
        {
            MazePosition mazePosition = new MazePosition(new Point(x, y), 0, 0, o);
            Rooms.Add(mazePosition);
        }


    }
}
