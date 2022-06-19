using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public GameObject DeathFX;
    private float hp;

    public Modules modules = null;
    public Modules Modules 
    { 
        get 
        { 
            if (modules == null) 
            { 
                modules = GetComponent<Modules>(); 
            } 
            return modules;
        } 
    }

    private void Awake()
    {
        hp = maxHealth;
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        print($"{gameObject.name} took {damage} damage");

        hp -= Modules.TakeDamage(damage);

        if(hp < 0)
        {
            Die(attacker);
        }
    }

    public void Die(GameObject killer)
    {
        print($"{gameObject.name} was killed by {killer.name}");
        Instantiate(DeathFX, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
