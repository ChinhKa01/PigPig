using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed, _forceJump;
    [SerializeField] private bool _isGround, _isJumping, _isTelePortting;
    [SerializeField] private float _radiusOfGroundCheckPoint, _radiusOfAttackPoint;
    [SerializeField] private Transform groundCheckPoint, attackPoint;
    [SerializeField] private GameObject jumpEffect, teleportEffect, runEffect;
    private float _xDir, _yDir;
    public LayerMask Ground, Enemy;
    private Rigidbody2D rid;
    //private SpriteRenderer render;
    private Animator animator;
    private float _scaleX, _scaleY;

    private void Awake()
    {
        //render = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rid = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _isJumping = false;
        _isTelePortting = false;
        _scaleX = transform.localScale.x;
        _scaleY = transform.localScale.y;
        animator.SetTrigger("Appear");
        runEffect.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        _xDir = Input.GetAxisRaw("Horizontal");
        //_yDir = Input.GetAxisRaw("Vertical");

        ButtonEventHandle();
        CheckGround();
        Rotate();
        SpecialAnimationHandling();
        Attack();
    }

    private void FixedUpdate()
    {
        Movement();
        Jumping();
        Teleportting();
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
        animator.SetFloat("Run", Mathf.Abs(_xDir));

        //Jumping
        if (_isGround == false)
        {
            if (rid.velocity.y > 0.01f)
            {
                animator.SetBool("IsJumpingUp", true);
            }
            else
            {
                animator.SetBool("IsJumpingUp", false);
                animator.SetBool("IsJumpingDown", true);
            }
        }
        else
        {
            animator.SetBool("IsJumpingUp", false);
            animator.SetBool("IsJumpingDown", false);
        }
    }

    //Hàm xử lý hành động xoay Player bằng phương pháp Scale
    private void Rotate()
    {
        if (_xDir < 0)
        {
            transform.localScale = new Vector2(-_scaleX, _scaleY);
        }
        else if (_xDir > 0)
        {
            transform.localScale = new Vector2(_scaleX, _scaleY);
        }
    }

    //Hàm xử lý hành động dịch chuyển tức thời của Player
    private void Teleportting()
    {
        if (_isTelePortting)
        {
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

            if (enemy.Length > 0)
            {
                enemy[0].gameObject.SetActive(false);
            }

            animator.SetTrigger("Attack");
        }
    }

    //Hàm kiểm tra xem Player có ở trên mặt đất hay không?
    private void CheckGround() => _isGround = Physics2D.OverlapCircle(groundCheckPoint.position, _radiusOfGroundCheckPoint, Ground);

    public void Disable() => this.enabled = false;
    public void Enable() => this.enabled = true;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheckPoint.position, _radiusOfGroundCheckPoint);
        Gizmos.DrawWireSphere(attackPoint.position, _radiusOfAttackPoint);
    }
}

