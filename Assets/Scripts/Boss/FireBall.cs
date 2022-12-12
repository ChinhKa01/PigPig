using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    Vector3 posOld;
    public GameObject Ins;
    public Transform ShootPos;
    public float angle;
    public int quantityIns;
    public Queue<GameObject> stack = new Queue<GameObject>();
    public GameObject pooling;
    public List<GameObject> listIns;

    private void Start()
    {
        for (int i = 0; i < quantityIns; i++)
        {
            GameObject obj = Instantiate(Ins, transform.position, Quaternion.identity);
            obj.SetActive(false);
            obj.transform.parent = pooling.transform;
            listIns.Add(obj);
        }
        posOld = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (SystemVariable.bossController.wave2)
        {
            transform.position += new Vector3(0, 5 * Time.deltaTime, 0);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, posOld.y, posOld.y + 5), 0);
            if (transform.position.y >= posOld.y + 5)
            {
                transform.Rotate(new Vector3(0, 0, angle));
                if (stack.Count == 0 && SystemVariable.bossController.isAttack)
                {
                    wave2();
                }
            }
        }
    }

    public void wave2()
    {
        foreach (GameObject obj in listIns)
        {
            stack.Enqueue(obj);
        }
        StartCoroutine(shoot());
    }

    IEnumerator shoot()
    {
        while (stack.Count != 0)
        {
            Debug.Log("stack:" + stack.Count);
            GameObject obj = stack.Dequeue();
            obj.transform.position = ShootPos.position;
            obj.transform.rotation = ShootPos.rotation;
            obj.SetActive(true);
            if (stack.Count != 0)
            {
                yield return null;
            }
            else
            {
                transform.position = posOld;
                gameObject.SetActive(false);
                SystemVariable.bossController.wave2 = false;
                SystemVariable.bossController.Attacked();
                Debug.Log("endWave");
                yield return null;
            }
        }
    }
}
