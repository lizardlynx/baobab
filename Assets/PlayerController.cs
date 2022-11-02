using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : StatefulBehaviour
{
    [SerializeField] private float hp = 100f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float speed = 5f;

    private bool isGrounded = true;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private GroundCheck groundCheck;

    public enum States
    {
        Idle,
        Run,
        Jump,
        JumpFall,
        Attack
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        groundCheck = transform.Find("GroundCheck").GetComponent<GroundCheck>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void UpdateState()
    {
        if (GetState<States>() != States.Attack && isGrounded) SetState(States.Idle);
        if (Input.GetButton("Horizontal")) Run();
        if (isGrounded && Input.GetButtonDown("Jump")) Jump();
        if (Input.GetButtonDown("Fire1")) Attack();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    private void FixedUpdate()
    {
        CheckGround();
    }
    private void CheckGround()
    {
        isGrounded = groundCheck.isGrounded;
        if (!isGrounded && hp > 0)
        {
            SetState(isFalling() ? States.JumpFall : States.Jump);
        }
    }

    private bool isFalling()
    {
        return rb.velocity.y < 0 ? true : false;
    }
    private void Run()
    {
        if (isGrounded) SetState(States.Run);

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        sprite.flipX = dir.x < 0.0f;
    }
    private void Jump()
    {
        SetState(States.Jump);
        rb.velocity = Vector2.up * jumpForce;
    }

    private void Attack()
    {
        SetState(States.Attack);
        StartCoroutine(OnAnimationComplete("Attack", () => SetState(States.Idle)));
    }
    
}

