using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modules : MonoBehaviour
{
    public Module[] InstalledModules;

    public float TakeDamage(float damage)
    {
        float processedDamage = damage;
        foreach (HealthModule module in InstalledModules)
        {
            if (module.isOn)
            {
                processedDamage = module.ProcessDamage(processedDamage);
            }
        }

        return processedDamage;
    }
}
