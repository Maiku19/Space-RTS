using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShield : Module
{
    [Header("Shield Settings")]
    [SerializeField] float maxHealth;
    [SerializeField] float healthRegen;
    [SerializeField] float healthRegenDelay;

    [SerializeField] Vector2 shape = Vector2.one;

    float currentHealth;

    float timer = Mathf.Infinity; 
    private void Update()
    {
        if(timer >= healthRegenDelay)
        {
            RegenerateHealth();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth > 0)
        {
            timer = 0;
            currentHealth -= damage;
            if (currentHealth < 0) { currentHealth = 0; }
        }
    }

    public void RegenerateHealth()
    {
        // area of an oval = Mathf.PI * shape.x / 2 * shape.y / 2
        throw new System.NotImplementedException();
    }
}
