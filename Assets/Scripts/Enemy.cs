using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private SpawnManager _spawnManager;
    private Player _player;
    private UI_manager _UI;
    public Animator _destruction;
    [SerializeField]
    private AudioSource _EnemyAudioSource;
    [SerializeField]
    private AudioClip _ExplosionClip;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private AudioClip _laserFire;
    [SerializeField]
    private bool _alive = true;

    [SerializeField]
    private float _timer = 0;
    private int _randomMovement = 0;
    [SerializeField]
    private int _movementType = 0;
    [SerializeField]
    private int _leftOrRight = 0;
    private bool _zigZag = false;
    private bool _left = false;
    private bool _right = false;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _UI = GameObject.Find("Canvas").GetComponent<UI_manager>();
        _destruction = GetComponent<Animator>();
        _EnemyAudioSource = GetComponent<AudioSource>();
        StartCoroutine(Firing());
        Randomizer();

    }

    // Update is called once per frame
    void Update()
    {
        Movement();


    }
    
    public void Randomizer()
    {
        if (_randomMovement == 0)
        {
            _randomMovement++;
            _movementType = Random.Range(0, 2);
            _leftOrRight = Random.Range(0, 2);
            if(_leftOrRight == 0)
            {
                _left = true;
            }
            else if (_leftOrRight == 1)
            {
                _right = true;
            }
        }
    }

    void Movement()
    {
        if (_movementType == 0)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y <= -5.3)
            {
                float randomX = Random.Range(-9, 10);
                transform.position = new Vector3(randomX, 7.3f, 0);
            }
        }
        else if(_movementType == 1)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            if (_left == true)
            {
                if (_timer <= 0)
                {
                    _timer = 1;
                    _left = false;
                    _right = true;
                }
                else if (_timer <= 1)
                {
                    _timer -= Time.deltaTime;
                    transform.Translate(Vector3.left * _speed * Time.deltaTime);
                    if (transform.position.y <= -5.3)
                    {
                        float randomX = Random.Range(-9, 10);
                        transform.position = new Vector3(randomX, 7.3f, 0);
                    }
                    else if (transform.position.x >= 11)
                    {
                        transform.position = new Vector3(-11f, transform.position.y, 0);
                    }
                    else if (transform.position.x <= -11)
                    {
                        transform.position = new Vector3(11f, transform.position.y, 0);
                    }
                }
            }
            else if (_right == true)
            {
                if(_timer <= 0)
                {
                    _timer = 1;
                    _right = false;
                    _left = true;
                }
                else if (_timer <= 1)
                {
                    _timer -= Time.deltaTime;
                    transform.Translate(Vector3.right * _speed * Time.deltaTime);
                    if (transform.position.y <= -5.3)
                    {
                        float randomX = Random.Range(-9, 10);
                        transform.position = new Vector3(randomX, 7.3f, 0);
                    }
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //if other is player
        //damage the player
        //destroy us
        //activate spawn count
        if (other.tag == "Player")
        {
            Player player= other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }


            if (_spawnManager != null)
            {
                _alive = false;
                _spawnManager.SpawnCounter();
            }
            _destruction.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _EnemyAudioSource.clip = _ExplosionClip;
            _EnemyAudioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
        }

        if (other.tag == "Laser")
        {

            if (_spawnManager != null)
            {
                _alive = false;
                _spawnManager.SpawnCounter();
            }
            _destruction.SetTrigger("OnEnemyDeath");
            _UI.ScoreUp(Random.Range(10,21));
            Destroy(other.gameObject);
            _speed = 0;
            _EnemyAudioSource.clip = _ExplosionClip;
            _EnemyAudioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
        }
    }

    private IEnumerator Firing()
    {
        yield return new WaitForSeconds(1);
        while (_alive == true)
        {
            float randomFire = Random.Range(4, 6);
            _EnemyAudioSource.clip = _laserFire;
            _EnemyAudioSource.Play();
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(randomFire);
        }
    }
}
