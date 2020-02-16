using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace MazeGenerator.Collections
{
    class EnemyCollection:MonoBehaviour
    {
        [SerializeField] private  GameObject[] _enemy=new GameObject[GenSettings.EnemyCount]; // Массив для ссылок на ресурсы-спрайты.

        public List<GameObject> GetEnemyObject(int lvl, int sec)
        {
            List<GameObject> currentEnemies=new List<GameObject>();
            foreach (var e in _enemy)
            {
                if (e != null && 
                    e.GetComponent<Enemy>().MaxLvl>=(sec-1)*GenSettings.Levels +lvl && 
                    e.GetComponent<Enemy>().MinLvl <= (sec - 1) * GenSettings.Levels + lvl)
                {
                    currentEnemies.Add(e);
                }
            }
            if (currentEnemies.Count > 0)
                return currentEnemies;
            else
                return null;
        }
    }
}
