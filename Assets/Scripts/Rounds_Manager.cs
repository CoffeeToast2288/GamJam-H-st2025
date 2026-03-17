using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles wave progression, spawning enemies, scaling difficulty,
/// triggering safe zone events, and coordinating UI feedback.
/// Attach this script to an empty GameObject (e.g., "WaveManager").
/// </summary>

public class WaveSystem : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Enemy prefab with Enemy_Script component")]
    public GameObject enemyPrefab;

    [Tooltip("Upgrade logic that makes the start of the process work")]
    public GameObject upgradeParts;
    [SerializeField] UpgradeOpen upgradingMore;
    [SerializeField] upgrades upgradeScript;

    [Tooltip("Possible spawn positions for enemies")]
    public Transform[] spawnPoints;

    [Tooltip("Player reference (the enemies will target this)")]
    public Transform player;

    [Tooltip("Script that handles teleporting player to safe zone")]
    public retern_to_safe safeEventScript;

    [Tooltip("UI controller that updates wave and message text")]
    public WaveUIController uiController;

    [Header("Enemy Tracking UI")]
    [Tooltip("Text element showing how many enemies remain. Place it anywhere in your Canvas.")]
    public TMPro.TMP_Text enemyCounterText;

    [Tooltip("Arrow indicator prefab (assign EnemyArrowIndicator prefab here)")]
    public GameObject arrowIndicatorPrefab;

    [Tooltip("How many enemies must remain before arrows appear")]
    public int arrowThreshold = 3;

    public GameObject arrowSpawn;

    private int totalEnemiesThisWave = 0;
    private int enemiesKilled = 0;


    [Header("Wave Settings")]
    [Tooltip("How many enemies spawn on the first wave")]
    public int startingEnemies = 5;

    [Tooltip("Delay between waves (used for normal waves)")]
    public float timeBetweenWaves = 3f;

    [Tooltip("Time between each enemy spawn within a wave")]
    public float spawnDelay = 0.3f;


    [Header("Enemy Scaling")]
    [Tooltip("Multiplier applied to enemy health every increase")]
    public float healthIncrease = 1f;

    [Tooltip("Multiplier applied to enemy damage every increase")]
    public float damageIncrease = 1f;

    [Tooltip("Multiplier applied to enemy speed every increase")]
    public float speedIncrease = 0.5f;


    // --- Runtime Variables ---
    private int currentWave = 0;
    private float specialEnemyChance = 0.1f;
    private List<GameObject> activeEnemies = new();

    // Arrow indicators — one per tracked enemy
    private Dictionary<GameObject, GameObject> arrowIndicators = new();


    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        StartCoroutine(WaveRoutine());
    }


    void Update()
    {
        // Update enemy counter text every frame
        UpdateEnemyCounter();

        // Show/hide arrows based on remaining enemy count
        UpdateArrowVisibility();
    }


    /// <summary>
    /// Updates the on-screen enemy counter text.
    /// </summary>
    void UpdateEnemyCounter()
    {
        if (enemyCounterText != null)
            enemyCounterText.text = $"Enemies: {activeEnemies.Count}";
    }


    /// <summary>
    /// Spawns or destroys arrow indicators depending on how many enemies remain.
    /// When above threshold, all arrows are hidden. At or below, each living enemy
    /// gets its own arrow indicator.
    /// </summary>
    void UpdateArrowVisibility()
    {
        int enemiesRemaining = totalEnemiesThisWave - enemiesKilled;
        bool showArrows = enemiesRemaining <= arrowThreshold && activeEnemies.Count > 0;

        if (showArrows)
        {
            // Spawn a new arrow for any enemy that doesn't have one yet
            foreach (GameObject enemy in activeEnemies)
            {
                if (enemy == null) continue;

                if (!arrowIndicators.ContainsKey(enemy))
                {
                    GameObject arrow = Instantiate(arrowIndicatorPrefab, arrowSpawn.transform);

                    // Tell the arrow which enemy and player to track
                    EnemyArrowIndicator indicator = arrow.GetComponent<EnemyArrowIndicator>();
                    if (indicator != null)
                    {
                        indicator.target = enemy.transform;
                        indicator.player = player;
                    }

                    arrowIndicators[enemy] = arrow;
                }
            }
        }

        // Clean up arrows for dead enemies, or hide all if above threshold
        List<GameObject> toRemove = new();
        foreach (var kvp in arrowIndicators)
        {
            if (kvp.Key == null || !showArrows)
            {
                Destroy(kvp.Value);
                toRemove.Add(kvp.Key);
            }
        }
        foreach (var key in toRemove)
            arrowIndicators.Remove(key);
    }


    /// <summary>
    /// Clears all arrow indicators — called when a wave ends.
    /// </summary>
    void ClearAllArrows()
    {
        foreach (var kvp in arrowIndicators)
            if (kvp.Value != null) Destroy(kvp.Value);

        arrowIndicators.Clear();
    }


    IEnumerator WaveRoutine()
    {
        while (true)
        {
            currentWave++;
            uiController?.UpdateWaveText(currentWave);
            uiController?.ShowMessage($"Wave {currentWave} Starting!");
            Debug.Log($"--- WAVE {currentWave} START ---");

            if (uiController != null)
                yield return uiController.StartCoroutine(uiController.FadeOutMessage(0.5f));
            if (currentWave >= 2)
            {
                // Damage: caps at +2 by wave 10 — roughly doubles a base-3 enemy at peak
                float damageScale = Mathf.Min(currentWave * 0.2f, 2f);
                damageIncrease = damageScale;

                // Health: caps at +8 by wave 16 — makes tanky enemies feel meatier without being sponges
                float healthScale = Mathf.Min(currentWave * 0.5f, 8f);
                healthIncrease = healthScale;

                // Speed: tiny increases, caps early — enemies should feel faster but never undodgeable
                float speedScale = Mathf.Min(currentWave * 0.03f, 0.5f);
                speedIncrease = speedScale;
            }

            specialEnemyChance = Mathf.Min(0.5f, 0.1f + currentWave * 0.05f);

            int enemiesThisWave = startingEnemies + (currentWave - 1) * 2;
            totalEnemiesThisWave = enemiesThisWave;
            enemiesKilled = 0;
            yield return StartCoroutine(SpawnWave(enemiesThisWave));

            yield return new WaitUntil(() => activeEnemies.Count == 0);

            // Wave is over — clean up any leftover arrows
            ClearAllArrows();

            Debug.Log($"--- WAVE {currentWave} CLEARED ---");
            uiController?.ShowMessage("Wave Cleared!");

            if (currentWave % 2 == 0 && safeEventScript != null)
                yield return StartCoroutine(SafeZonePhase());
            else
                yield return new WaitForSeconds(timeBetweenWaves);
        }
    }


    IEnumerator SafeZonePhase()
    {
        uiController?.ShowMessage("SAFE ZONE - Upgrade Time!");
        Debug.Log("Entering Safe Zone Phase...");
        safeEventScript.EnterSafeZone();

        yield return new WaitForSeconds(2f);

        if (uiController != null)
            yield return uiController.StartCoroutine(uiController.FadeOutMessage(1.5f));

        upgradingMore.hasOpened = false;
        upgradeParts.SetActive(true);
        upgradeScript.StartSafeZone(currentWave);

        uiController?.ShowContinuePrompt(true);
        Debug.Log("Player is in safe zone. Waiting for Enter key to continue...");

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter));

        upgradeParts.SetActive(false);
        uiController?.ShowContinuePrompt(false);
        uiController?.ShowMessage("Next Wave Starting...");
        safeEventScript.ExitSafeZone();

        Debug.Log("Safe Zone phase ended. Starting next wave...");
        yield return new WaitForSeconds(2f);
    }


    IEnumerator SpawnWave(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyPrefab, spawn.position, Quaternion.identity);

            Enemy_Script enemyScript = enemy.GetComponent<Enemy_Script>();
            enemyScript.player = GameObject.FindGameObjectWithTag("Player").transform;

            int type = ChooseEnemyType();
            ApplyType(enemyScript, type);

            float eliteChance = Mathf.Min(0.05f + currentWave * 0.03f, 0.5f);
            bool isElite = Random.value < eliteChance;
            enemyScript.SetElite(isElite);

            enemyScript.health += healthIncrease;
            enemyScript.damage += damageIncrease;
            enemyScript.moveSpeed += speedIncrease;

            activeEnemies.Add(enemy);
            StartCoroutine(RemoveOnDestroy(enemy));

            yield return new WaitForSeconds(spawnDelay);
        }
    }


    IEnumerator RemoveOnDestroy(GameObject enemy)
    {
        while (enemy != null)
            yield return null;

        enemiesKilled++; 
        activeEnemies.RemoveAll(e => e == null);
    }


    int ChooseEnemyType()
    {
        float roll = Random.value;
        if (roll < specialEnemyChance / 3f) return 2;
        if (roll < (specialEnemyChance * 2f) / 3f) return 3;
        if (roll < specialEnemyChance) return 4;
        return 1;
    }


    void ApplyType(Enemy_Script enemy, int type)
    {
        enemy.hitty = false;
        enemy.shooty = false;
        enemy.tanky = false;
        enemy.lungie = false;

        switch (type)
        {
            case 1: enemy.Hitty(); break;
            case 2: enemy.Shooty(); break;
            case 3: enemy.Tanky(); break;
            case 4: enemy.Lungie(); break;
        }
    }
}