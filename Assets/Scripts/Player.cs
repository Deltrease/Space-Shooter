using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //use [SerializeField] for allowing a private value to be changed in the inspector view
    [SerializeField]
    private float _speed = 5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1.0f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _ammoCount = 15;
    [SerializeField]
    private int _tripleShotAmmo = 10;

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
    [SerializeField]
    private int _shieldHealth = 3;
    [SerializeField]
    private bool _reloading = false;

    private SpawnManager _spawnManager;
    private SpriteRenderer _blueShield;
    private SpriteRenderer _greenShield;
    private SpriteRenderer _redShield;
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
        _blueShield = GameObject.Find("BlueShield").GetComponent<SpriteRenderer>();
        _greenShield = GameObject.Find("GreenShield").GetComponent<SpriteRenderer>();
        _redShield = GameObject.Find("RedShield").GetComponent<SpriteRenderer>();
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

        if (transform.position.y >= 2.5)
        {
            transform.position = new Vector3(transform.position.x, 2.5f, 0);
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed += 5f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed -= 5f;
        }
    }
    void FireLaser()
    {

        _canFire = Time.time + _fireRate;
        //spawn gameobject
        if (_TripleShotActive == false)
        {
            if (_ammoCount == 0)
            {
                StartCoroutine(Reloading());
            }
            else if (_ammoCount > 0)
            {
                _ammoCount--;
                Vector3 gap = transform.position + new Vector3(0, 1.5f, 0);
                Instantiate(_laserPrefab, gap, Quaternion.identity);
                _playerAudio.Play();
                _UI.Ammo(_ammoCount);
            }
        }

        else if (_TripleShotActive == true)
        {
            if (_reloading == true)
            {
                return;
            }
            else if (_reloading == false)
            {
                _tripleShotAmmo--;
                Instantiate(_TripleShotPrefab, transform.position, Quaternion.identity);
                _playerAudio.Play();
                _UI.Ammo(_tripleShotAmmo);
            }
        }   
    }

    public IEnumerator Reloading()
    {
        if (_reloading == false)
        {
            _reloading = true;
            yield return new WaitForSeconds(1);
        }
        if(_reloading == true )
        {
            _reloading = false;
            _ammoCount = 15;
            _UI.Ammo(_ammoCount);
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
            CheckShields();
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
        _tripleShotAmmo = 10;
        _TripleShotActive = true;
        _UI.TripleShot(_tripleShotAmmo);
        StartCoroutine(TripleShotAmmoCheck());
    }

    public IEnumerator TripleShotAmmoCheck()
    {
        while (_TripleShotActive == true)
        {
            if (_tripleShotAmmo == 0)
            {
                _TripleShotActive = false;
                _UI.NormalShot(_ammoCount);
            }
            else
            {
                yield return null;
            }
        }
    }

    public void AmmoGet()
    {
        if (_TripleShotActive == false)
        {
            _ammoCount += 5;
        }
        else if (_TripleShotActive == true)
        {
            _tripleShotAmmo += 5;
        }    
    }

    public void SpeedUp()
    {
        _SpeedUpActive = true;
        _speed += 5f;
        StartCoroutine(SpeedUpPowerDown());
    }

    public IEnumerator SpeedUpPowerDown()
    {
        {
            yield return new WaitForSeconds(5);
            _SpeedUpActive = false;
            _speed -= 5f;
        }
    }

    public void ShieldsOn()
    {
        _shieldHealth = 3;
        _ShieldsActive = true;
        _greenShield.enabled = true;
        _redShield.enabled = false;
        _blueShield.enabled = false;
    }

    public void CheckShields()
    {
        _shieldHealth--;
        StartCoroutine(ShieldFlicker());
    }

    public IEnumerator ShieldFlicker()
    {
        if(_shieldHealth == 2)
        {
            _greenShield.enabled = false;
            _blueShield.enabled = true;
        }
        else if(_shieldHealth == 1)
        {
            _blueShield.enabled = false;
            _redShield.enabled = true;
        }
        if(_shieldHealth == 0)
        {
            _redShield.enabled = false;
            DeactivateShields();
            yield return null;
        }
    }
    public void DeactivateShields()
    {
        _ShieldsActive = false;
        _redShield.enabled = false;
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