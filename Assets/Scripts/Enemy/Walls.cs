using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{
    private bool _inPosition = false;
    private int _speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_inPosition == false)
        {
            Movement();
        }
    }
    public void Movement()
    {
        if (transform.position.y >= 0)
        {
            _inPosition = true;
        }
        else if (transform.position.y >= -16)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
    }
}
