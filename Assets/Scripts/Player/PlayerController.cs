using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
[System.Serializable]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed, _forceJump, _forceShoot;
    [SerializeField] private float _radiusOfGroundCheckPoint, _radiusOfAttackPoint;
    [SerializeField] private Transform groundCheckPoint, attackPoint;
    [SerializeField] private GameObject jumpEffect, teleportEffect, runEffect;
    public bool _isGround, _isJumping, _isTelePortting, _isPlatform;
    private float _xDir, _yDir;
    private Rigidbody2D rid;
    private Animator animator;
    private float _scaleX, _scaleY;
    public GameObject[] listHeart;
    public int _damage, poolingAmount;
    public LayerMask Ground, Enemy, Platform;
    public GameObject damagePopups;
    public GameObject[] pooling;
    /* public Transform ene;*/
    private Queue<GameObject> stack;
    public GameObject dartObj, dartPool;
    public List<GameObject> listIns;
    public Vector2 minCam, maxCam, posStart;

    private void Awake()
    {
        SystemVariable.playerController = this;
        animator = GetComponent<Animator>();
        rid = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        minCam = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        maxCam = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        posStart = transform.position;

        _isPlatform = false;
        _isJumping = false;
        _isTelePortting = false;
        _scaleX = transform.localScale.x;
        _scaleY = transform.localScale.y;
        animator.SetTrigger(stateOfPlayer.Appear.ToString());
        runEffect.SetActive(false);
        SystemVariable.quantityCurrentHeart = listHeart.Length;
        stack = new Queue<GameObject>();
        for (int i = 0; i < poolingAmount; i++)
        {
            GameObject obj = Instantiate(dartObj, transform.position, Quaternion.identity);
            obj.SetActive(false);
            obj.transform.parent = dartPool.transform;
            listIns.Add(obj);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        _xDir = Input.GetAxisRaw("Horizontal");

        if (SystemVariable.gameController._state != stateOfGame.GameOver.ToString())
        {
            ButtonEventHandle();
            CheckCircle();
            Rotate();
            SpecialAnimationHandling();
            Attack();
        }
        Death();
    }

    private void FixedUpdate()
    {
        if (SystemVariable.gameController._state != stateOfGame.GameOver.ToString())
        {
            Movement();
            Jumping();
            Teleportting();
        }
    }

    //Hàm xử lý sự kiện nhập từ bàn phím
    private void ButtonEventHandle()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _isJumping = true;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _isTelePortting = true;
        }
    }

    //Hàm xử lý hành động di chuyển player trên trục x
    private void Movement()
    {
        rid.velocity = new Vector2(_xDir * _speed, rid.velocity.y);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCam.x, maxCam.x),
                                          Mathf.Clamp(transform.position.y, posStart.y, maxCam.y),
                                          0);

        if (_xDir != 0 && _isGround)
        {
            runEffect.SetActive(true);
        }
        else
        {
            runEffect.SetActive(false);
        }
    }

    //Hàm xử lý hành động nhảy của Player
    private void Jumping()
    {
        if (_isJumping && _isGround)
        {
            Instantiate(jumpEffect, transform.position, Quaternion.identity);
            rid.AddForce(Vector2.up * _forceJump, ForceMode2D.Impulse);
            _isJumping = false;
        }
    }

    //Hàm xử lý hoạt ảnh nhảy lên và lúc rơi xuống của Player
    private void SpecialAnimationHandling()
    {
        //Movement
        animator.SetFloat(stateOfPlayer.Run.ToString(), Mathf.Abs(_xDir));

        //Jumping
        if (_isGround == false)
        {
            if (rid.velocity.y > 0.01f)
            {
                animator.SetBool(stateOfPlayer.IsJumpingUp.ToString(), true);
            }
            else
            {
                animator.SetBool(stateOfPlayer.IsJumpingUp.ToString(), false);
                animator.SetBool(stateOfPlayer.IsJumpingDown.ToString(), true);
            }
        }
        else
        {
            animator.SetBool(stateOfPlayer.IsJumpingUp.ToString(), false);
            animator.SetBool(stateOfPlayer.IsJumpingDown.ToString(), false);
        }
    }

    //Hàm xử lý hành động xoay Player bằng phương pháp Scale
    private void Rotate()
    {
        if (_xDir < 0)
        {
            transform.localScale = new Vector2(-_scaleX, _scaleY);
            attackPoint.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else if (_xDir > 0)
        {
            transform.localScale = new Vector2(_scaleX, _scaleY);
            attackPoint.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }

    //Hàm xử lý hành động dịch chuyển tức thời của Player
    private void Teleportting()
    {
        if (_isTelePortting)
        {
            animator.SetTrigger(stateOfPlayer.Teleport.ToString());
            GameObject obj = Instantiate(teleportEffect, transform.position, Quaternion.identity);
            obj.transform.localScale = transform.localScale;
            rid.AddForce(Vector2.right * _forceJump * transform.localScale.x, ForceMode2D.Impulse);
            _isTelePortting = false;
        }
    }

    //Hàm xử lý hành động tấn công của Player
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.position, _radiusOfAttackPoint, Enemy);
            RandomDamage();
            if (enemy.Length > 0)
            {
                foreach (var obj in enemy)
                {
                    Instantiate(damagePopups, obj.transform.position, Quaternion.identity);
                    obj.GetComponent<BossController>().TakeDame(_damage);
                }
                animator.SetTrigger(stateOfPlayer.Attack.ToString());
            }
            else
            {
                animator.SetTrigger(stateOfPlayer.Attack2.ToString());
                GameObject obj = Instantiate(dartObj, attackPoint.position, Quaternion.identity);
                obj.GetComponent<Rigidbody2D>().AddForce(attackPoint.right * _forceShoot);
                if (stack.Count == 0)
                {
                    foreach (GameObject ob in listIns)
                    {
                        stack.Enqueue(ob);
                    }
                }
                GameObject dart = stack.Dequeue();
                dart.transform.position = attackPoint.position;
                dart.SetActive(true);
                dart.GetComponent<Rigidbody2D>().AddForce(attackPoint.right * _forceShoot);
            }
        }
    }

    public void TakeDame()
    {
        try
        {
            animator.SetTrigger(stateOfPlayer.Teleport.ToString());
            listHeart[SystemVariable.quantityCurrentHeart - 1].SetActive(false);
            SystemVariable.quantityCurrentHeart--;
        }
        catch (System.Exception ex)
        {
            Debug.Log("Lỗi" + ex);
        }
    }

    public void Death()
    {
        if (SystemVariable.gameController._state == stateOfGame.GameOver.ToString())
        {
            animator.SetTrigger(stateOfPlayer.Death.ToString());
            Debug.Log("Die");
        }
    }

    //Hàm kiểm tra xem Player có ở trên mặt đất hay không?
    private void CheckCircle()
    {
        _isGround = Physics2D.OverlapCircle(groundCheckPoint.position, _radiusOfGroundCheckPoint, Ground);
        _isPlatform = Physics2D.OverlapCircle(groundCheckPoint.position, _radiusOfGroundCheckPoint, Platform);
        if (_isPlatform)
        {
            _isGround = true;
        }
    }

    public void Disable() => this.enabled = false;

    public void ActiveDialogWithEvent() => SystemVariable.gameController.OpenDialog();

    public void RandomDamage()
    {
        _damage = Random.Range(10, 30);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tag.Rubi.ToString()))
        {
            collision.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheckPoint.position, _radiusOfGroundCheckPoint);
        Gizmos.DrawWireSphere(attackPoint.position, _radiusOfAttackPoint);
    }
}

