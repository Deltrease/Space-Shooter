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
    private bool _canSpawn = true;
    [SerializeField]
    private bool _gameStart = false;
    [SerializeField]
    private int _spawnCount = 0;
    [SerializeField]
    private int _waves = 0;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private int _kills = 0;
    private float _cd = 0.2f;
    [SerializeField]
    private bool _canGoDown = true;
    private UI_manager _UI;



    // Start is called before the first frame update
    void Start()
    {
        _UI = GameObject.Find("Canvas").GetComponent<UI_manager>();

    }

    public void StartSpawning()
    {
        Waves();
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerup());
        _gameStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_gameStart == true)
        {
            StartCoroutine(SpawnEnemyRoutine());
        }
    }

    public IEnumerator SpawnEnemyRoutine()
    {
        //Can we spawn
        if(_canSpawn == true)
        {
            _canSpawn = false;
            switch(_waves)
            {
                case 1: _kills = 10;
                    yield return new WaitForSeconds(3);
                    while(_stopSpawning == false && _spawnCount < _kills)
                    {
                        float randomSeconds = Random.Range(3, 5);
                        float randomX = Random.Range(9, -10);
                        yield return new WaitForSeconds(randomSeconds);
                        _spawnCount++;
                        GameObject newEnemy = Instantiate(_enemyPrefab, (transform.position + new Vector3(randomX, 7.3f, 0)), Quaternion.identity);
                        newEnemy.transform.parent = _enemyContainer.transform;
                    }
                    while (_canSpawn == false)
                    {
                        if (_kills == 0)
                        {
                            Waves();
                            _canSpawn = true;
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.5f);
                        }
                    }
                    break;
                case 2:
                    _kills = 15;
                    yield return new WaitForSeconds(3);
                    while (_stopSpawning == false && _spawnCount < _kills)
                    {
                        float randomSeconds = Random.Range(3, 5);
                        float randomX = Random.Range(9, -10);
                        yield return new WaitForSeconds(randomSeconds);
                        _spawnCount++;
                        GameObject newEnemy = Instantiate(_enemyPrefab, (transform.position + new Vector3(randomX, 7.3f, 0)), Quaternion.identity);
                        newEnemy.transform.parent = _enemyContainer.transform;
                    }
                    while (_canSpawn == false)
                    {
                        if (_kills == 0)
                        {
                            Waves();
                            _canSpawn = true;
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.5f);
                        }
                    }
                    break;
                case 3:
                    _kills = 20;
                    yield return new WaitForSeconds(3);
                    while (_stopSpawning == false && _spawnCount < _kills)
                    {
                        float randomSeconds = Random.Range(3, 5);
                        float randomX = Random.Range(9, -10);
                        yield return new WaitForSeconds(randomSeconds);
                        _spawnCount++;
                        GameObject newEnemy = Instantiate(_enemyPrefab, (transform.position + new Vector3(randomX, 7.3f, 0)), Quaternion.identity);
                        newEnemy.transform.parent = _enemyContainer.transform;
                    }
                    while (_canSpawn == false)
                    {
                        if (_kills == 0)
                        {
                            Waves();
                            _canSpawn = true;
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.5f);
                        }
                    }
                    break;
                case 4:
                    _kills = 25;
                    yield return new WaitForSeconds(3);
                    while (_stopSpawning == false && _spawnCount < _kills)
                    {
                        float randomSeconds = Random.Range(3, 5);
                        float randomX = Random.Range(9, -10);
                        yield return new WaitForSeconds(randomSeconds);
                        _spawnCount++;
                        GameObject newEnemy = Instantiate(_enemyPrefab, (transform.position + new Vector3(randomX, 7.3f, 0)), Quaternion.identity);
                        newEnemy.transform.parent = _enemyContainer.transform;
                    }
                    while (_canSpawn == false)
                    {
                        if (_kills == 0)
                        {
                            Waves();
                            _canSpawn = true;
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.5f);
                        }
                    }
                    break;
                case 5:
                    _kills = 25;
                    yield return new WaitForSeconds(3);
                    while (_stopSpawning == false && _spawnCount < _kills)
                    {
                        float randomSeconds = Random.Range(3, 5);
                        float randomX = Random.Range(9, -10);
                        yield return new WaitForSeconds(randomSeconds);
                        _spawnCount++;
                        GameObject newEnemy = Instantiate(_enemyPrefab, (transform.position + new Vector3(randomX, 7.3f, 0)), Quaternion.identity);
                        newEnemy.transform.parent = _enemyContainer.transform;
                    }
                    while (_canSpawn == false)
                    {
                        if (_kills == 0)
                        {
                            Waves();
                            _canSpawn = true;
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.5f);
                        }
                    }
                    break;
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
        _spawnCount--;
    }
    
    public void Kills()
    {
        _kills--;
    }

    public void Waves()
    {
        _waves++;
        StartCoroutine(_UI.FadeTextToFullAlpha(_waves));
    }
    
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}

