using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private int _speed = 3;
    [SerializeField] // 0 = TriShot 1 = Speed 2 = Shields
    private int powerupID;
    [SerializeField]
    private AudioClip _powerUpClip;
    private bool _isMagnetised = false;
    private int _magnetSpeed = 5;
    private GameObject _player;

    Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        movement();
    }

    void movement()
    {
        if (_isMagnetised == true)
        {
            PowerupMagnetMove();
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            if (transform.position.y <= -7f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_powerUpClip, transform.position);
            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleShot();
                        break;
                    case 1:
                        player.SpeedUp();
                        break;
                    case 2:
                        player.ShieldsOn();
                        break;
                    case 3:
                        player.AmmoGet();
                        break;
                    case 4:
                        player.HealthGet();
                        break;
                    case 5:
                        player.HomingShot();
                        break;
                    case 6:
                        player.InverseControls();
                        break;
                }
            }
            Destroy(gameObject);
        }
        else if (other.tag == "Enemy Laser")
        {
            Destroy(gameObject);
        }
        else if (other.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
    public void Magnetise()
    {
        _isMagnetised = true;
    }
    public void PowerupMagnetMove()
    {
        playerPos = _player.transform.position;
        Vector3 direction = transform.position - playerPos;
        direction = -direction.normalized;
        transform.position += direction * _magnetSpeed * Time.deltaTime;
    }
}
