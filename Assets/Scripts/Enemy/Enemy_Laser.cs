using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;
    [SerializeField]
    private int _shotType;
    private int _counter = 0;

    private Transform _target;
    private GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
        StartCoroutine(HomingGone());
    }
    private void Moving()
    {
        if (_shotType == 0)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            if (transform.position.y <= -8)
            {
                Destroy(gameObject);
            }
        }
        else if (_shotType == 1)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            if (_counter == 0)
            {
                _counter++;
                _target = _player.transform;
                transform.up = _target.position - transform.position;
            }
        }
    }
    private IEnumerator HomingGone()
    {
        if(_shotType == 1)
        {
            yield return new WaitForSeconds(3f);
            Destroy(this.gameObject);
        }
        else
        {
            StopCoroutine(HomingGone());
        }
    }
}