using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/// <summary>
/// Contains the logic for spawning enemy waves ath the start of the path
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject infoScreen;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;     // greater value means more enemies
    [SerializeField] private float enemiesPerSecondCap = 15f;

    [Header("Events")]
    private static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float actualEnemiesPerSecond;
    private bool isSpawning = false;
    private bool checkForEnd = false;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
        startButton.onClick.AddListener(() => StartGame());
    }

    public void Start()
    {
        // coroutine for time based waves 
        StartCoroutine(StartWave());
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        infoScreen.SetActive(false);
        UIManager.Instance.SetHoveringState(false);
    }

    private void Update()
    {
        if (isSpawning && !checkForEnd)
        {
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= (1f / actualEnemiesPerSecond) && enemiesLeftToSpawn > 0)
            {
                SpawnEnemy();
                enemiesLeftToSpawn--;
                enemiesAlive++;
                timeSinceLastSpawn = 0f;
            }

            // start new wave if no enemies are left
            if (enemiesAlive == 0 && enemiesLeftToSpawn == 0 && !GameManager.Instance.IsFinished())
            {
                checkForEnd = true;
                EndWave();
            }
        }
    }

    /// <summary>
    ///     Destroys an enemy, i.e., decreases the number of enemies alive
    /// </summary>
    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    /// <summary>
    ///     Spwans a random enemy at the beginning of the path
    /// </summary>
    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[index];
        Instantiate(prefabToSpawn, LevelManager.Instance.GetStartPoint().position, Quaternion.identity );
        Debug.Log("Spawn Enemy");
    }

    /// <summary>
    ///     Handles phase between subsequent waves
    /// </summary>
    /// <returns>Iterator for coroutine</returns>
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = baseEnemies;
        actualEnemiesPerSecond = EnemiesPerSecond();
    }

    /// <summary>
    ///     Ends a wave, thus starting the intermission coroutine.
    /// </summary>
    private async void EndWave()
    {
        bool isFinished = await QuestionManager.Instance.CheckForEnd();

        if (isFinished)
        {
            GameManager.Instance.LoadEndScreen();
        }
        else
        {
            isSpawning = false;
            timeSinceLastSpawn = 0f;
            currentWave++;
            StartCoroutine(StartWave());
            checkForEnd = false;
        }
    }

    /// <summary>
    ///     Calculates the number of enemies per wave depending on the difficulty factor 
    /// </summary>
    /// <returns>number of enemies per wave</returns>
    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    /// <summary>
    ///     Calculates the the actual enemy per seconds rate depending on the difficulty factor
    /// </summary>
    /// <returns>actual enemy per second rate</returns>
    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave,
            difficultyScalingFactor), 0f, enemiesPerSecondCap);
    }

    /// <summary>
    ///     Gets the unity event that describes what happens if an enemy gets destroyed
    /// </summary>
    /// <returns>on enemy destroyed event </returns>
    public static UnityEvent GetOnEnemyDestroy()
    {
        return onEnemyDestroy;
    }
}
