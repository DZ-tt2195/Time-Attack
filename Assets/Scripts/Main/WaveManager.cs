using UnityEngine;
using UnityEngine.UI;
using MyBox;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class WaveManager : MonoBehaviour
{

#region Setup

    public static WaveManager instance;
    List<BaseEnemy> allEnemies = new();
    List<Wave<Collection>> waveList = new();

    [Foldout("Prefabs", true)]
    [SerializeField] Resupply resupplyPrefab;
    [SerializeField] HealthPack healthPack;
    Queue<Resupply> resupplyQueue = new();

    [Foldout("UI", true)]
    public Camera mainCamera;
    [SerializeField] Slider waveSlider;
    [SerializeField] TMP_Text waveCounter;
    public int currentWave { get; private set; }
    [SerializeField] Slider enemySlider;
    [SerializeField] TMP_Text enemyCounter;
    [SerializeField] TMP_Text tutorialText;
    [SerializeField] TMP_Text replay;
    [SerializeField] TMP_Text quit;
    [SerializeField] TMP_Text endText;

    public static float minX { get; private set; }
    public static float maxX { get; private set; }
    public static float minY { get; private set; }
    public static float maxY { get; private set; }

    private void Awake()
    {
        instance = this;

        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        minX = mainCamera.transform.position.x - cameraWidth / 2f;
        maxX = mainCamera.transform.position.x + cameraWidth / 2f;
        minY = mainCamera.transform.position.y - cameraHeight / 2f;
        maxY = 4f;
        Debug.Log($"X: {minX} to {maxX}; Y: {minY} to {maxY}");

        replay.text = AutoTranslate.Replay();
        quit.text = AutoTranslate.Quit();

        InvokeRepeating(nameof(SpawnResupply), 1f, 2.25f);
        currentWave = PrefManager.GetStartWave()-1;

        Level currentLevel = ThingsToCarry.inst.CurrentLevel();
        waveList = currentLevel.listOfWaves;
        if (currentLevel.levelType == LevelType.Shuffled)
            waveList = waveList.Shuffle();
        NewWave();
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
            JuggleBall[] allBalls = FindObjectsByType<JuggleBall>(FindObjectsSortMode.None);
            foreach (JuggleBall ball in allBalls) Destroy(ball.gameObject);

            (int missedBullets, int tookDamage) = Player.instance.PlayerStats();
            int score = (int)(PrefManager.GetDifficulty() * 100) - missedBullets - tookDamage*2;
            string endText = AutoTranslate.Victory();
            
            if (PrefManager.GetStartWave() > 1)
                endText += $" {AutoTranslate.Skipped_Ahead(PrefManager.GetStartWave().ToString())}";
            else if (score > PrefManager.GetScore(currentLevel.levelName.ToString()))
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
        allEnemies.RemoveAll(enemy => enemy == null);
        int currentEnemies = 0;
        if (allEnemies.Count > 0)
        {
            foreach (BaseEnemy enemy in allEnemies)
            {
                if (enemy.health != 0)
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
    }
    public void EndGame(string text, (int missedBullets, int tookDamage) stats, int score)
    {
        if (!endText.transform.parent.gameObject.activeSelf)
        {
            endText.transform.parent.gameObject.SetActive(true);
            endText.text = $"{text}\n\n" +
                $"{AutoTranslate.Bullets_Missed(stats.missedBullets.ToString())}" +
                $"\n{AutoTranslate.Health_Lost(stats.tookDamage.ToString())}\n";
            if (score > 0)
                endText.text += $"\n{AutoTranslate.Score(score.ToString())}";
        }
    }

    #endregion

}