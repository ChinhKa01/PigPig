using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazeBeam : MonoBehaviour
{
    [Range(0, 50)]
    public float speed;
    public GameObject[] list;

    private void Start()
    {
        foreach (GameObject obj in list)
        {
            obj.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(onOFF());
        transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
    }

    /*IEnumerator onOFF()
    {
        foreach (GameObject obj in list)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(2   f);
            obj.SetActive(false);
        }
    }*/
}
