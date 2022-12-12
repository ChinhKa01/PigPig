using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float speed = 20;
    public virtual void Update()
    {
        setActive();
        move();
    }

    public virtual void move()
    {
        transform.position -= transform.up * speed * Time.deltaTime;
    }

    public void setActive()
    {
        if (transform.position.x < SystemVariable.playerController.minCam.x - 1 || transform.position.x > SystemVariable.playerController.maxCam.x + 1
           || transform.position.y < SystemVariable.playerController.minCam.y - 1 || transform.position.y > SystemVariable.playerController.maxCam.y + 1)
        {
            gameObject.SetActive(false);
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Instantiate(SystemVariable.gameController.Effect[0], transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDame();
            Instantiate(SystemVariable.gameController.Effect[1], transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}
