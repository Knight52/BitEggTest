using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Serializable]
    private struct LevelInfo
    {
        public int enemyAmount;
        public EnemyScript[] enemyPool;
    }
    [SerializeField]
    private LevelInfo defaultLevelInfo;
    [SerializeField]
    private LevelInfo[] levels;
    [SerializeField]
    private float spawnInterval;
    [SerializeField]
    private float timeBetweenLevels;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private MeshRenderer spawnArea;

    private float timeUntilNextSpawn;
    private LevelInfo currentLevelInfo;
    private int currentLevel;
    private List<EnemyScript> activeEnemies;
	// Use this for initialization
	void Start () {
        activeEnemies = new List<EnemyScript>();
        timeUntilNextSpawn = spawnInterval;
        currentLevelInfo = levels[0];
        currentLevel = 1;
        uiManager.SetLevel(currentLevel);
	}
	
	// Update is called once per frame
	void Update ()
    {
        timeUntilNextSpawn -= Time.deltaTime;
        if (timeUntilNextSpawn <= 0 && currentLevelInfo.enemyAmount > 0)
        {
            timeUntilNextSpawn = spawnInterval;
            EnemyScript enemy = Instantiate(currentLevelInfo.enemyPool[UnityEngine.Random.Range(0, currentLevelInfo.enemyPool.Length)]);
            enemy.transform.position = new Vector3(UnityEngine.Random.Range(spawnArea.bounds.center.x - spawnArea.bounds.size.x / 2, spawnArea.bounds.center.x + spawnArea.bounds.size.x / 2),
                UnityEngine.Random.Range(spawnArea.bounds.center.y - spawnArea.bounds.size.y / 2, spawnArea.bounds.center.y + spawnArea.bounds.size.y / 2), 0);
            enemy.Die += OnEnemyDie;
            activeEnemies.Add(enemy);
            currentLevelInfo.enemyAmount--;
        }
	}

    private void OnEnemyDie(EnemyScript enemy)
    {
        activeEnemies.Remove(enemy);
        if (activeEnemies.Count == 0 && currentLevelInfo.enemyAmount == 0)
        {
            currentLevel++;
            uiManager.SetLevel(currentLevel);
            timeUntilNextSpawn = timeBetweenLevels;
            if (currentLevel >= levels.Length)
            {
                currentLevelInfo = defaultLevelInfo;
                defaultLevelInfo.enemyAmount = currentLevel * 20;
            }
            else
            {
                currentLevelInfo = levels[currentLevel - 1];
            }
        }
    }
}
