using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //use [SerializeField] for allowing a private value to be changed in the inspector view
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1.0f;
    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private GameObject _TripleShotPrefab;
    [SerializeField]
    private bool _TripleShotActive = false;
    [SerializeField]
    private bool _SpeedUpActive = false;
    [SerializeField]
    private bool _ShieldsActive = false;
    [SerializeField]
    private SpriteRenderer[] _damageOne;
    [SerializeField]
    private SpriteRenderer[] _damageTwo;
    [SerializeField]
    private GameObject _enemyLaserObject;
    [SerializeField]
    private bool _canBeDamaged = true;

    private SpawnManager _spawnManager;
    private SpriteRenderer Shield;
    private UI_manager _UI;
    private Game_Manager _gameManager;
    private Animator _turning;

    //variable to store audio clip
    [SerializeField]
    private AudioSource _playerAudio;
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioClip _playerExplosionClip;


    // Start is called before the first frame update
    void Start()
    {
        //this grabs the current position, and spawns it at a new position (Vector3)
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _UI = GameObject.Find("Canvas").GetComponent<UI_manager>();
        Shield = GameObject.Find("Shield").GetComponent<SpriteRenderer>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();
        _playerAudio = GetComponent<AudioSource>();
        _playerAudio.clip = _laserSoundClip;
        _turning = GetComponent<Animator>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
        Turning();
    }

    //create new voids to organize your work
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);
        //Use if/elseif for making boundaries

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x >= 11.3)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }
    void FireLaser()
    {

        _canFire = Time.time + _fireRate;
        //spawn gameobject
        if (_TripleShotActive == false)
        {

            Vector3 gap = transform.position + new Vector3(0, 1.5f, 0);
            Instantiate(_laserPrefab, gap, Quaternion.identity);
            _playerAudio.Play();
        }

        else if (_TripleShotActive == true)
        {
            Instantiate(_TripleShotPrefab, transform.position, Quaternion.identity);
            _playerAudio.Play();
        }

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy Laser")
        {
            if (other != null)
            {
                Damage();
                Destroy(other.gameObject);
            }
        }
    }

    public void Damage()
    {
        if (_ShieldsActive == true)
        {
            DeactivateShields();
            return;
        }
        else if (_ShieldsActive == false)
        {
            _lives--;
            if (_lives == 2)
            {
                int randomSide = Random.Range(0, 2);
                _damageOne[randomSide].enabled = true;
                _UI.UpdateLives(_lives);
            }
            else if (_lives == 1)
            {
                int otherSide = Random.Range(0, 2);
                _damageTwo[otherSide].enabled = true;
                _UI.UpdateLives(_lives);
            }
            else if (_lives == 0)
            {
                _UI.UpdateLives(_lives);
            }
        }

        //random thruster on when damaged
        if (_lives < 1)
        {
            if (_spawnManager != null)
            {
                _playerAudio.clip = _playerExplosionClip;
                _playerAudio.Play();
                _spawnManager.OnPlayerDeath();
                PlayerGameOver();
            }
            Destroy(this.gameObject);
        }
    }

    public void TripleShot()
    {
        _TripleShotActive = true;
        StartCoroutine(TripleShotPowerDown());
    }

    public IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5);
        _TripleShotActive = false;
    }

    public void SpeedUp()
    {
        _SpeedUpActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedUpPowerDown());
    }

    public IEnumerator SpeedUpPowerDown()
    {
        yield return new WaitForSeconds(5);
        _SpeedUpActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldsOn()
    {
        _ShieldsActive = true;
        Shield.enabled = true;
    }
    public void DeactivateShields()
    {
        _ShieldsActive = false;
        Shield.enabled = false;
    }

    public void PlayerGameOver()
    {
        _gameManager.GameOver();
    }

    public void Turning()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            _turning.SetTrigger("OnHoldA");
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            _turning.SetTrigger("OnHoldD");
        }
        else if(Input.GetKeyUp(KeyCode.A))
        {
            _turning.Play("Default");
        }
        else if(Input.GetKeyUp(KeyCode.D))
        {
            _turning.Play("Default");
        }

    }
}
