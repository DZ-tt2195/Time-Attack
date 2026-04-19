using UnityEngine;
using UnityEngine.UI;
using MyBox;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System.Diagnostics;
public enum GameState {Setup, Playing, Paused, Over}
public class WaveManager : MonoBehaviour
{

#region Setup

    public static WaveManager instance;
    public static GameState state = GameState.Setup;
    List<BaseEnemy> allEnemies = new();
    List<Wave<Collection>> waveList = new();
    public int currentWave { get; private set; }
    Stopwatch gameTimer;

    [Foldout("Prefabs", true)]
    [SerializeField] Resupply resupplyPrefab;
    [SerializeField] HealthPack healthPack;
    Queue<Resupply> resupplyQueue = new();

    [Foldout("UI", true)]
    public Camera mainCamera;
    [SerializeField] Slider waveSlider;
    [SerializeField] TMP_Text waveCounter;
    [SerializeField] Slider enemySlider;
    [SerializeField] TMP_Text enemyCounter;
    [SerializeField] TMP_Text tutorialText;
    [SerializeField] TMP_Text replay;
    [SerializeField] TMP_Text quit;
    [SerializeField] TMP_Text endText;
    [SerializeField] TMP_Text timerText;
    [SerializeField] Slider energySlider;
    [SerializeField] TMP_Text energyCounter;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] Slider healthSlider;
    [SerializeField] TMP_Text healthCounter;
    [SerializeField] TMP_Text pause;
    [SerializeField] GameObject blackOutObject;
    [ReadOnly] public float blackOutTime = 0f;

    [Foldout("FPS", true)]
    int lastframe = 0;
    int lastupdate = 60;
    float[] framearray = new float[60];

    public static float minX { get; private set; }
    public static float maxX { get; private set; }
    public static float minY { get; private set; }
    public static float maxY { get; private set; }

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1f;

        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        minX = mainCamera.transform.position.x - cameraWidth / 2f;
        maxX = mainCamera.transform.position.x + cameraWidth / 2f;
        minY = mainCamera.transform.position.y - cameraHeight / 2f;
        maxY = 4f;

        replay.text = AutoTranslate.Replay();
        quit.text = AutoTranslate.Quit();
        pauseScreen.SetActive(false);

        state = GameState.Setup;
        gameTimer = new Stopwatch();

        Level currentLevel = ThingsToCarry.inst.CurrentLevel();
        waveList = currentLevel.listOfWaves;
        if (currentLevel.levelType == LevelType.Shuffled)
            waveList = waveList.Shuffle();

        Player player = ThingsToCarry.inst.RandomPlayer();
        Instantiate(player, new Vector3(0, -3), new Quaternion());
        BeginGame();
    }
    public void BeginGame()
    {
        state = GameState.Playing;
        gameTimer.Start();
        NewWave();
        InvokeRepeating(nameof(SpawnResupply), 1f, 2.25f);
    }

    #endregion

#region Gameplay

    void SpawnResupply()
    {
        Resupply resupply = (resupplyQueue.Count > 0) ? resupplyQueue.Dequeue() : Instantiate(resupplyPrefab);
        resupply.transform.position = new(Random.Range(minX + 0.5f, maxX - 0.5f), maxY);
        resupply.gameObject.SetActive(true);
    }
    public void ReturnResupply(Resupply resupply)
    {
        resupplyQueue.Enqueue(resupply);
        resupply.gameObject.SetActive(false);
    }
    void NewWave()
    {
        StartCoroutine(Player.instance.Immunity(false));
        Level currentLevel = ThingsToCarry.inst.CurrentLevel();

        if (currentWave < waveList.Count() || currentLevel.levelType == LevelType.Endless)
        {
            HealthPack pack = Instantiate(healthPack);
            pack.transform.position = new(Random.Range(minX + 0.5f, maxX - 0.5f), maxY);

            foreach (Collection collection in waveList[Mathf.Min(waveList.Count-1, currentWave)].enemies)
                CreateEnemy(collection.position, collection.toCreate);

            waveSlider.value = (currentWave + 1) / (float)waveList.Count;

            if (currentLevel.levelType != LevelType.Endless)
            {
                waveCounter.text = AutoTranslate.Wave((currentWave+1).ToString(), waveList.Count.ToString());
                tutorialText.text = Translator.inst.Translate(waveList[currentWave].tutorialKey);;
            }
            else
            {
                waveCounter.text = AutoTranslate.Wave((currentWave+1).ToString(), "\u221E");
                tutorialText.text = "";
            }
        }
        else
        {
            Bullet[] allBullets = FindObjectsByType<Bullet>(FindObjectsSortMode.None);
            foreach (Bullet bullet in allBullets) Destroy(bullet.gameObject);

            (int missedBullets, int tookDamage) = Player.instance.EndStats();
            int score = (int)(PrefManager.GetDifficulty() * 100) - missedBullets - tookDamage*2;
            string endText = AutoTranslate.Victory();
            
            if (score > PrefManager.GetScore(currentLevel.levelName.ToString()))
                PrefManager.SetScore(currentLevel.levelName.ToString(), score);
            EndGame(endText, new(missedBullets, tookDamage), score);
        }
    }
    void CreateEnemy(Vector2 start, BaseEnemy prefab)
    {
        BaseEnemy enemy = Instantiate(prefab != null ? prefab : ThingsToCarry.inst.RandomEnemy());
        enemy.EnemySetup();
        enemy.transform.position = start;
        allEnemies.Add(enemy);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pause.text = AutoTranslate.Paused();
            if (state == GameState.Playing)
            {
                state = GameState.Paused;
                gameTimer.Stop();
                pauseScreen.SetActive(true);
                Time.timeScale = 0f;                
            }
            else if (state == GameState.Paused)
            {
                state = GameState.Playing;
                gameTimer.Start();
                pauseScreen.SetActive(false);
                Time.timeScale = 1f;
            }
        }

        if (state != GameState.Playing) return;

        allEnemies.RemoveAll(enemy => enemy == null);
        int currentEnemies = 0;
        if (allEnemies.Count > 0)
        {
            foreach (BaseEnemy enemy in allEnemies)
            {
                if (enemy.currentHealth != 0)
                    currentEnemies++;
            }

            enemySlider.value = (float)currentEnemies / allEnemies.Count;
            enemyCounter.text = AutoTranslate.Enemies(currentEnemies.ToString(), allEnemies.Count().ToString());

            if (currentEnemies == 0)
            {
                for (int i = allEnemies.Count - 1; i >= 0; i--)
                    Destroy(allEnemies[i].gameObject);
                currentWave++;
                NewWave();
            }
        }

        (int health, int maxHealth, int energy, int maxEnergy) playerStats = Player.instance.HealthEnergy();
        healthSlider.value = playerStats.health / (float)playerStats.maxHealth;
        healthCounter.text = AutoTranslate.Health(playerStats.health.ToString(), playerStats.maxHealth.ToString());

        energySlider.value = playerStats.energy / (float)playerStats.maxEnergy;
        energyCounter.text = AutoTranslate.Energy(playerStats.energy.ToString(), playerStats.maxEnergy.ToString());

        timerText.text = $"{AutoTranslate.Difficulty($"{PrefManager.GetDifficulty()*100:F0}")}\n{MyExtensions.StopwatchTime(gameTimer)}";
        timerText.text += $" | {AutoTranslate.FPS(GetFPS())}";

        string GetFPS()
        {
            framearray[lastframe] = Time.deltaTime;
            lastframe++;
            if (lastframe == 60)
            {
                lastframe = 0;
                float total = 0;
                for (int i = 0; i < framearray.Length; i++)
                    total += framearray[i];
                lastupdate = (int)(framearray.Length / total);
                return lastupdate.ToString();
            }
            return (lastupdate > Application.targetFrameRate) ? Application.targetFrameRate.ToString() : lastupdate.ToString();
        }

        if (blackOutTime > 0f)
            blackOutTime -= Time.deltaTime;
        blackOutObject.SetActive(blackOutTime > 0f);
        blackOutObject.transform.position = Player.instance.transform.position;
    }
    public void EndGame(string text, (int missedBullets, int tookDamage) stats, int score)
    {
        if (!pauseScreen.activeSelf)
        {
            state = GameState.Over;
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;

            endText.text = $"{text}\n\n" +
                $"{AutoTranslate.Bullets_Missed(stats.missedBullets.ToString())}" +
                $"\n{AutoTranslate.Health_Lost(stats.tookDamage.ToString())}\n";
            if (score > 0)
                endText.text += $"\n{AutoTranslate.Score(score.ToString())}";
        }
    }

    #endregion

}