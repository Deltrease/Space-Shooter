using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_manager : MonoBehaviour
{
    //handle to Text
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _startText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _reloadText;
    [SerializeField]
    private Text _homingShotText;
    [SerializeField]
    private Text _waveText;
    [SerializeField]
    private Text _warningText;
    [SerializeField]
    private Text _winningText;

    [SerializeField]
    private float _score = 0;

    [SerializeField]
    private bool _normalShot = true;
    [SerializeField]
    private bool _tripleShot = false;
    [SerializeField]
    private bool _ammoType = true;

    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Image _BossHealth;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Slider _thrusterSlider;

    [SerializeField]
    private Game_Manager _gameManager;

    private bool _loading = true;
    private bool _timerOn = false;
    private float _timeLeft = 5.0f;
    private float _fadeTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        _ammoType = _normalShot;
        _scoreText.text = "Score: " + _score;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();

    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = "Score: " + _score;

        if (_timerOn == true)
        {
            HomingShot();
            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0)
            {
                _homingShotText.enabled = false;
                _ammoText.enabled = true;
            }
        }
    }

    public void ScoreUp(int points)
    {
        _score += points;
    }

    public void UpdateLives(int currentLives)
    {
        //display image sprite
        //give it a new one bosed on the currentLives index
        _livesImage.sprite = _liveSprites[currentLives];
        if (currentLives <= 0)
        {
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        _restartText.enabled = true;
        while (true)
        {
            _gameOverText.enabled = true;
            yield return new WaitForSeconds(0.5f);
            _gameOverText.enabled = false;
            yield return new WaitForSeconds(0.5f);

        }
    }

     public IEnumerator BossLoadingText()
    {
        while (_loading == true)
        {
            _warningText.enabled = true;
            yield return new WaitForSeconds(0.5f);
            _warningText.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void BossHealthText(int _health)
    {
        if (_BossHealth.enabled == false)
        {
            _BossHealth.enabled = true;
            _loading = false;
        }
        _BossHealth.fillAmount = Mathf.Clamp((_health / 75f), 0, 1f);
    }

    public void StartGame()
    {
        _startText.enabled = false;
        _ammoText.enabled = true;
    }

    public void Ammo(int _currentAmmo)
    {
        if (_ammoType == _normalShot)
        {
            if (_currentAmmo == 0)
            {
                _ammoText.enabled = false;
                _reloadText.enabled = true;
            }
            else if (_currentAmmo > 0)
            {
                _reloadText.enabled = false;
                _ammoText.enabled = true;
                _ammoText.text = "Ammo " + _currentAmmo + "/15";
            }
        }
        else if (_ammoType == _tripleShot)
        {
            _ammoText.text = "Triple Shot Ammo " + _currentAmmo + "/10";
        }
    }

    public void NormalShot(int _currentAmmo)
    {
        _homingShotText.enabled = false;
        _ammoText.enabled = true;
        _ammoType = _normalShot;
        _ammoText.text = "Ammo " + _currentAmmo + "/15";
    }

    public void TripleShot(int _currentAmmo)
    {
        _homingShotText.enabled = false;
        _ammoText.enabled = true;
        _ammoType = _tripleShot;
        _ammoText.text = "Triple Shot Ammo " + _currentAmmo + "/10";
    }

    public void HomingShot()
    {
        _ammoText.enabled = false;
        _homingShotText.enabled = true;
        _homingShotText.text = "Homing Shot Remaining: " + _timeLeft;
    }

    public void Timer()
    {
        _timerOn = true;
    }

    public void TimerOff()
    {
        _timerOn = false;
        _timeLeft = 5.0f;
    }

    public void UpdateThrusterFuel(float value)
    {
        _thrusterSlider.value = value;
    }
    public IEnumerator FadeTextToFullAlpha(int _waveNumber)
    {
        _waveText.enabled = true;
        _waveText.text = "Wave: " + _waveNumber;
        float t = 2f;
        _waveText.color = new Color(_waveText.color.r, _waveText.color.g, _waveText.color.b, 0);
        while (_waveText.color.a < 1.0f)
        {
            _waveText.color = new Color(_waveText.color.r, _waveText.color.g, _waveText.color.b, _waveText.color.a + (Time.deltaTime / t));
            yield return null;
        }
        StartCoroutine(FadeTextToZeroAlpha(_waveNumber));
    }

    public IEnumerator FadeTextToZeroAlpha(int _waveNumber)
    {
        float t = 2f;
        _waveText.text = "Wave: " + _waveNumber;
        _waveText.color = new Color(_waveText.color.r, _waveText.color.g, _waveText.color.b, 1);
        while (_waveText.color.a > 0.0f)
        {
            _waveText.color = new Color(_waveText.color.r, _waveText.color.g, _waveText.color.b, _waveText.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
    public void WinningScreen()
    {
        _winningText.enabled = true;
        _gameManager.GameOver();
    }
}
