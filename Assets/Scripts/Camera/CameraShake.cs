using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool enable;
    public float duration = 1f;
    // Start is called before the first frame update
    void Start()
    {
        enable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (enable)
        {
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking()
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = startPos + Random.insideUnitSphere;
            yield return null;
        }
        transform.position = startPos;
    }
}
