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

    [Tooltip("Possible spawn positions for enemies")]
    public Transform[] spawnPoints;

    [Tooltip("Player reference (the enemies will target this)")]
    public Transform player;

    [Tooltip("Script that handles teleporting player to safe zone")]
    public retern_to_safe safeEventScript;

    [Tooltip("UI controller that updates wave and message text")]
    public WaveUIController uiController;


    [Header("Wave Settings")]
    [Tooltip("How many enemies spawn on the first wave")]
    public int startingEnemies = 5;

    [Tooltip("Delay between waves (used for normal waves)")]
    public float timeBetweenWaves = 3f;

    [Tooltip("Time between each enemy spawn within a wave")]
    public float spawnDelay = 0.3f;


    [Header("Enemy Scaling")]
    [Tooltip("Multiplier applied to enemy health every 5th wave")]
    public float healthIncrease = 1.2f;

    [Tooltip("Multiplier applied to enemy damage every 5th wave")]
    public float damageIncrease = 1.15f;



    // --- Runtime Variables ---
    private int currentWave = 0;                     // Tracks which wave the player is on
    private float specialEnemyChance = 0.1f;         // Starting chance for special enemies
    private List<GameObject> activeEnemies = new();  // Keeps track of alive enemies in current wave


    // Called automatically when the game starts
    void Start()
    {
        // Start the main wave loop
        StartCoroutine(WaveRoutine());
    }


    /// <summary>
    /// Main loop that controls the entire wave cycle.
    /// Handles wave progression, difficulty scaling, and special events.
    /// </summary>
    IEnumerator WaveRoutine()
    {
        while (true)
        {
            // --- Start New Wave ---
            currentWave++;
            uiController?.UpdateWaveText(currentWave);
            uiController?.ShowMessage($"Wave {currentWave} Starting!");
            Debug.Log($"--- WAVE {currentWave} START ---");

            // --- Fade out the message smoothly over 1.5 seconds ---
            if (uiController != null)
                yield return uiController.StartCoroutine(uiController.FadeOutMessage(0.5f));

            // --- Difficulty Scaling ---
            if (currentWave % 5 == 0)
            {
                // Every 5th wave, make enemies stronger
                healthIncrease *= 1.2f;
                damageIncrease *= 1.1f;
                Debug.Log("Enemies got stronger!");
            }

            // --- Increase Special Enemy Chance ---
            specialEnemyChance = Mathf.Min(0.5f, 0.1f + currentWave * 0.05f); // max 50%

            // --- Spawn Enemies ---
            int enemiesThisWave = startingEnemies + (currentWave - 1) * 2;
            yield return StartCoroutine(SpawnWave(enemiesThisWave));

            // --- Wait Until All Enemies Are Dead ---
            yield return new WaitUntil(() => activeEnemies.Count == 0);

            // --- Wave Completed ---
            Debug.Log($"--- WAVE {currentWave} CLEARED ---");
            uiController?.ShowMessage("Wave Cleared!");

            // --- Safe Zone Phase Trigger (Every 3rd Wave) ---
            if (currentWave % 3 == 0 && safeEventScript != null)
            {
                yield return StartCoroutine(SafeZonePhase());
            }
            else
            {
                // Normal delay between waves
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
    }


    /// <summary>
    /// Handles teleporting player to safe zone and waiting for player input
    /// to continue to the next wave.
    /// </summary>
    IEnumerator SafeZonePhase()
    {
        // --- Enter Safe Zone ---
        uiController?.ShowMessage("SAFE ZONE - Upgrade Time!");
        Debug.Log("Entering Safe Zone Phase...");
        safeEventScript.EnterSafeZone();

        // --- Give the player a moment to settle ---
        yield return new WaitForSeconds(1f);

        // --- Fade out the message smoothly over 1.5 seconds ---
        if (uiController != null)
            yield return uiController.StartCoroutine(uiController.FadeOutMessage(1.5f));

        // --- Show Continue Prompt ---
        uiController?.ShowContinuePrompt(true);

        Debug.Log("Player is in safe zone. Waiting for Enter key to continue...");

        // --- Wait for Enter key ---
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter));

        // --- Exit Safe Zone ---
        uiController?.ShowContinuePrompt(false);
        uiController?.ShowMessage("Next Wave Starting...");
        safeEventScript.ExitSafeZone();

        Debug.Log("Safe Zone phase ended. Starting next wave...");

        // --- Short delay before next wave ---
        yield return new WaitForSeconds(2f);
    }


    /// <summary>
    /// Spawns a wave of enemies, assigning each one a type and scaling their stats.
    /// </summary>
    IEnumerator SpawnWave(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Choose a random spawn point
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Instantiate the enemy
            GameObject enemy = Instantiate(enemyPrefab, spawn.position, Quaternion.identity);

            // Immediately assign the player reference
            Enemy_Script enemyScript = enemy.GetComponent<Enemy_Script>();
            enemyScript.player = GameObject.FindGameObjectWithTag("Player").transform;

            // Choose enemy type (Hitty, Shooty, Tanky, Lungie)
            int type = ChooseEnemyType();
            ApplyType(enemyScript, type);

            // --- ELITE LOGIC ---
            float eliteChance = Mathf.Min(0.05f + currentWave * 0.03f, 0.5f); // starts 5%, grows +3%/wave, max 50%
            bool isElite = Random.value < eliteChance;
            enemyScript.SetElite(isElite);

            // Scale stats based on wave difficulty
            enemyScript.health *= healthIncrease;
            enemyScript.damage *= damageIncrease;

            // Track active enemies
            activeEnemies.Add(enemy);
            StartCoroutine(RemoveOnDestroy(enemy));

            // Delay before spawning next enemy
            yield return new WaitForSeconds(spawnDelay);
        }
    }


    /// <summary>
    /// Removes an enemy from the active list once it’s destroyed.
    /// </summary>
    IEnumerator RemoveOnDestroy(GameObject enemy)
    {
        while (enemy != null)
            yield return null;

        activeEnemies.RemoveAll(e => e == null);
    }


    /// <summary>
    /// Decides which enemy type to spawn based on the current special enemy chance.
    /// </summary>
    int ChooseEnemyType()
    {
        float roll = Random.value;

        if (roll < specialEnemyChance / 3f) return 2; // Shooty
        if (roll < (specialEnemyChance * 2f) / 3f) return 3; // Tanky
        if (roll < specialEnemyChance) return 4; // Lungie
        return 1; // Hitty
    }


    /// <summary>
    /// Applies the appropriate type settings to an enemy.
    /// </summary>
    void ApplyType(Enemy_Script enemy, int type)
    {
        // Reset all flags first
        enemy.hitty = false;
        enemy.shooty = false;
        enemy.tanky = false;
        enemy.lungie = false;

        // Apply correct behavior/stats
        switch (type)
        {
            case 1: enemy.Hitty(); break;
            case 2: enemy.Shooty(); break;
            case 3: enemy.Tanky(); break;
            case 4: enemy.Lungie(); break;
        }
    }
}