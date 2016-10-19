using UnityEngine;
using System.Collections;


public class DeathToParticles : MonoBehaviour, IDamageable
{

    ParticleSystem.EmissionModule particles;
    public float Health = 100;
	// Use this for initialization
	void Start () {
        particles = GetComponent<ParticleSystem>().emission;
        particles.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (particles.enabled)
        {

        }
	}

    public void Die()
    {
        //GetComponent<MeshRenderer>().enabled = false;
        particles.enabled = true;
    }

    public void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0) Die();
    }
}
