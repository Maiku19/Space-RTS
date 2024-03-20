using UnityEngine;
using System;

[RequireComponent(typeof(SuperTransform2D))]
[RequireComponent(typeof(Health))]
public class Unit : MonoBehaviour
{
    [SerializeField] protected Module[] modules;
    [SerializeField] protected SuperVector2 movementTarget = new SuperVector2(0, 0, 0, 0);

    private Health hp = null;
    protected Health objectHealth
    {
        get
        {
            return hp = hp == null ? GetComponent<Health>() : hp;
        }
    }

    protected float energy;
    protected float maxEnergy;

    SuperTransform2D st = null;
    protected SuperTransform2D SuperTransform
    {
        get
        { 
            if (st == null) { st = GetComponent<SuperTransform2D>(); }
            return st;
        }
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

        double num = targetpos.FullX - SuperTransform.FullPositionX;
        double num2 = targetpos.FullY - SuperTransform.FullPositionY;
        Vector2 v = new Vector2((float)num, (float)num2).normalized;

        // I'm unsure what I was trying to test here (I've made it a comment in case it's important)
        /*print("target reached");
        SuperTransform.SetPosition(new Vector2Int(targetpos.chunkX, targetpos.chunkY), new Vector2(targetpos.x, targetpos.y));
        return;*/

        SuperTransform.Move(v.x * speed, v.y * speed);
    }
}
