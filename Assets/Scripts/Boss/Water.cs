using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Skill2
{
    private Animator animator;
    bool checkActive;

    private void Awake()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        animator = GetComponent<Animator>();
    }

    public override void Update()
    {
        if (checkActive)
        {
            animator.SetTrigger("Active");
        }
    }

    public void FixedUpdate()
    {
        if (checkActive)
        {
            move();
        }
    }

    public void isActive()
    {
        checkActive = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDame();
            animator.SetTrigger("Break");
            checkActive = false;
        }
    }
}
