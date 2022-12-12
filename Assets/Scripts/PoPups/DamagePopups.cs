using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopups : MonoBehaviour
{
    public Vector3 Offset;
    public Vector3 vectorRandom;
    private Vector3 posRandom;
    private int damage;
    private TextMesh textMesh;
   
    private void Start()
    {
        textMesh = GetComponent<TextMesh>();
        damage = FindObjectOfType<PlayerController>()._damage;
        textMesh.text = damage.ToString();
        posRandom = new Vector3(Random.Range(-vectorRandom.x, vectorRandom.x), Random.Range(-vectorRandom.y, vectorRandom.y),0);
        transform.localPosition += posRandom;
    }

    private void Update()
    {
        if(damage >= 25)
        {
            textMesh.color = Color.red;
            textMesh.fontSize = 30;
        }
        transform.position += new Vector3(0,Offset.y, 0);
    }
}
