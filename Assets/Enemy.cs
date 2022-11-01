using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : StatefulBehaviour
{
    private int hp;
    protected enum States : int
    {
        Idle,
        Hit
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // UpdateState();
    }

    public void GetHit()
    {
        Debug.Log("hit");
        SetState(States.Hit);
    }
}
