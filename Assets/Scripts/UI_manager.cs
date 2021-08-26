using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_manager : MonoBehaviour
{
    //handle to Text
    [SerializeField]
    private Text _scoreText;
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
    private Sprite[] _liveSprites;
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

    // Start is called before the first frame update
    void Start()
    {
        _ammoType = _normalShot;
        _scoreText.text = "Score: " + _score;

    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = "Score: " + _score;
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
        if(currentLives <= 0)
        {
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        _restartText.enabled = true;
        while(true)
        {
            _gameOverText.enabled = true;
            yield return new WaitForSeconds(0.5f);
            _gameOverText.enabled = false;
            yield return new WaitForSeconds(0.5f);

        }
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
        else if(_ammoType == _tripleShot)
        {
            _ammoText.text = "Triple Shot Ammo " + _currentAmmo + "/10";
        }
    }

    public void NormalShot(int _currentAmmo)
    {
        _ammoType = _normalShot;
        _ammoText.text = "Ammo " + _currentAmmo + "/15";
    }  
    
    public void TripleShot(int _currentAmmo)
    {
        _ammoType = _tripleShot;
        _ammoText.text = "Triple Shot Ammo " +_currentAmmo + "/10";
    }
}
