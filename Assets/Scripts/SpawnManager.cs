using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _laser;
    [SerializeField]
    private bool _stopSpawning = false;
    [SerializeField]
    private int _spawnCount = 0;
    [SerializeField]
    private GameObject[] _powerups;



    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerup());
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        //while loop (infinite loop)
        while (_stopSpawning == false)
        {
            if (_spawnCount == 5)
            {
                yield return new WaitForSeconds(2);
            }
            else if (_spawnCount < 5)
            {
                float randomSeconds = Random.Range(3, 5);
                float randomX = Random.Range(9, -10);
                yield return new WaitForSeconds(randomSeconds);
                _spawnCount++;
                GameObject newEnemy = Instantiate(_enemyPrefab, (transform.position + new Vector3(randomX, 7.3f, 0)), Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
        }

    }

    public IEnumerator SpawnPowerup()
    {
        yield return new WaitForSeconds(3.0f);
        

        while(_stopSpawning == false)
        {
            float randomSeconds = Random.Range(3, 8);
            float randomX = Random.Range(9, -10);
            int randomPowerup = Random.Range(0, 20);
            Instantiate(_powerups[randomPowerup], (transform.position + new Vector3(randomX, 7.3f, 0)), Quaternion.identity);
            yield return new WaitForSeconds(randomSeconds);
        }
    }
    public void SpawnCounter()
    {
       _spawnCount --;

    }
    
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}

