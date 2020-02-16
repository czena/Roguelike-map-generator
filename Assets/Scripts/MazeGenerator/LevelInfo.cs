using System.Collections.Generic;
using MazeGenerator.Methods;
using UnityEngine;

namespace MazeGenerator
{
    public class LevelInfo
    {
        public int Level { get; set; }//Номер уровня
        public int Section { get; set; }//Номер секции
        public int NFloors { get; set; }//Число флоров
        public int?[,] LevelData { get; set; }//массив уровня в цифрах
        public Point GlobalLevelPosition { get; set; }//позиция уровня в глобальных координатах
        public Point Enter { get; set; }//позиция точки входа в глобальных координатах
        public Point Exit { get; set; }//позиция выхода в глобальных координатах
        public Point SectionExit { get; set; }
        public int MaxYPosition { get; set; }//максимальня позиция уровня по оси Y
        public int MinYPosition { get; set; }//минимальная позиция уровня по оси Y
        public int MaxXPosition { get; set; }//максимальня позиция уровня по оси X
        public int MinXPosition { get; set; }//минимальная позиция уровня по оси X
        public int GlobalMaxXPosition { get; set; }
        public int GlobalMinXPosition { get; set; }
        public int GlobalMaxYPosition { get; set; }
        public int GlobalMinYPosition { get; set; }
        public bool Correct { get; set; }//Уровень корректный?
        public ShortPath Path { get; set; }//Минимальный путь от входа до выхода
        public EnemyInfo Enemies { get; set; }
        

        public GameObject FloorObject { get; set; }//префаб пола
        public GameObject WallObject { get; set; }//префаб стены
        public GameObject EnterObject { get; set; }//префаб старт поинта
        public GameObject ExitObject { get; set; }//префаб энд поинта
        public GameObject SectionEndObject { get; set; }
        public GameObject SecretRoomEnterObject { get; set; }
        public GameObject PlayerSpawner { get; set; }//префаб старта игрока
        public GameObject SectionStartObject { get; set; }

        public List<MazePosition> ListOfGameObjects { get; set; }//все префабы уровня с координатами

        public bool IsSecretRoomActive { get; set; }
        public SecretRoomInfo SecretRoom { get; set; }
        public List<MazePosition> ListOfSecretRoomObjects { get; set; }

        public LevelInfo(int n, int?[,]data)
        {
            NFloors = n;
            LevelData = new int?[data.GetLength(1), data.GetLength(0)];
            Correct = true;
            Path = new ShortPath();
            ListOfGameObjects=new List<MazePosition>();
            //ListOfSecretRoomObjects=new List<MazePosition>();
            Enemies =new EnemyInfo();
            MaxYPosition = 0;
            MinYPosition = LevelData.GetLength(0);
            MaxXPosition = 0;
            MinXPosition = LevelData.GetLength(1);
            IsSecretRoomActive = false;
        }
        /// <summary>
        /// Создает стены
        /// </summary>
        public void WallCreator()
        {
            for (int j = 1; j < LevelData.GetLength(0) - 1; j++)
            {
                for (int i = 1; i < LevelData.GetLength(1) - 1; i++)
                {
                    if (LevelData[j, i] == 0)
                    {
                        if (LevelData[j + 1, i + 1] == null)
                            LevelData[j + 1, i + 1] = GenSettings.WallNumber;
                        if (LevelData[j + 1, i] == null)
                            LevelData[j + 1, i] = GenSettings.WallNumber;
                        if (LevelData[j + 1, i - 1] == null)
                            LevelData[j + 1, i - 1] = GenSettings.WallNumber;
                        if (LevelData[j, i - 1] == null)
                            LevelData[j, i - 1] = GenSettings.WallNumber;
                        if (LevelData[j - 1, i - 1] == null)
                            LevelData[j - 1, i - 1] = GenSettings.WallNumber;
                        if (LevelData[j - 1, i] == null)
                            LevelData[j - 1, i] = GenSettings.WallNumber;
                        if (LevelData[j - 1, i + 1] == null)
                            LevelData[j - 1, i + 1] = GenSettings.WallNumber;
                        if (LevelData[j, i + 1] == null)
                            LevelData[j, i + 1] = GenSettings.WallNumber;
                        if (MaxYPosition < j + 1)
                            MaxYPosition = j + 1;
                        if (MinYPosition > j - 1)
                            MinYPosition = j - 1;
                        if (MaxXPosition < i + 1)
                            MaxXPosition = i+1;
                        if (MinXPosition > i - 1)
                            MinXPosition = i - 1;
                    }
                }
            }
        }
        /// <summary>
        /// Превращает массив цифр в список объектов
        /// </summary>
        public void ArrayToList()
        {
            int rMax = LevelData.GetUpperBound(1);
            int cMax = LevelData.GetUpperBound(0);
            for (int i = 0; i <= rMax; i++)
            {
                for (int j = 0; j <= cMax; j++)
                {

                    if (LevelData[j, i] == GenSettings.FloorNumber)
                        InstantiateObject(FloorObject, GlobalLevelPosition.X + i, GlobalLevelPosition.Y + j);
                    else if (LevelData[j, i] == GenSettings.WallNumber)
                        InstantiateObject(WallObject, GlobalLevelPosition.X + i, GlobalLevelPosition.Y + j);
                    else if (LevelData[j, i] == GenSettings.EnterNumber)
                    {
                        if(Level!=1)
                            InstantiateObject(EnterObject, GlobalLevelPosition.X + i, GlobalLevelPosition.Y + j);
                        else
                            InstantiateObject(SectionStartObject, GlobalLevelPosition.X + i, GlobalLevelPosition.Y + j);
                        if (Section==1 && Level==1)
                            InstantiateObject(PlayerSpawner, GlobalLevelPosition.X + i+1, GlobalLevelPosition.Y + j);
                    }
                    else if (LevelData[j, i] == GenSettings.ExitNumber)
                        InstantiateObject(ExitObject, GlobalLevelPosition.X + i, GlobalLevelPosition.Y + j);
                    else if (LevelData[j, i] == GenSettings.SectionExitNumber)
                        InstantiateObject(SectionEndObject, GlobalLevelPosition.X + i, GlobalLevelPosition.Y + j);
                    else if (LevelData[j, i] == GenSettings.SecretRoomEnterNumber)
                        InstantiateObject(SectionEndObject, GlobalLevelPosition.X + i, GlobalLevelPosition.Y + j);
                }
            }
        }

        public void SecretRoomEnter(int x, int y)
        {
            ListOfGameObjects.Remove(ListOfGameObjects.Find(mp =>
                mp.GlobalPosition.X == GlobalLevelPosition.X + x 
                && mp.GlobalPosition.Y == GlobalLevelPosition.Y + y 
                && mp.Prefab == WallObject));
            InstantiateObject(SecretRoomEnterObject, GlobalLevelPosition.X + x, GlobalLevelPosition.Y + y);
        }

        /// <summary>
        /// Добавляет объект в виде MazePosition в список всех объектов
        /// </summary>
        /// <param name="o">Ссылка на префаб объекта</param>
        /// <param name="x">Глобальная координата по X</param>
        /// <param name="y">Глобальная координата по Y</param>
        public void InstantiateObject(GameObject o, int x, int y)
        {
            MazePosition mazePosition=new MazePosition(new Point(x,y), Section, Level, o);
            ListOfGameObjects.Add(mazePosition);
        }
    }

}

