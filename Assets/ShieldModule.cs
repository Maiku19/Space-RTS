using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldModule : HealthModule
{
    [Header("Shield Settings")]
    [SerializeField] float maxHealth;
    [SerializeField] float healthRegen;
    [SerializeField] float healthRegenDelay;
    
    float currentHealth;

    public override float ProcessDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            if(currentHealth < 0) { currentHealth = 0; }
            return 0;
        }
        else
        {
            return damage;
        }
    }
}
