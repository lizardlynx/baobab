using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : StatefulBehaviour
{
    public int maxHealth = 100;
    [SerializeField] Transform player = null;
    public int range = 300;
    private int currentHealth;
    protected enum States : int
    {
        Idle,
        Hit,
        Death,
        Waiting
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        if (GetState<States>() == States.Idle && Vector2.Distance(transform.position, player.position) <= range)
            SetState(States.Waiting);
        else if (Vector2.Distance(transform.position, player.position) > range)
            SetState(States.Idle);
 
    }

    public void GetHit(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) Death();
        else TakeDamage();

    }

    public void TakeDamage()
    {
        SetState(States.Hit);
        StartCoroutine(OnAnimationComplete("Hit", () => SetState(States.Idle)));
    }

    public void Death()
    {
        SetState(States.Death);
        StartCoroutine(OnAnimationComplete("Death", () => {
            Destroy(gameObject);
            SceneManager.LoadScene("3000_level_1");
        }));
    }
}
