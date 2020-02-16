using System;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGenerator
{
    public class RoomInfo
    {
        public int X,Y, Width, Height; // положение и размер этого листа

        public RoomInfo LeftChild; // левый дочерний RoomInfo нашего листа
        public RoomInfo RightChild; // правый дочерний RoomInfo нашего листа
        //public Rectangle room; // комната, находящаяся внутри листа
        public Rectangle Room;
        public List<Rectangle> Halls; // коридоры, соединяющие этот лист с другими листьями

        public RoomInfo(int x, int y, int width, int height )
        {
            // инициализация листа
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Room = new Rectangle(new Point(x, y), new Point(width, height));
            Halls=new List<Rectangle>();
        }

        public bool Split()
        {
            // начинаем разрезать лист на два дочерних листа
            if (LeftChild != null || RightChild != null)
                return false; // мы уже его разрезали! прекращаем!

            // определяем направление разрезания
            // если ширина более чем на 25% больше высоты, то разрезаем вертикально
            // если высота более чем на 25% больше ширины, то разрезаем горизонтально
            // иначе выбираем направление разрезания случайным образом
            bool splitH  = Convert.ToDouble(GenSettings.Rand.Next(100)) / 100 > 0.5;
            if (Width > Height && Width / Height >= 1.25)
                splitH = false;
            else if (Height > Width && Height / Width >= 1.25)
                splitH = true;

            int max = (splitH ? Height : Width)- (int)GenSettings.MinLeafSize; // определяем максимальную высоту или ширину
            if (max <= GenSettings.MinLeafSize)
                return false; // область слишком мала, больше её делить нельзя...

            int split = GenSettings.Rand.Next((int)GenSettings.MinLeafSize, max); // определяемся, где будем разрезать

            // создаём левый и правый дочерние листы на основании направления разрезания
            if (splitH)
            {
                LeftChild = new RoomInfo(X, Y, Width, split);
                RightChild = new RoomInfo(X, Y + split, Width, Height - split);
            }
            else
            {
                LeftChild = new RoomInfo(X, Y, split, Height);
                RightChild = new RoomInfo(X + split, Y, Width - split, Height);
            }
            return true; // разрезание выполнено!
        }
    
        public void CreateRooms()
        {
            // эта функция генерирует все комнаты и коридоры для этого листа и всех его дочерних листьев.
            if (LeftChild != null || RightChild != null)
            {
                // этот лист был разрезан, поэтому переходим к его дочерним листьям
                if (LeftChild != null)
                {
                    LeftChild.CreateRooms();
                }
                if (RightChild != null)
                {
                    RightChild.CreateRooms();
                }
                // если у этого листа есть и левый, и правый дочерние листья, то создаём между ними коридор
                if (LeftChild != null && RightChild != null)
                        CreateHall(LeftChild.GetRoom(), RightChild.GetRoom());
            }
            else
            {
                // этот лист готов к созданию комнаты
                Point roomSize;
                Point roomPos;
                // размер комнаты может находиться в промежутке от 3 x 3 тайла до размера листа - 2.
                if (Width< GenSettings.MinRoomSize || Height  < GenSettings.MinRoomSize)
                {
                    Debug.Log("Работает?");
                    Room = new Rectangle(new Point(X , Y ), new Point(Width, Height));
                }
                else
                {
                    roomSize = new Point(GenSettings.Rand.Next((int)GenSettings.MinRoomSize, Width-1 ), GenSettings.Rand.Next((int)GenSettings.MinRoomSize, Height-1 ));
                    // располагаем комнату внутри листа, но не помещаем её прямо 
                    // рядом со стороной листа (иначе комнаты сольются)
                    roomPos = new Point(GenSettings.Rand.Next(1, Width - roomSize.X - 1), GenSettings.Rand.Next(1, Height - roomSize.Y - 1));
                    Room = new Rectangle(new Point(X + roomPos.X, Y + roomPos.Y), new Point(roomSize.X, roomSize.Y));
                }
            
            }
        
        }
        public Rectangle GetRoom()
        {
            // итеративно проходим весь путь по этим листьям, чтобы найти комнату, если она существует.
            if (Room != null)
                return Room;
            else
            {
                Debug.Log("Бывает?");
                Rectangle lRoom=null;
                Rectangle rRoom=null;
                if (LeftChild != null)
                {
                    lRoom = LeftChild.GetRoom();
                }
                if (RightChild != null)
                {
                    rRoom = RightChild.GetRoom();
                }
                if (lRoom == null && rRoom == null)
                    return null;
                else if (rRoom == null)
                    return lRoom;
                else if (lRoom == null)
                    return rRoom;
                else if (Convert.ToDouble(GenSettings.Rand.Next(100)) / 100 > 0.5)
                    return lRoom;
                else
                    return rRoom;
            }
        }
        public void CreateHall(Rectangle l, Rectangle rg)
        {
            Point point1 = new Point(GenSettings.Rand.Next(l.StartPosition.X + 1, l.StartPosition.X + l.Size.X - 2), GenSettings.Rand.Next(l.StartPosition.Y + 1, l.StartPosition.Y + l.Size.Y - 2));
            Point point2 = new Point(GenSettings.Rand.Next(rg.StartPosition.X + 1, rg.StartPosition.X + rg.Size.X - 2), GenSettings.Rand.Next(rg.StartPosition.Y + 1, rg.StartPosition.Y + rg.Size.Y - 2));

            int w = point2.X - point1.X;
            int h = point2.Y - point1.Y;
            int hallWidth = GenSettings.Rand.Next((int)GenSettings.MinHall, (int)GenSettings.MaxHall);
            if (w < 0)
            {
                if (h < 0)
                {
                    if (Convert.ToDouble(GenSettings.Rand.Next(100)) / 100 < 0.5)
                    {
                        Halls.Add(new Rectangle(new Point(point2.X, point1.Y), new Point(Math.Abs(w), hallWidth)));
                        Halls.Add(new Rectangle(new Point(point2.X, point2.Y), new Point(hallWidth, Math.Abs(h))));
                    }
                    else
                    {
                        Halls.Add(new Rectangle(new Point(point2.X, point2.Y), new Point(Math.Abs(w), hallWidth)));
                        Halls.Add(new Rectangle(new Point(point1.X, point2.Y), new Point(hallWidth, Math.Abs(h))));
                    }
                }
                else if (h > 0)
                {
                    if (Convert.ToDouble(GenSettings.Rand.Next(100)) / 100 < 0.5)
                    {
                        Halls.Add(new Rectangle(new Point(point2.X, point1.Y), new Point(Math.Abs(w), hallWidth)));
                        Halls.Add(new Rectangle(new Point(point2.X, point1.Y), new Point(hallWidth, Math.Abs(h))));
                    }
                    else
                    {
                        Halls.Add(new Rectangle(new Point(point2.X, point2.Y), new Point(Math.Abs(w), hallWidth)));
                        Halls.Add(new Rectangle(new Point(point1.X, point1.Y), new Point(hallWidth, Math.Abs(h))));
                    }
                }
                else // если (h == 0)
                {
                    Halls.Add(new Rectangle(new Point(point2.X, point2.Y), new Point(Math.Abs(w), hallWidth)));
                }
            }
            else if (w > 0)
            {
                if (h < 0)
                {
                    if (Convert.ToDouble(GenSettings.Rand.Next(100)) / 100 < 0.5)
                    {
                        Halls.Add(new Rectangle(new Point(point1.X, point2.Y), new Point(Math.Abs(w), hallWidth)));
                        Halls.Add(new Rectangle(new Point(point1.X, point2.Y), new Point(hallWidth, Math.Abs(h))));
                    }
                    else
                    {
                        Halls.Add(new Rectangle(new Point(point1.X, point1.Y), new Point(Math.Abs(w), hallWidth)));
                        Halls.Add(new Rectangle(new Point(point2.X, point2.Y), new Point(hallWidth, Math.Abs(h))));
                    }
                }
                else if (h > 0)
                {
                    if (Convert.ToDouble(GenSettings.Rand.Next(100)) / 100 < 0.5)
                    {
                        Halls.Add(new Rectangle(new Point(point1.X, point1.Y), new Point(Math.Abs(w), hallWidth)));
                        Halls.Add(new Rectangle(new Point(point2.X, point1.Y), new Point(hallWidth, Math.Abs(h))));
                    }
                    else
                    {
                        Halls.Add(new Rectangle(new Point(point1.X, point2.Y), new Point(Math.Abs(w), hallWidth)));
                        Halls.Add(new Rectangle(new Point(point1.X, point1.Y), new Point(hallWidth, Math.Abs(h))));
                    }
                }
                else // если (h == 0)
                {
                    Halls.Add(new Rectangle(new Point(point1.X, point1.Y), new Point(Math.Abs(w), hallWidth)));
                }
            }
            else // если (w == 0)
            {
                if (h < 0)
                {
                    Halls.Add(new Rectangle(new Point(point2.X, point2.Y), new Point(hallWidth, Math.Abs(h))));
                }
                else if (h > 0)
                {
                    Halls.Add(new Rectangle(new Point(point1.X, point1.Y), new Point(hallWidth, Math.Abs(h))));
                }
            }
        }

        public void CreateBossRoom(int section)
        {
            if (section == 1)
            {
                LeftChild = new RoomInfo(11, 4, 7, 3);
                RightChild = new RoomInfo(11, 8, 7, 3);
                Halls.Add(new Rectangle(new Point(14,1), new Point(1,10)));
            }
            if (section == 2 || section == 3|| section == 4|| section == 5)
            {
                LeftChild = new RoomInfo(19, 11, 3, 7);
                RightChild = new RoomInfo(23, 11, 3, 7);
                Halls.Add(new Rectangle(new Point(19, 14), new Point(10, 1)));
            }
            if (section == 6 || section == 7 || section == 8 || section == 9 || section == 10)
            {
                LeftChild = new RoomInfo(4, 12, 3, 7);
                RightChild = new RoomInfo(8, 12, 3, 7);
                Halls.Add(new Rectangle(new Point(1, 15), new Point(10, 1)));
            }
        }
    }
}
