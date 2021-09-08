using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private GameObject _homingLaser;
    [SerializeField]
    private GameObject _laserWalls;
    [SerializeField]
    private GameObject _bigLaser;
    private GameObject _laserWallsClone;
    private GameObject _bigLaserClone;

    private UI_manager _UI;

    private Transform _target;
    private GameObject _player;
    private SpawnManager _spawnManager;
    private Vector3 _targetPosition;

    private float _speed = 1f;
    private float _timer = 10f;

    [SerializeField]
    private int _life = 75;
    private int _counter = 0;

    private bool _justSpawn = true;
    private bool _phaseOne = false;
    private bool _phaseTwo = false;
    private bool _phaseThree = false;
    private bool _alive = true;
    private bool _recharging = false;
    private bool _wallsSpawned = false;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _UI = GameObject.Find("Canvas").GetComponent<UI_manager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        InitialSpawn();
        StartCoroutine(Phases());
        StartCoroutine(_UI.BossLoadingText());

    }

    // Update is called once per frame
    void Update()
    {
        Timer();
        PhaseCheck();
        InitialSpawn();
    }

    public IEnumerator Phases()
    {
        while (_alive == true)
        {
            if (_justSpawn == true)
            {
                yield return new WaitForSeconds(0.01f);
                InitialSpawn();
            }
            if (_phaseOne == true)
            {
                if (_recharging == false)
                {
                    Instantiate(_homingLaser, transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(0.4f);
                }
                else if (_recharging == true)
                {
                    yield return new WaitForSeconds(_timer);
                }
            }
            else if(_phaseTwo == true)
            {
                if(_wallsSpawned == false)
                {
                    _laserWallsClone = Instantiate(_laserWalls, new Vector3(0, -15, 0), Quaternion.identity);
                }
                _wallsSpawned = true;
                if (_recharging == false)
                {
                    Instantiate(_homingLaser, transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(0.6f);
                }
                else if(_recharging == true)
                {
                    yield return new WaitForSeconds(_timer);
                }
            }
            else if(_phaseThree == true)
            {
                if (_counter == 0)
                {
                    _target = _player.transform;
                    _targetPosition = _target.position;
                    _counter++;
                    Destroy(_bigLaserClone);
                    yield return new WaitForSeconds(1f);
                }
                else if(_counter == 1)
                {
                    Vector3 _vectorToTarget = transform.position - _targetPosition;
                    float _angle = Mathf.Atan2(_vectorToTarget.y, _vectorToTarget.x) * Mathf.Rad2Deg;
                    _bigLaserClone = Instantiate(_bigLaser, _targetPosition, Quaternion.AngleAxis((_angle-90), Vector3.forward));
                    yield return new WaitForSeconds(2);
                    _counter--;
                }
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }
    }
    public void PhaseCheck()
    {
        if (_life <= 0)
        {
            _phaseThree = false;
            Destroy(_bigLaserClone);
            Destroy(_laserWallsClone);
            _spawnManager.Kills();
            Destroy(this.gameObject);
        }
        else if(_life >= 16 && _life <= 35)
        {
            _phaseOne = false;
            _phaseTwo = true;
        }
        else if(_life < 16)
        {
            _phaseTwo = false;
            _phaseThree = true;
        }
    }
    private void Timer()
    {
        if (_justSpawn == false)
        {
            if (_recharging == false)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    _recharging = true;
                    _timer = 5.0f;
                }
            }
            else if (_recharging == true)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    _recharging = false;
                    _timer = 10;
                }
            }
        }
    }
    public void InitialSpawn()
    {
        if (transform.position.y > 4)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        else if(transform.position.y <=4)
        {
            _UI.BossHealthText(_life);
            if (_justSpawn == true)
            {
                _phaseOne = true;
            }
            _justSpawn = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.position.y <= 4)
        {
            if (collision.tag == "Laser")
            {
                _life--;
                _UI.BossHealthText(_life);
                Destroy(collision.gameObject);
            }
        }
    }
}
