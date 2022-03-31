using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    public float energyUsageIdle;
    public float energyUsage;
    public bool isOn = true;

    private Unit unit;

    private void Awake()
    {
        transform.parent.TryGetComponent(out unit);
    }

    private void Update()
    {
        unit.DrainEnergy(energyUsageIdle);
    }
}
