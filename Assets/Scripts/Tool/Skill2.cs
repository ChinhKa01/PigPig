using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2 : Fire
{
    public Transform AttackPoint;

    public void Start()
    {
        AttackPoint = GameObject.FindGameObjectWithTag("AttackPointOfBoss").transform;
    }

    public override void move()
    {
        GetComponent<Rigidbody2D>().velocity = AttackPoint.right * speed;
    }
}
