using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public float startingHealth = 100f;
    public float damage = 10f;
    protected float health;
    public bool Isdead { get; protected set; }

    protected virtual void OnEnable()
    {
        Isdead = false;
        health = startingHealth;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage;
        if (health <= 0 && !Isdead)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Isdead = true;
    }
}