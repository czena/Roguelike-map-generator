
namespace MazeGenerator
{
    /// <summary>
    /// Класс для магический чисел.
    /// </summary>
    public static class GenSettings 
    {
        public static uint MinLeafSize = 5;//default 5 //минимальный размер области под комнату
        public static uint MaxLeafSize = 20;//default 20 //максимальный размер области под комнату
        public static uint MinRoomSize = MinLeafSize - 2;//default MinLeafSize-2 //минимальный размер комнаты
        public static uint Width = 30;//default 30 //ширина уровня
        public static uint Height = 30;//default 30 //высота уровня
        public static uint MinPath = 50;//default 50 //минимальный путь для выхода с уровня
        public static uint MinHall = 1;//default 1 //минимальная ширина коридора
        public static uint MaxHall = 3;//default 3 //максимальная ширина коридора
        public static uint MinFloors = 300;//default 300 //минимальное количество пола на уровне
        public static uint MaxFloors = 400;//default 400 //максимальное количество пола на уровне
        public static uint HalfWidthSectionOne = Height-5; //Height-5 //Ширина первой секции
        public static uint HoleForSectionOne = 10;//default 10//ширина основания для дыры в первой секции//четные лучше работают
        public static uint MaxGenerations = 100;//default 100//максимальное количество генераций перед регенерацией
        public static int MaxSectionGenerations = 100;
        public static System.Random Rand = new System.Random();

        public static uint Sections = 10;//default 10//количество секций в мире
        public static uint Levels = 10;//default 10//количество уровней в секции
        public static uint EnemyCount = 27;//default 27//количество обычных мобов


        public static int EnemiesOnLevel = 15;//Враги на уровне


        public static int OtherLevelsWidth = 35;
        public static int OtherLevelsHeight = 35;

        //Отждествления номеров в массиве уровней с префабами
        public static int FloorNumber = 0;
        public static int WallNumber = 1;
        public static int EnterNumber = 2;
        public static int ExitNumber = 3;
        public static int SectionExitNumber = 4;
        public static int SecretRoomEnterNumber = 5;

        //Секретные комнаты
        public static float ChanceOfSecretRoom=1;
        public static int Grow=5;
        public static int SecretRoomFloorCount = 20;

        //Пути к ресурсам
        public static string FloorPath="BasePrefabs/Floors/Floor";
        public static string EndPointPath = "BasePrefabs/EndPoint/EndPoint";
        public static string SecretRoomPath = "BasePrefabs/SecretRoomPoint/SecretWall";
        public static string SectionPointsEndPath = "BasePrefabs/EndSectionPoint/EndSectionPoint";
        public static string SectionPointsStartPath = "BasePrefabs/StartSectionPoint/StartSectionPoint";
        public static string StartPointPath = "BasePrefabs/StartPoint/StartPoint";
        public static string WallPath = "BasePrefabs/Walls/Wall";
        public static string PlayerSpawnerPath = "BasePrefabs/PlayerPrefabs/PlayerSpawner";
        public static string PlayerPath = "BasePrefabs/PlayerPrefabs/Player";

    }
}
