using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public enum FireMode
    {
        Single,
        Burst,
        Automatic
    }

    public Projectile projectile;
    public float MuzzleVelocity;
    public Transform[] ProjectileSpawns;
    public FireMode fireMode = FireMode.Single;


    // Use this for initialization
    void Start ()
    {
        Shoot();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void Shoot()
    {
        foreach (Transform spawn in ProjectileSpawns)
        {
            Projectile newProjectile = Instantiate(projectile, spawn.position, spawn.rotation) as Projectile;
            newProjectile.SetSpeed(MuzzleVelocity);
        }
    }
}
