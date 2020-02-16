namespace MazeGenerator
{
    public class EnemiesConstructor
    {

        public void EnemyCreator(int?[,] levelData, int?[,]enemiesData)
        {
            int e = 0;
            int j;
            int i;
            while (e < GenSettings.EnemiesOnLevel)
            {
                j = GenSettings.Rand.Next(0, levelData.GetLength(0));
                i = GenSettings.Rand.Next(0, levelData.GetLength(1));
                if (levelData[j, i] == GenSettings.FloorNumber && enemiesData[j, i] == null)
                {
                    if (levelData[j-1, i-1] != GenSettings.EnterNumber &&
                        levelData[j-1, i] != GenSettings.EnterNumber &&
                        levelData[j-1, i+1] != GenSettings.EnterNumber &&
                        levelData[j, i+1] != GenSettings.EnterNumber &&
                        levelData[j+1, i+1] != GenSettings.EnterNumber &&
                        levelData[j+1, i] != GenSettings.EnterNumber &&
                        levelData[j+1, i-1] != GenSettings.EnterNumber &&
                        levelData[j, i-1] != GenSettings.EnterNumber)
                    {
                        enemiesData[j, i] = 1;
                        e++;
                    }
                }
            }
        }

        public void EnemiesArrayToList(LevelInfo level)
        {
            level.Enemies.SetEnemyObjects(level);
            if (level.Enemies.EnemiesStack!=null && level.Enemies.EnemiesStack.Count>0)
                for (int i = 0; i < level.Enemies.EnemyData.GetLength(1); i++)
                {
                    for (int j = 0; j < level.Enemies.EnemyData.GetLength(0); j++)
                    {
                        if (level.Enemies.EnemyData[j, i] == 1)
                        {
                            level.ListOfGameObjects.Add(new MazePosition(new Point(level.GlobalLevelPosition.X + i, level.GlobalLevelPosition.Y + j), level.Section, level.Level, level.Enemies.ReturnRandomEnemy()));
                        }
                    }
                }
        }
    }
}
