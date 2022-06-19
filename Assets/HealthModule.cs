using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthModule : Module
{
    public virtual float ProcessDamage(float damage)
    {
        return damage;
    }
}
