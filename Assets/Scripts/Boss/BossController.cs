using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class BossController : MonoBehaviour
{
    public Rigidbody2D rid;
    public GameObject poolingSkill1, poolingSkill2;
    public GameObject Player;
    public GameObject AttackCheckPoint;
    public GameObject _Skill1, _Skill2;
    public List<GameObject> skin1List, skin2List;
    public GameObject FireBall, MagicCircle;
    public GameObject ItemDrop, Cam;
    private Queue<GameObject> queueStack;
    public int skillUsed, quantityIns;
    public int HP;
    public float speed, xScale, yScale, distanceStop, timeToAttack, countDown, radius;
    public string Dir;
    public bool enableMove, isAttack, wave2;
    public Animator animator;
    public LayerMask PlayerLM;
    public Slider HealthBar;
    public HealthBar healthBarScript;

    public virtual void Start()
    {
        for (int i = 0; i < quantityIns; i++)
        {
            //skill1
            GameObject obj = Instantiate(_Skill1, transform.position, Quaternion.identity);
            obj.SetActive(false);
            obj.transform.parent = poolingSkill1.transform;
            skin1List.Add(obj);

            //skill2
            GameObject obj2 = Instantiate(_Skill2, transform.position, Quaternion.identity);
            obj2.SetActive(false);
            obj2.transform.parent = poolingSkill2.transform;
            skin2List.Add(obj2);
        }

        if (MagicCircle != null)
        {
            MagicCircle.transform.position = AttackCheckPoint.transform.position;
            MagicCircle.SetActive(false);
            FireBall.SetActive(false);
        }
        isAttack = false;
        wave2 = false;
        enableMove = true;
        SystemVariable.bossController = this;
        xScale = transform.localScale.x;
        yScale = transform.localScale.y;
        countDown = 0;
        healthBarScript.SetMaxHealth(HP);
        queueStack = new Queue<GameObject>();
    }

    void Update()
    {
        if (SystemVariable.gameController._state == stateOfGame.Play.ToString() && HP > 0)
        {
            if (isAttack == false)
            {
                Rotate();
            }

            if (Vector2.Distance(transform.position, Player.transform.position) > distanceStop && isAttack == false)
            {
                Move();
            }
            else
            {
                AttackHandle();
            }
        }
        else if (HP <= 0)
        {
            Death();
        }
    }

    public virtual void AttackHandle()
    {
        animator.SetBool(stateOfBoss.Walk.ToString(), false);

        if (queueStack.Count == 0 && isAttack == false)
        {
            if (countDown <= 0)
            {
                int x = Random.Range(0, 6);
                while (skillUsed == x)
                {
                    x = Random.Range(0, 6);
                }
                switch (x)
                {
                    case 1:
                        Skill1();
                        break;
                    case 2:
                        Skill2();
                        break;
                    default:
                        SkillDefault();
                        break;
                }
                /*  else if (x == 4)
                  {
                      StartCoroutine(UseSkill3());
                  }*/
                isAttack = true;
                countDown = timeToAttack;
                skillUsed = x;
            }
            else
            {
                countDown -= Time.deltaTime;
            }
        }
    }

    public void Move()
    {
        animator.SetBool(stateOfBoss.Walk.ToString(), true);
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(Player.transform.position.x, transform.position.y), speed * Time.deltaTime);
    }

    public virtual void SkillDefault()
    {
        if (MagicCircle != null)
        {
            MagicCircle.SetActive(false);
            FireBall.SetActive(false);
        }
        animator.SetTrigger(stateOfBoss.Attack.ToString());
    }

    public virtual void Skill1()
    {
        queueStack.Clear();
        foreach (GameObject obj in skin1List)
        {
            queueStack.Enqueue(obj);
        }

        animator.SetTrigger(stateOfBoss.UseSkill1.ToString());
        FireBall.SetActive(true);
        StartCoroutine(UseSkill1());
    }

    public virtual void Skill2()
    {
        queueStack.Clear();
        foreach (GameObject obj in skin2List)
        {
            queueStack.Enqueue(obj);
        }

        animator.SetTrigger(stateOfBoss.UseSkill2.ToString());
        MagicCircle.SetActive(true);
        StartCoroutine(UseSkill2());
    }

    private void Rotate()
    {
        if (transform.position.x > Player.transform.position.x)
        {
            Dir = "Left";
            transform.localScale = new Vector2(xScale, yScale);
            HealthBar.direction = Slider.Direction.LeftToRight;
            AttackCheckPoint.transform.localRotation = Quaternion.Euler(new Vector3(0, -180, 0));
        }
        else if (transform.position.x < Player.transform.position.x)
        {
            Dir = "Right";
            transform.localScale = new Vector2(-xScale, yScale);
            HealthBar.direction = Slider.Direction.RightToLeft;
            AttackCheckPoint.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }

    public void check()
    {
        bool check = Physics2D.OverlapCircle(AttackCheckPoint.transform.position, radius, PlayerLM);
        if (check)
        {
            Player.GetComponent<PlayerController>().TakeDame();
            if (Dir == "Left")
            {
                Player.transform.position = new Vector3(Player.transform.position.x - 1, Player.transform.position.y + 1, 0);
            }
            else
            {
                Player.transform.position = new Vector3(Player.transform.position.x + 1, Player.transform.position.y + 1, 0);
            }
        }
    }

    public void BeAttacked() => animator.SetTrigger(stateOfBoss.BeAttacked.ToString());

    public virtual void Death()
    {
        animator.SetBool(stateOfBoss.Death.ToString(), true);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        if (FireBall != null || MagicCircle != null)
        {
            FireBall.SetActive(false);
            MagicCircle.SetActive(false);
        }
        this.enabled = false;
    }

    public void DropItem()
    {
        Instantiate(ItemDrop, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        this.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Player.transform.position);
        Gizmos.DrawWireSphere(AttackCheckPoint.transform.position, radius);
    }


    public void Attacked() => isAttack = false;


    public void TakeDame(int damage)
    {
        Vector2 pos = new Vector2(transform.position.x, Player.transform.position.y);
        Instantiate(SystemVariable.gameController.EffectOfPlayer[1], pos, Quaternion.identity);
        HP -= damage;
        healthBarScript.SetCurrentHealth(HP);
        BeAttacked();
    }

    IEnumerator UseSkill1()
    {
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        while (queueStack.Count != 0 && HP > 0)
        {
            GameObject obj = queueStack.Dequeue();
            Vector3 random = new Vector3(Random.Range(Player.transform.position.x - 1, Player.transform.position.x + 1), max.y + 1);
            obj.transform.position = random;
            obj.SetActive(true);
            if (queueStack.Count != 0)
            {
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                wave2 = true;
                yield return null;
            }
        }
    }

    IEnumerator UseSkill2()
    {
        while (queueStack.Count != 0 && HP > 0)
        {
            GameObject obj = queueStack.Dequeue();
            Vector2 random = new Vector2(AttackCheckPoint.transform.position.x, Random.Range(AttackCheckPoint.transform.position.y - 0.5f, AttackCheckPoint.transform.position.y + 0.5f));
            obj.transform.position = random;
            if (AttackCheckPoint.transform.rotation.y == 0)
            {
                obj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
            }
            else
            {
                obj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));

            }
            obj.SetActive(true);
            if (queueStack.Count != 0)
            {
                yield return new WaitForSeconds(1f);
            }
            else
            {
                Attacked();
                MagicCircle.SetActive(false);
                yield return null;
            }
        }
    }

    /*  IEnumerator UseSkill3()
      {
          Cam.GetComponent<CameraShake>().enable = true;
          yield return new WaitForSeconds(0.5f);
          Cam.GetComponent<CameraShake>().enable = false;
          yield return new WaitForSeconds(1.5f);
          Skill2();
      }*/

    public void DameLaze()
    {
        RaycastHit2D ob = Physics2D.Raycast(AttackCheckPoint.transform.position, AttackCheckPoint.transform.right);
        if (ob)
        {
            if (ob.transform.name == "Player")
            {
                Debug.Log("Die");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tag.Dart.ToString()))
        {
            BeAttacked();
        }
    }
}
