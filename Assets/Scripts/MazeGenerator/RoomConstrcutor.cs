using System;
using System.Collections.Generic;

namespace MazeGenerator
{
    public class RoomConstrcutor
    {
        public Point _size;

        public RoomConstrcutor(uint x, uint y)
        {
            _size = new Point((int) x, (int) y);
        }

        public List<RoomInfo> Creator()
        {
            //Debug.Log("public List<RoomInfo> Creator()");
            List<RoomInfo> leafs = new List<RoomInfo>();

            // сначала создаём лист, который будет "корнем" для всех остальных листьев.
            RoomInfo root = new RoomInfo(0, 0, _size.X, _size.Y);
            leafs.Add(root);

            bool didSplit = true;
            // циклически снова и снова проходим по каждому листу в нашем Vector, пока больше не останется листьев, которые можно разрезать.
            while (didSplit)
            {
                didSplit = false;
                foreach (RoomInfo l in leafs)
                {
                    if (l.LeftChild == null && l.RightChild == null) // если лист ещё не разрезан...
                    {
                        // если этот лист слишком велик, или есть вероятность 75%...
                        if (l.Width > GenSettings.MaxLeafSize || l.Height > GenSettings.MaxLeafSize ||
                            Convert.ToDouble(GenSettings.Rand.Next(100)) / 100 > 0.25)
                        {
                            if (l.Split()) // разрезаем лист!
                            {
                                // если мы выполнили разрезание, передаём дочерние листья в Vector, чтобы в дальнейшем можно было в цикле обойти и их
                                leafs.Add(l.LeftChild);
                                leafs.Add(l.RightChild);
                                didSplit = true;
                                break;
                            }
                        }
                    }
                }
            }

            // затем итеративно проходим по каждому листу и создаём в каждом комнату.
            root.CreateRooms();
            return leafs;
        }

        public List<RoomInfo> CreatorBossRoom(int section)
        {
            List<RoomInfo> leafs = new List<RoomInfo>();
            RoomInfo root = new RoomInfo(0, 0, _size.X, _size.Y);
            root.CreateBossRoom(section);
            leafs.Add(root);
            leafs.Add(root.LeftChild);
            leafs.Add(root.RightChild);
            return leafs;
        }
    }
}
