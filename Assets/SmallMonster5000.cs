using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMonster5000 : MonoBehaviour
{
    public Transform rayCast;
    public LayerMask rayCastMask;
    public float rayCastLength;
    public float attackDistance;
    public float moveSpeed;
    public float timer;
    private Collider2D attackArea;

    private RaycastHit2D hit;
    private GameObject target;
    private float distance;
    private bool attackMode;
    private bool inRange;
    private bool cooling;
    private float intTimer;
    private float damageAmount = 10;
    public LayerMask heroLayers;
    private Transform transform;
    private Transform testAttackPoint;

    private int maxHP = 30;
    private int currentHP;
    private Animator anim;
    private bool dead = false;

    void Awake()
    {
        intTimer = timer;
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            target = trig.gameObject;
            inRange = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        currentHP = 30;
        cooling = true;
        transform = GetComponent<Transform>();
        attackArea = transform.Find("AttackPoint").GetComponent<Collider2D>();
        testAttackPoint = transform.Find("testAttackPoint").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("dead", dead);
        if (dead) return;
        Debug.Log("Update");
        if (inRange)
        {
            Debug.Log("Hero in monster range");
            hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLength, rayCastMask);
        }


        if (hit.collider != null)
            EnemyLogic();
        else
            inRange = false;

    }

    void OnDrawGizmosSelected()
    {
        if (testAttackPoint.position == null)
            return;
        Gizmos.DrawWireSphere(testAttackPoint.position, 7);
    }

    void EnemyLogic()
    {
        Debug.Log("enemy logic");
        Debug.Log("cooling " + cooling);
        //distance = Vector2.Distance(transform.position, target.transform.position);

        if (!target.GetComponent<HeroScript>().IsDead() && !cooling) 
        {
            Debug.Log("enemy attack!!!");
            Attack();
        } else 
        {
            Cooldown();
        }
    }

    /*void Move()
    {
        anim.SetBool("canWalk", true);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Small_Monster_Attack"))
        {
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        }
    }*/

    void Attack()
    {
        timer = intTimer;
        //anim.SetBool("canWalk", false);
        Debug.Log("monster attacks");

        //Collider2D[] hitEnemies = new Collider2D[10];
        //ContactFilter2D filter = new ContactFilter2D();
        //filter.SetLayerMask(heroLayers);
        //Physics2D.OverlapCollider(attackArea, filter, hitEnemies);

        Collider2D enemy = Physics2D.OverlapCircle(testAttackPoint.position, 5, heroLayers);
        anim.SetTrigger("Attack");

        if (enemy != null)
        {
            Debug.Log("Hit hero");
            enemy.GetComponent<HeroScript>().TakeDamage(damageAmount);
        }
        cooling = true;

    }


    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        anim.SetTrigger("Hit");

        Debug.Log("Enemy HP " + currentHP);
        if (currentHP <= 0)
        {
            dead = true;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void Cooldown()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && cooling)
        {
            cooling = false;
            timer = intTimer;
        }
    }
}
