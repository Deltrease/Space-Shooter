using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //use [SerializeField] for allowing a private value to be changed in the inspector view
    [SerializeField]
    private float _speed = 5f;
    //private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _TripleShotPrefab;
    [SerializeField]
    private GameObject _homingShotPrefab;
    [SerializeField]
    private GameObject _tripleHomingShotPrefab;
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
    private bool _TripleShotActive = false;
    [SerializeField]
    private bool _SpeedUpActive = false;
    [SerializeField]
    private bool _ShieldsActive = false;
    [SerializeField]
    private bool _homingActive = false;
    [SerializeField]
    private SpriteRenderer _damageLeft;
    [SerializeField]
    private SpriteRenderer _damageRight;
    [SerializeField]
    private bool _canBeDamaged = true;
    [SerializeField]
    private int _shieldHealth = 3;
    [SerializeField]
    private bool _reloading = false;
    [SerializeField]
    private bool _inverseControlsActive = false;

    private float _timeRemaining = 5.0f;
    [SerializeField]
    private float _fuel = 100f;
    private bool _fuelCooldownActive = false;

    private bool _isMagnetCooldown = false;

    private SpawnManager _spawnManager;
    private SpriteRenderer _blueShield;
    private SpriteRenderer _greenShield;
    private SpriteRenderer _redShield;
    private UI_manager _UI;
    private Game_Manager _gameManager;
    private Animator _turning;
    private CameraShake _mainCamera;

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
        _mainCamera = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        
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
        _UI.UpdateThrusterFuel(_fuel);
        Thrusters();
        MagnetActivated();
    }

    //create new voids to organize your work
    void CalculateMovement()
    {
        if (_inverseControlsActive == false)
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

            if (Input.GetKeyDown(KeyCode.LeftShift) && _fuelCooldownActive == false)
            {
                _speed += 5f;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) && _fuelCooldownActive == false)
            {
                _speed -= 5f;
            }
        }
        else if (_inverseControlsActive == true)
        {
            float horizontalInput = Input.GetAxis("Horizontal2");
            float verticalInput = Input.GetAxis("Vertical2");
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

            if (Input.GetKeyDown(KeyCode.LeftShift) && _fuelCooldownActive == false)
            {
                _speed += 5f;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) && _fuelCooldownActive == false)
            {
                _speed -= 5f;
            }
        }
    }

    private void Thrusters()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_fuel > 0 && _fuelCooldownActive == false)
            {
                _fuel -= 15f * Time.deltaTime;
            }
            else
            {
                if (_speed == 10f)
                {
                    _speed -= 5f;
                }
                StartCoroutine(ThrusterCooldown());
            }
        }
        if (_speed == 0)
        {
            _speed = 5;
        }
    }

    private void FireLaser()
    {

        _canFire = Time.time + _fireRate;
        //spawn gameobject
        if (_TripleShotActive == false && _homingActive == false)
        {
            if (_ammoCount == 0)
            {
                StartCoroutine(Reloading());
            }
            else if (_ammoCount > 0)
            {
                _fireRate = 0.5f;
                _ammoCount--;
                Vector3 gap = transform.position + new Vector3(0, 1.5f, 0);
                Instantiate(_laserPrefab, gap, Quaternion.identity);
                _playerAudio.Play();
                _UI.Ammo(_ammoCount);
            }
        }
        else if (_TripleShotActive == true && _homingActive == true)
        {
            _fireRate = 1f;
            Instantiate(_tripleHomingShotPrefab, transform.position, Quaternion.identity);
            _playerAudio.Play();
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
        else if (_homingActive == true)
        {
            _fireRate = 1f;
            Instantiate(_homingShotPrefab, transform.position, Quaternion.identity);
            _playerAudio.Play();
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
        else if (other.tag == "BossLaser")
        {
            if(other != null)
            {
                Damage();
            }
        }
    }

    public void Damage()
    {
        StartCoroutine(_mainCamera.Shake(0.75f, 0.5f));
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
                _damageLeft.enabled = true;
                _UI.UpdateLives(_lives);
            }
            else if (_lives == 1)
            {
                int otherSide = Random.Range(0, 2);
                _damageRight.enabled = true;
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

    public void HealthGet()
    {
        if (_lives < 3)
        {
            _lives++;
            if (_lives == 3)
            {
                int randomSide = Random.Range(0, 2);
                _damageLeft.enabled = false;
                _UI.UpdateLives(_lives);
            }
            else if (_lives == 2)
            {
                int otherSide = Random.Range(0, 2);
                _damageRight.enabled = false;
                _UI.UpdateLives(_lives);
            }
        }
        else if(_lives == 3)
        {
            return;
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
            if(_ammoCount > 10)
            {
                switch(_ammoCount)
                {
                    case 11:
                        _ammoCount += 4;
                        _UI.Ammo(_ammoCount);
                        break;
                    case 12:
                        _ammoCount += 3;
                        _UI.Ammo(_ammoCount);
                        break;
                    case 13:
                        _ammoCount += 2;
                        _UI.Ammo(_ammoCount);
                        break;
                    case 14:
                        _ammoCount += 1;
                        _UI.Ammo(_ammoCount);
                        break;
                    case 15:
                        break;
                }

            }
            else if (_ammoCount <= 10)
            {
                _ammoCount += 5;
                _UI.Ammo(_ammoCount);
            }
        }
        else if (_TripleShotActive == true)
        {
            if(_tripleShotAmmo > 5)
            {
                switch(_tripleShotAmmo)
                {
                    case 6:
                        _tripleShotAmmo += 4;
                        _UI.Ammo(_tripleShotAmmo);
                        break;
                    case 7:
                        _tripleShotAmmo += 3;
                        _UI.Ammo(_tripleShotAmmo);
                        break;
                    case 8:
                        _tripleShotAmmo += 2;
                        _UI.Ammo(_tripleShotAmmo);
                        break;
                    case 9:
                        _tripleShotAmmo += 1;
                        _UI.Ammo(_tripleShotAmmo);
                        break;
                    case 10:
                        break;
                }
            }
            else if (_tripleShotAmmo <= 5)
            {
                _tripleShotAmmo += 5;
                _UI.Ammo(_tripleShotAmmo);
            }
        }
    }

    public void HomingShot()
    {
        _homingActive = true;
        StartCoroutine(HomingShotPowerDown());
        _UI.Timer();
    }

    public IEnumerator HomingShotPowerDown()
    {
        yield return new WaitForSeconds(5);
        _UI.TimerOff();
        _homingActive = false;
    }

    public void SpeedUp()
    {
        _SpeedUpActive = true;
        _speed += 5f;
        StartCoroutine(SpeedUpPowerDown());
    }

    public IEnumerator SpeedUpPowerDown()
    {
            yield return new WaitForSeconds(5);
            _SpeedUpActive = false;
            _speed -= 5f;
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

    public void InverseControls()
    {
        _inverseControlsActive = true;
        StartCoroutine(DeactivateInverseControls());
    }
    public IEnumerator DeactivateInverseControls()
    {
        yield return new WaitForSeconds(5);
        _inverseControlsActive = false;
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

    public IEnumerator ThrusterCooldown()
    {
        _fuelCooldownActive = true;
        yield return new WaitForSeconds(1.5f);
        while (_fuelCooldownActive == true)
        {
            _fuel += 5f * Time.deltaTime;
            if (_fuel >= 100.0f)
            {
                _fuelCooldownActive = false;
            }
            yield return new WaitForSeconds(15 * Time.deltaTime);
        }
    }

    private void MagnetActivated()
    {
        if(Input.GetKeyDown(KeyCode.C) && _isMagnetCooldown == false)
        {
            StartMagnet();
        }
        else if (Input.GetKeyDown(KeyCode.C) && _isMagnetCooldown == true)
        {
            Debug.Log("Magnet is cooling down");
        }
    }
    
    private void StartMagnet()
    {
        GameObject[] _powerUps = GameObject.FindGameObjectsWithTag("Powerup");

        if (_powerUps != null)
        {
            for(int i = 0; i < _powerUps.Length; i++)
            {
                Powerup power = _powerUps[i].GetComponent<Powerup>();
                power.Magnetise();
                StartCoroutine(MagnetCooldownRoutine(5f));
            }
        }

        if(_powerUps.Length == 0)
        {
            StartCoroutine(MagnetCooldownRoutine(5f));
        }
    }
    private IEnumerator MagnetCooldownRoutine(float time)
    {
        _isMagnetCooldown = true;
        yield return new WaitForSeconds(time);
        _isMagnetCooldown = false;
    }
}