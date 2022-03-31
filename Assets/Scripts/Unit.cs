using UnityEngine;
using System;

public class Unit : SuperTransform2D
{
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float maxShieldHealh;
    [SerializeField] protected Module[] modules;
    [SerializeField] protected GameObject deathFX;

    protected float health;
    protected float shieldHealth;
    [SerializeField] protected SuperVector2 movementTarget = new SuperVector2(0, 0, 0, 0);
    protected float energy;
    protected float maxEnergy;

    protected virtual void Awake()
    {
        
    }

    protected virtual void Update()
    {
        MoveUnit();
    }

    protected virtual void MoveUnit()
    {
        MoveTwards(movementTarget, 1_000 * Time.deltaTime);
    }

    public void DrainEnergy(float amount)
    {
        energy -= amount;
        energy = Mathf.Clamp(amount, 0, maxEnergy);
    }

    void MoveTwards(SuperVector2 targetpos, float speed)
    {
        // Unit moves twards target position regarding its own position in the world which causes the target to progresivly change course!!!

        double num = targetpos.FullX - FullPositionX;
        double num2 = targetpos.FullY - FullPositionY;
        Vector2 v = new Vector2((float)num, (float)num2).normalized;

        if (false)
        {
            print("target reached");
            SetPosition(new Vector2Int(targetpos.chunkX, targetpos.chunkY), new Vector2(targetpos.x, targetpos.y));
            return;
        }

        Move(v.x * speed, v.y * speed);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health == 0) { Die(); }
    }

    protected virtual void Die()
    {
        if(deathFX != null) Instantiate(deathFX, ChunkPosition, Quaternion.identity);
        Destroy(gameObject);
    } 
}
