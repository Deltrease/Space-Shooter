using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontDetection : MonoBehaviour
{
    private Enemy _enemy;
    // Start is called before the first frame update
    void Start()
    {
        _enemy = transform.parent.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Powerup")
        {
            StartCoroutine(_enemy.PowerupDestroy());
        }
    }
}
