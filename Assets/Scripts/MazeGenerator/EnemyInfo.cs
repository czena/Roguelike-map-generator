using System.Collections.Generic;
using System.Linq;
using MazeGenerator.Collections;
using UnityEngine;

namespace MazeGenerator
{
    public class EnemyInfo
    {
        public int?[,] EnemyData { get; set; }//массив врагов в цифрах
        public List<GameObject> EnemiesStack { get; set; }
        public GameObject EnemiesPrefab { get; set; }
        

        public EnemyInfo()
        {
            EnemyData = new int?[GenSettings.Height, GenSettings.Width];
            EnemiesStack=new List<GameObject>();
        }

        public void SetEnemyObjects(LevelInfo level)
        {
            EnemiesStack = EnemiesPrefab.GetComponent<EnemyCollection>().GetEnemyObject(level.Level, level.Section);
        }

        public GameObject ReturnRandomEnemy()
        {

            if (EnemiesStack != null)
            {
                if (EnemiesStack.Count == 1)
                    return EnemiesStack.ElementAt(0);
                else
                    return EnemiesStack.ElementAt(GenSettings.Rand.Next(0, EnemiesStack.Count));
            }
            else
                return null;

        }

    }


}
