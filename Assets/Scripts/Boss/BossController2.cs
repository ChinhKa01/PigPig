using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController2 : BossController
{
    public GameObject laze;
    public GameObject water;
    private GameObject waterIns;
    public override void Start()
    {
        base.Start();
        laze.SetActive(false);
        waterIns = Instantiate(water, AttackCheckPoint.transform.position, Quaternion.identity);
        waterIns.SetActive(false);
    }

    public override void Skill2() => laze.SetActive(true);

    public override void Skill1() => StartCoroutine(UseSkin1());

    IEnumerator UseSkin1()
    {
        SkillDefault();
        waterIns.transform.position = AttackCheckPoint.transform.position;
        yield return new WaitForSeconds(0.5f);
        waterIns.SetActive(true);
        isAttack = false;
    }
    public void Death()
    {
        if (laze != null || waterIns != null)
        {
            laze.SetActive(false);
            water.SetActive(false);
        }
    }
}
