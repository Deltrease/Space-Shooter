using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;
    [SerializeField]
    private float _shotSpeed = 2f;
    [SerializeField]
    private int _shotType;
    private Transform _target;

    private GameObject[] _enemy;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Firing();
        StartCoroutine(HomingGone());
    }

    public void Firing()
    {
        _enemy = GameObject.FindGameObjectsWithTag("Enemy");

        if (_shotType == 0)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            if (transform.position.y >= 8)
            {
                Destroy(gameObject);
            }
        }
        else if (_shotType == 1)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);

            for (int i = 0; i < _enemy.Length; i++)
            {
                _target = _enemy[i].transform;
                transform.up = _target.position - transform.position;
            }

        }
        if (transform.position.y >= 8)
        {
            Destroy(gameObject);
        }
        else if(transform.position.y <= -8)
        {
            Destroy(gameObject);
        }    
        else if(transform.position.x >= 10)
        {
            Destroy(gameObject);
        }
        else if(transform.position.x <= -10)
        {
            Destroy(gameObject);
        }    
    }

    public IEnumerator HomingGone()
    {
        if(_shotType == 1)
        {
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }
    }
}
