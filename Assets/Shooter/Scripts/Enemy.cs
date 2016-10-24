using UnityEngine;
using System.Collections;


public class Enemy : MonoBehaviour, IDamageable
{
    public ParticleSystem deathEffect;
    
    public float Health = 100;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    public void Die()
    {
        GameObject.Destroy(gameObject);
    }

    public void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (damage >= Health)
            Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)) as GameObject, deathEffect.startLifetime);
        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0) Die();
    }
}
