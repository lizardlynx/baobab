using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class HeroScript : MonoBehaviour
{
    [SerializeField] private float speed;
    private float scale = 3;
    private Rigidbody2D body;
    private Animator anim;
    private Transform transform;
    private bool grounded;
    private bool died;
    public Transform attackPoint;
    private Collider2D attackArea;
    public float attackRange = 2f;
    public LayerMask enemyLayers;
    private int damageAmount = 10;
    private float attackRate = 1f;
    private float nextAttackTime = 0;
    private float maxHP = 30;
    private float currentHP = 30;
    private float cooling = 2f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        attackArea = transform.GetChild(0).GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector2.one*scale;
        }
        else if (horizontalInput < -0.01f){
            transform.localScale = new Vector2(-scale, scale);
        }

        if (Input.GetKey(KeyCode.Space) && grounded)
            Jump();

        if (Input.GetKeyDown(KeyCode.S) && grounded && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }

        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
        anim.SetBool("died", died);
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        grounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.contacts[0]);
        if (collision.gameObject.tag == "Ground")
            grounded = true;
    }

    private void Attack()
    {
        Debug.Log("Playing Attack Animation");
        anim.SetTrigger("Attack");

        Collider2D[] hitEnemies = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(enemyLayers);
        Physics2D.OverlapCollider(attackArea, filter, hitEnemies);

        Debug.Log(hitEnemies.Length);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy != null) enemy.GetComponent<SmallMonster5000>().TakeDamage(damageAmount);
        }

        nextAttackTime = cooling;
    }

    public void TakeDamage(float damageAmount)
    {
        Debug.Log("Hero hit! " + currentHP);
        anim.SetTrigger("Hit");
        currentHP -= damageAmount;
        if (currentHP <= 0)
        {
            Debug.Log("Why do you die???");
            died = true;
            body.bodyType = RigidbodyType2D.Static;
            gameObject.layer = 7;
        }
    }

    public bool IsDead()
    {
        return died;
    }

    //void OnDrawGizmosSelected()
    //{
    //    if (attackPoint.position == null)
    //        return;
    //    Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    //}

    //private void RestartLevel()
    //{
     //   SceneManagement.LoadScene(SceneManagement.GetActiveScene().name);
    //}
}
