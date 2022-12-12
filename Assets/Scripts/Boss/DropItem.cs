using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public GameObject[] item;
    // public List<GameObject> listItemRandom = new List<GameObject>();
    private Queue<GameObject> stack;
    public float forceSplatter;
    public int amount;

    private void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            int x = Random.Range(0, item.Length + 1);
            Vector2 random = new Vector2(Random.Range(-forceSplatter, forceSplatter), Random.Range(-forceSplatter, forceSplatter));
            if (x == 1)
            {
                GameObject obj = Instantiate(item[0], transform.position, Quaternion.identity);
                //listItemRandom.Add(obj);
                obj.GetComponent<Rigidbody2D>().AddForce(random, ForceMode2D.Impulse);
            }
            else
            {
                GameObject obj = Instantiate(item[1], transform.position, Quaternion.identity);
                //listItemRandom.Add(obj);
                obj.GetComponent<Rigidbody2D>().AddForce(random, ForceMode2D.Impulse);
            }
            //Vector3 random = new Vector3(Random.Range(transform.position.x - 1, transform.position.x + 10),Random.Range(transform.position.y - forceSplatter, transform.position.y + forceSplatter), 0) * Time.deltaTime;
            //listItemRandom[i].GetComponent<Rigidbody2D>().velocity = random; 
        }
    }
}
