using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int attackDamage = 40;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D  col)
    {
        GameObject enemy = col.gameObject;
        if (enemy.tag == "Enemy")
        {
            Debug.Log("Hit trigger");
            Enemy script = enemy.GetComponent<Enemy>();
            script.GetHit(attackDamage);
        }
    }
}
