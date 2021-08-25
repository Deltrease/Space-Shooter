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
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _startText;

    // Start is called before the first frame update
    void Start()
    {
        
        //assign text component to the handle
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
    }
}
