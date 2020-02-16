using System.Collections.Generic;
using System.Linq;
using ControllerScripts;
using Enemies;
using UnityEngine;
using UnityEngine.Events;

namespace MazeGenerator
{
    public class SectionConstructor: MonoBehaviour
    {
        private readonly List<SectionInfo> _sections;
        private PlayerController _playerController;
        public bool IsCorrect { get; set; }

        public SectionConstructor()
        {
            _sections = new List<SectionInfo>();
        }

        public void CreateSection()
        {
            for (var i = 1; i <= GenSettings.Sections; i++)
            {
                var section = new SectionInfo
                {
                    SectionN = i
                };
                if (section.SectionN != GenSettings.Sections)
                    section.FindCenterLevelX(_sections.Count == 0 ? null : _sections.ElementAt(0));
                else if (section.SectionN == GenSettings.Sections)
                    section.FindCenterLevelY(_sections.ElementAt(0).Levels.ElementAt(section.SectionN - 2));
                GetObjects(section);
                section.CreateSection(_sections);
                if (section.Levels.Count != GenSettings.Levels)
                {
                    //Debug.Log("asdasdasd");
                    _sections.Add(section);
                    IsCorrect = false;
                    return;
                }

                _sections.Add(section);
            }
            SecretRoomConstructor secretConstructor= new SecretRoomConstructor(_sections);
            for (int level = 0; level < GenSettings.Levels-1; level++)
            {
                for (int section = 0; section < GenSettings.Sections; section++)
                {
                    if(GenSettings.Rand.NextDouble()<GenSettings.ChanceOfSecretRoom )
                        secretConstructor.GenerateSecretRooms(section, level);
                }
            }
            IsCorrect = true;
        }

        public void GetObjects(SectionInfo section)
        {
            //section.Floor = _floors.GetComponent<Collections.Collections>().GetLevelObject(section.SectionN - 1);
            //section.Wall = _walls.GetComponent<Collections.Collections>().GetLevelObject(section.SectionN - 1);
            //section.EndPoint = _endPoints.GetComponent<Collections.Collections>().GetLevelObject(section.SectionN - 1);
            //section.StartPoint = _startPoints.GetComponent<Collections.Collections>().GetLevelObject(section.SectionN - 1);
            section.Enemies = _enemies;
            //section.SectionPoint = _sectionPoints.GetComponent<Collections.Collections>().GetLevelObject(section.SectionN - 1);
            //section.SecretRoomPoint = _secretRoomPoints.GetComponent<Collections.Collections>().GetLevelObject(section.SectionN - 1);
            //section.SectionStartPoint = _startSectionPoint.GetComponent<Collections.Collections>()
            //    .GetLevelObject(section.SectionN - 1);
            //if (section.SectionN == 1)
            //    section.PlayerSpawner = _playerSpawner;
        }

        public void DisplayWorld()
        {
            foreach (var section in _sections)
                foreach (var level in section.Levels)
                {
                    foreach (var mazePosition in level.ListOfGameObjects)
                    {
                        GameObject obj = Instantiate(mazePosition.Prefab,
                            new Vector3(mazePosition.GlobalPosition.X, mazePosition.GlobalPosition.Y, 0.1f),
                            Quaternion.identity);
                        obj.name = mazePosition.Prefab.name;
                        if (obj.tag != "PlayerSpawner")
                            obj.layer = 10;
                    }

                    if (level.ListOfSecretRoomObjects != null && level.IsSecretRoomActive)
                    {
                        //Debug.Log("Secret!!!!!!!!!!!!!!!!!!!");
                        foreach (var mazePosition in level.ListOfSecretRoomObjects)
                        {
                            GameObject obj = Instantiate(mazePosition.Prefab,
                                new Vector3(mazePosition.GlobalPosition.X, mazePosition.GlobalPosition.Y, 0.1f),
                                Quaternion.identity);
                            obj.name = mazePosition.Prefab.name;
                            if (obj.tag != "PlayerSpawner")
                                obj.layer = 10;
                        }
                    }
                }

        }

        public void PlayerSpawn()
        {
            foreach (var section in _sections)
            foreach (var level in section.Levels)
            {
                MazePosition maze = level.ListOfGameObjects.Find(ps => ps.Prefab.CompareTag("PlayerSpawner"));
                if (maze != null)
                {
                    GameObject obj = Instantiate(maze.Prefab,
                        new Vector3(maze.GlobalPosition.X, maze.GlobalPosition.Y, 0.1f),
                        Quaternion.identity);
                    obj.name = maze.Prefab.name;
                    if (obj.tag != "PlayerSpawner")
                            obj.layer = 10;
                    return;
                }
            }
        }

        public void DisplayLevel(int lvl, int scn)
        {
            DestroyLevel();
            var level = _sections.Find(s => s.SectionN == scn).Levels.Find(l => l.Level == lvl);
            foreach (var mazePosition in level.ListOfGameObjects)
            {
                if (mazePosition.Prefab.tag != "PlayerSpawner")
                {
                    GameObject obj = Instantiate(mazePosition.Prefab,
                        new Vector3(mazePosition.GlobalPosition.X, mazePosition.GlobalPosition.Y, 0.1f),
                        Quaternion.identity);
                    obj.name = mazePosition.Prefab.name;
                    if (obj.tag != "PlayerSpawner")
                        obj.layer = 10;
                }

            }

            if (level.ListOfSecretRoomObjects != null && level.IsSecretRoomActive)
            {
                //Debug.Log("Secret!!!!!!!!!!!!!!!!!!!");
                foreach (var mazePosition in level.ListOfSecretRoomObjects)
                {
                    GameObject obj = Instantiate(mazePosition.Prefab,
                        new Vector3(mazePosition.GlobalPosition.X, mazePosition.GlobalPosition.Y, 0.1f),
                        Quaternion.identity);
                    obj.name = mazePosition.Prefab.name;
                    if(obj.tag != "PlayerSpawner")
                        obj.layer = 10;
                }
            }
        }

        private void DestroyLevel()
        {
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            _playerController= FindObjectOfType<PlayerController>();
            foreach (var item in allObjects)
            {
                if (item.layer==10/* && item.tag!="PlayerSpawner"*/)

                {
                    if (item.tag == "Enemy")
                    {
                        item.GetComponent<Enemy>().UndescribableEvents();
                    }
                    Destroy(item);
                }
            }
        }

        public void DisplaySecretRoom(int s, int l)
        {
            LevelInfo level = _sections.Find(sec => sec.SectionN == s).Levels.Find(lev => lev.Level == l);
            level.IsSecretRoomActive = true;
            foreach (var mazePosition in level.ListOfSecretRoomObjects)
            {
                GameObject obj = Instantiate(mazePosition.Prefab,
                    new Vector3(mazePosition.GlobalPosition.X, mazePosition.GlobalPosition.Y, 0.1f),
                    Quaternion.identity);
                obj.name = mazePosition.Prefab.name;
                if (obj.tag != "PlayerSpawner")
                    obj.layer = 10;
            }
        }

        public void DestroyEnemy(int section, int level, int x, int y)
        {
            var enemy = _sections.Find(s => s.SectionN ==section).Levels.Find(l => l.Level == level).ListOfGameObjects;
                enemy.Remove(enemy.Find(e => e.Prefab.tag == "Enemy" && e.GlobalPosition.X == x && e.GlobalPosition.Y == y));
        }

        public void SectionsClear()
        {
            _sections.Clear();
        }

        //[SerializeField] private GameObject _floors;
        //[SerializeField] private GameObject _walls;
        //[SerializeField] private GameObject _startPoints;
        //[SerializeField] private GameObject _endPoints;
        //[SerializeField] private GameObject _sectionPoints;
        //[SerializeField] private GameObject _playerSpawner;
        [SerializeField] private GameObject _enemies;
        //[SerializeField] private GameObject _secretRoomPoints;
        //[SerializeField] private GameObject _startSectionPoint;
    }
}