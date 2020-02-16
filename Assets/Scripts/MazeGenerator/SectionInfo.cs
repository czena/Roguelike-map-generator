using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Diagnostics;
using MazeGenerator;
using Debug = UnityEngine.Debug;
using ResourcesManager;

namespace MazeGenerator
{
    public class SectionInfo
    {
        public int SectionN { get; set; }
        public List<LevelInfo> Levels { get; set; }
        public GameManager GameManager { get; set; }
        public GameObject Floor { get; set; }
        public GameObject Wall { get; set; }
        public GameObject StartPoint { get; set; }
        public GameObject EndPoint { get; set; }
        public GameObject PlayerSpawner { get; set; }
        public GameObject Enemies { get; set; }
        public GameObject SectionPoint { get; set; }
        public GameObject SecretRoomPoint { get; set; }
        public GameObject SectionStartPoint { get; set; }

        public int GlobalCenterX { get; set; }
        public int GlobalCenterY { get; set; }
        public SectionInfo()
        {
            Levels = new List<LevelInfo>();
            GameManager = MonoBehaviour.FindObjectOfType<GameManager>();
        }
        public void CreateSection(List<SectionInfo> sections)
        {
            var constructor = new LevelConstructor(this);
            var goodSection = false; //Сгенерилась ли нормально секция
            var critical = 0;

            while (!goodSection && critical < GenSettings.MaxSectionGenerations)
            {
                for (var i = 1; i <= GenSettings.Levels; i++)
                {
                    LevelInfo level;
                    if (SectionN == 1 && Levels.Count == 0)
                        level = constructor.GenerateNewLevel(null);
                    else if (SectionN == 1 && Levels.Count > 0)
                        level = constructor.GenerateNewLevel(Levels.Last());
                    else if (SectionN != 1 && Levels.Count == 0)
                        level = constructor.GenerateNewLevel(sections.ElementAt(0).Levels.ElementAt(SectionN - 2));
                    else if (SectionN != 1 && Levels.Count > 0)
                        level = constructor.GenerateNewLevel(Levels.Last());
                    else
                        level = constructor.GenerateNewLevel(null);
                    if (level == null)
                    {
                        //Levels.Clear();
                        break;
                    }

                    level.Level = i;
                    if (SectionN == 2 && level.Level == 3 ||
                        SectionN == 3 && level.Level == 3 ||
                        SectionN == 4 && level.Level == 5 ||
                        SectionN == 5 && level.Level == 5 ||
                        SectionN == 6 && level.Level == 5 ||
                        SectionN == 7 && level.Level == 5 ||
                        SectionN == 8 && level.Level == 3 ||
                        SectionN == 9 && level.Level == 3)
                        FindCenterLevelY(level);
                    level.Section = SectionN;
                    Levels.Add(level);
                    if (SectionN == 1 && level.Level == 9)
                        FindCenterLevelX(this);
                    Debug.Log("Уровень " + i + " - " + "Секция " + SectionN);
                }

                if (Levels.Count == GenSettings.Levels )
                    goodSection = true;
                critical++;
                if (critical == GenSettings.MaxSectionGenerations && goodSection==false)
                {
                    //sections.Clear();
                    Debug.Log("Бесконечные циклы");
                }
                if(critical!= GenSettings.MaxSectionGenerations && goodSection == false)
                    Levels.Clear();
            }

            var enemiesCtor = new EnemiesConstructor();
            foreach (var room in Levels)
            {
                constructor.SetSectionsObjects(room);
                room.ArrayToList();
                enemiesCtor.EnemyCreator(room.LevelData, room.Enemies.EnemyData);
                enemiesCtor.EnemiesArrayToList(room);
            }
        }

        public void FindCenterLevelX(SectionInfo firstSection)
        {
            if (firstSection != null)
            {
                LevelInfo level;
                if (SectionN!=1)
                     level= firstSection.Levels.ElementAt(SectionN - 1);
                else
                     level = firstSection.Levels.Last();
                int minX = level.LevelData.GetLength(1), maxX = 0;
                for (var i = 0; i < level.LevelData.GetLength(1); i++)
                for (var j = 0; j < level.LevelData.GetLength(0); j++)
                {
                    if (level.LevelData[j, i] != null && i < minX)
                        minX = i;
                    if (level.LevelData[j, i] != null && i > maxX)
                        maxX = i;
                }
                if (SectionN != 1)
                    GlobalCenterX = (maxX - minX) / 2 + firstSection.Levels.ElementAt(SectionN - 2).GlobalLevelPosition.X;
                else
                {
                    GlobalCenterX = (maxX - minX) / 2 + firstSection.Levels.Last().GlobalLevelPosition.X;
                }
                    
            }
        }

        public void FindCenterLevelY(LevelInfo level)
        {
            if (SectionN == 2 || SectionN == 4 || SectionN == 6 || SectionN == 8 || SectionN == 10)
                GlobalCenterY = level.Enter.Y + GenSettings.OtherLevelsHeight / 2 + level.GlobalLevelPosition.Y;
            if (SectionN == 3 || SectionN == 5 || SectionN == 7 || SectionN == 9)
                GlobalCenterY = level.Enter.Y - GenSettings.OtherLevelsHeight / 2 + level.GlobalLevelPosition.Y;
            if (SectionN == 10)
                GlobalCenterY = GenSettings.OtherLevelsHeight / 2 + level.GlobalLevelPosition.Y;
        }
    }
}