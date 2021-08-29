using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = new Vector3(0, 0, 0);

        float elapsedTime = 0f;

        while(elapsedTime < duration)
        {
            float xOffSet = Random.Range(-0.5f, 0.5f) * magnitude;
            float yOffSet = Random.Range(-0.5f, 0.5f) * magnitude;
            transform.localPosition = new Vector3(xOffSet, yOffSet, originalPos.z);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = originalPos;
    }
}
