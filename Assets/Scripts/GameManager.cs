using System.Diagnostics;
using MazeGenerator;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using ResourcesManager;

[RequireComponent(typeof(SectionConstructor))]//Атрибут RequireComponent обеспечивает добавление компонента MazeConstructor при добавлении этого скрипта к GameObject.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public ResourceManager ResManager { get; set; }
    public GameObject[] Enemies;
    public Text TimerText;
    private Stopwatch _stopwatch;

    public SectionConstructor SectionGenerator { get; set; }

    private void Awake()
    {
        Debug.Log("AwakeGameManager");
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        _stopwatch = new Stopwatch();
			
    }
		
    void Start()
    {
        _stopwatch.Start();
        ResManager=new ResourceManager();
        if (SceneManager.GetActiveScene().name == "Game")
        {
            SectionGenerator = GetComponent<SectionConstructor>();
            SectionGenerator.CreateSection();
            _stopwatch.Stop();
            SectionGenerator.PlayerSpawn();
            var stats= FindObjectOfType<PlayerStats>();
            SectionGenerator.DisplayLevel(stats.CurrentStage, stats.CurrentSection);
        }
        if (SceneManager.GetActiveScene().name == "Maze")
        {
            //TestGeneration();
            SectionGenerator = GetComponent<SectionConstructor>();
            SectionGenerator.CreateSection();
            _stopwatch.Stop();
            SectionGenerator.DisplayWorld();

            //GameObject[] spawnPositions = GameObject.FindGameObjectsWithTag("Floor");
            //for (int i = 0; i < spawnPositions.Length; i += Random.Range(6, 20))
            //{
            //	Instantiate(enemies[Random.Range(0, 5)], spawnPositions[i].transform.position, Quaternion.identity);
            //}
        }
        TimerText.text = "Время генерации секций в милисекундах: " + _stopwatch.ElapsedMilliseconds;
    }

    private void TestGeneration()
    {
        SectionGenerator = GetComponent<SectionConstructor>();
        double averangeTime = 0;
        int notCorrect = 0;
        int n = 100;
        for (int i = 0; i < n; i++)
        {

            _stopwatch.Start();
            SectionGenerator.CreateSection();
            _stopwatch.Stop();
            averangeTime += _stopwatch.ElapsedMilliseconds;
            Debug.Log(i+" "+ _stopwatch.ElapsedMilliseconds);
            _stopwatch.Reset();
            if (!SectionGenerator)
                notCorrect++;
            SectionGenerator.SectionsClear();
        }
        Debug.Log("Всего времени "+ averangeTime+"; Количество проходов: " + n+ "; Среднее время "+ averangeTime / n + "; "+"Плохие генерации: "+ notCorrect);
        //SectionGenerator.DisplayWorld();
    }
}