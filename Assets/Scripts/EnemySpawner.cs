using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private List<Damagable> _enemieList;
    public Dictionary<EnemyType, Damagable> EnemieList = new Dictionary<EnemyType, Damagable>();
    [SerializeField] private DaySettings[] DayConditions;
    [SerializeField] private Transform _propHolder;
    [SerializeField] List<GameObject> _propList = new List<GameObject>();


 


    private void Start()
    {
        Game.Instance.StateChanged += OnStateChanged;
        foreach(var enemie in _enemieList)
        {
            EnemieList.Add(enemie.Type, enemie);
        }
    }

    private void OnStateChanged(GameState state)
    {
        StopAllCoroutines();
        switch (state)
        {
            case GameState.StartingGame:
                break;
            case GameState.Day:
               StartCoroutine(SpawnProps(DayConditions[Game.Instance.CurrentDay - 1].EnviriomentAmount));
                break;
            case GameState.Night:
                SpawnEnemiesOnTurn(Game.Instance.CurrentDay);
                
                break;
            case GameState.EndGame:
                break;
        }
    }

    private IEnumerator SpawnProps(int amount)
    {
        yield return new WaitForSeconds(1f);
        foreach(Transform prop in _propHolder)
        {
            Destroy(prop.gameObject);
        }

        for(int i = 0; i < amount; i++)
        {
            Instantiate(_propList[Random.Range(0, _propList.Count)], GetSpawnPosition(), Quaternion.identity,_propHolder);
        }
    }

    private void SpawnEnemiesOnTurn(int day)
    {
        foreach(var daySettings in DayConditions[day - 1].SpawnConditions)
        {
            StartCoroutine(SpawnUnits(daySettings));
        }
    }

    IEnumerator SpawnUnits(SpawnCondition condition)
    {
        int enemyCount = 0;
        yield return new WaitForSeconds(Random.Range(1f,3f));
        Player.Instance.AddEnemy(this,condition.Type);
        while (enemyCount < condition.limit)
        {
            Instantiate<Damagable>(EnemieList[condition.Type],GetSpawnPosition(),Quaternion.identity);
            enemyCount++;
            yield return new WaitForSeconds(condition.interval);
        }

    }


    IEnumerator SpawnEnemies(float interval)
    {
        while (true)
        {
            Instantiate(_enemyPrefab, GetSpawnPosition(), Quaternion.identity,transform);
            yield return new WaitForSeconds(interval);
        }
    }

    private Vector2 GetSpawnPosition()
    {
        var freeTiles =  MapProvider.Instance.GetFreePositions();
        return freeTiles[Random.Range(0, freeTiles.Count)];
    }
}
