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

    [Header("Recoil")]
    public Vector2 kickMinMax = new Vector2(.05f, .2f);
    public Vector2 recoilAngleMinMax = new Vector2(3, 5);
    public float recoilMoveSettleTime = .1f;
    public float recoilRotationSettleTime = .1f;

    Vector3 recoilSmoothDampVelocity;
    float recoilRotSmoothDampVelocity;
    float recoilAngle;


    // Use this for initialization
    void Start ()
    {
        Shoot();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void LateUpdate()
    {
        // animate recoil
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, recoilMoveSettleTime);
        recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, recoilRotationSettleTime);
        transform.localEulerAngles = Vector3.left * recoilAngle;
    }

    void Shoot()
    {
        foreach (Transform spawn in ProjectileSpawns)
        {
            Projectile newProjectile = Instantiate(projectile, spawn.position, spawn.rotation) as Projectile;
            newProjectile.SetSpeed(MuzzleVelocity);
        }

        // Initiate Recoil
        transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y);
        recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
        recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);
    }
}
