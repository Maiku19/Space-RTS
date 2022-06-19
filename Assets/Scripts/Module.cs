using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    public string ModuleName { get { return name; } }

    [Header("Common Settings")]
    public float energyUsageIdle;
    public float energyUsageActive;

    public bool isOn = true;
    public bool modifiesHealth = false;

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
