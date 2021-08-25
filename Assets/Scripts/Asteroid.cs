using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asteroid : MonoBehaviour
{
    private float _speed = 20.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;

    [SerializeField]
    private AudioSource _asteroidAudioSource;
    [SerializeField]
    private AudioClip _gameStartClip;

    private UI_manager _UI;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _asteroidAudioSource = GetComponent<AudioSource>();
        _UI = GameObject.Find("Canvas").GetComponent<UI_manager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            Destroy(other.gameObject);
            _asteroidAudioSource.clip = _gameStartClip;
            _asteroidAudioSource.Play();
            _UI.StartGame();
            Destroy(this.gameObject, 0.5f);
        }
    }
}
