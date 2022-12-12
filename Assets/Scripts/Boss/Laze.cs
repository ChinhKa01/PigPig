using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laze : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void endAttack()
    {
        SystemVariable.bossController.isAttack = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.SetActive(false);
    }

    public void dame() => gameObject.GetComponent<BoxCollider2D>().enabled = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDame();
        }
    }
}
