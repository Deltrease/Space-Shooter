using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _alienEnemyPrefab;
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
    private bool _setKills = false;
    [SerializeField]
    private int _spawnCount = 0;
    [SerializeField]
    private int _waves = 0;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject _boss;
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
                case 1:
                    if (_setKills == false)
                    {
                        _kills = 8;
                        _setKills = true;
                    }
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
                            _setKills = false;
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.5f);
                        }
                    }
                    break;
                case 2:
                    if (_setKills == false)
                    {
                        _kills = 12;
                        _setKills = true;
                    }
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
                            _setKills = false;
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.5f);
                        }
                    }
                    break;
                case 3:
                    if (_setKills == false)
                    {
                        _kills = 16;
                        _setKills = true;
                    }
                    yield return new WaitForSeconds(3);
                    while (_stopSpawning == false && _spawnCount < _kills)
                    {
                        float randomSeconds = Random.Range(3, 5);
                        float randomX = Random.Range(9, -10);
                        float randomSpawn = Random.Range(1, 3);
                        Vector3 posSpawn = transform.position + new Vector3(randomX, 7.3f, 0);
                        yield return new WaitForSeconds(randomSeconds);
                        _spawnCount++;
                        if (randomSpawn == 1)
                        {
                            GameObject newEnemy = Instantiate(_enemyPrefab, posSpawn, Quaternion.identity);
                            newEnemy.transform.parent = _enemyContainer.transform;
                        }
                        else if (randomSpawn == 2)
                        {
                            GameObject newEnemy = Instantiate(_alienEnemyPrefab, posSpawn, Quaternion.identity);
                        }
                    }
                    while (_canSpawn == false)
                    {
                        if (_kills == 0)
                        {
                            Waves();
                            _canSpawn = true;
                            _setKills = false;
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.5f);
                        }
                    }
                    break;
                case 4:
                    if (_setKills == false)
                    {
                        _kills = 20;
                        _setKills = true;
                    }
                    yield return new WaitForSeconds(3);
                    while (_stopSpawning == false && _spawnCount < _kills)
                    {
                        float randomSeconds = Random.Range(3, 5);
                        float randomX = Random.Range(9, -10);
                        float randomSpawn = Random.Range(1, 3);
                        Vector3 posSpawn = transform.position + new Vector3(randomX, 7.3f, 0);
                        yield return new WaitForSeconds(randomSeconds);
                        _spawnCount++;
                        if (randomSpawn == 1)
                        {
                            GameObject newEnemy = Instantiate(_enemyPrefab, posSpawn, Quaternion.identity);
                            newEnemy.transform.parent = _enemyContainer.transform;
                        }
                        else if (randomSpawn == 2)
                        {
                            GameObject newEnemy = Instantiate(_alienEnemyPrefab, posSpawn, Quaternion.identity);
                            newEnemy.transform.parent = _enemyContainer.transform;
                        }
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
                    if (_setKills == false)
                    {
                        _kills = 1;
                        _setKills = true;
                    }
                    while (_stopSpawning == false && _spawnCount < _kills)
                    {
                        _spawnCount++;
                        Vector3 posSpawn = transform.position + new Vector3(0, 10, 0);
                        GameObject newBoss = Instantiate(_boss, posSpawn, Quaternion.identity);
                        newBoss.transform.parent = _enemyContainer.transform;
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
                    case 6:
                    _UI.WinningScreen();
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
        if (_waves == 6)
        {
            _stopSpawning = true;
        }
        else
        {
            StartCoroutine(_UI.FadeTextToFullAlpha(_waves));
        }
    }
    
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}

