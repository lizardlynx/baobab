using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float hp = 100f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float speed = 5f;

    private bool isGrounded = true;
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private GroundCheck groundCheck;

    public enum HeroStates
    {
        Idle,
        Run,
        Jump,
        JumpFall
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
        if (isGrounded) State = HeroStates.Idle;
        if (Input.GetButton("Horizontal")) Run();
        if (isGrounded && Input.GetButtonDown("Jump")) Jump();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }
    private HeroStates State
    {
        get { return (HeroStates)anim.GetInteger("AnimState"); }
        set { anim.SetInteger("AnimState", (int)value); }
    }
    private bool isFalling()
    {
        return rb.velocity.y < 0 ? true : false;
    }
    private void Run()
    {
        if (isGrounded) State = HeroStates.Run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        sprite.flipX = dir.x < 0.0f;
    }
    private void Jump()
    {
        State = HeroStates.Jump;
        rb.velocity = Vector2.up * jumpForce;
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
            State = isFalling() ? HeroStates.JumpFall : HeroStates.Jump;
        }
    }
}

