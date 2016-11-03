using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public enum FireMode
    {
        Single,
        Burst,
        Automatic
    }

    [Header("Stats")]
    public float MuzzleVelocity;
    public FireMode fireMode = FireMode.Single;
    public int burstCount;
    public float msBetweenShots = 100;
    public int ProjectilesPerMag;
    public float reloadTime = 0.3f;
    
    [Header("Projectile")]
    public Projectile projectile;
    public Transform[] ProjectileSpawns;

    [Header("Effects")]
    public Transform shell;
    public Transform shellEjection;
    public AudioClip shootAudio;
    public AudioClip reloadAudio;

    AudioSource source;

    [Header("Recoil")]
    public Vector2 kickMinMax = new Vector2(.05f, .2f);
    public Vector2 recoilAngleMinMax = new Vector2(3, 5);
    public float recoilMoveSettleTime = .1f;
    public float recoilRotationSettleTime = .1f;

    Vector3 recoilSmoothDampVelocity;
    float recoilRotSmoothDampVelocity;
    float recoilAngle;

    Muzzleflash muzzleFlash;

    float nextShotTime;
    bool triggerReleasedSinceLastShot;
    int shotsRemainingInBurst;
    int projectilesRemainingInMag;

    bool isReloading;

    // Use this for initialization
    void Start ()
    {
        muzzleFlash = GetComponent<Muzzleflash>();
        shotsRemainingInBurst = burstCount;
        projectilesRemainingInMag = ProjectilesPerMag;
        source = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void LateUpdate()
    {
        // animate recoil
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, recoilMoveSettleTime);
        

        if (!isReloading)
        {
            recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, recoilRotationSettleTime);
            transform.localEulerAngles = Vector3.left * recoilAngle;
            if (projectilesRemainingInMag == 0)
                Reload();
        }
    }

    void Shoot()
    {
        if (!isReloading && Time.time > nextShotTime && projectilesRemainingInMag > 0)
        {
            // Firemodes
            if (fireMode == FireMode.Burst)
            {
                if (shotsRemainingInBurst == 0)
                {
                    return;
                }
                shotsRemainingInBurst--;
            }
            else if (fireMode == FireMode.Single)
            {
                if (!triggerReleasedSinceLastShot) return;
            }


            nextShotTime = Time.time + msBetweenShots / 1000f;
            // Spawn projectiles
            foreach (Transform spawn in ProjectileSpawns)
            {
                if (projectilesRemainingInMag == 0) break;
                projectilesRemainingInMag--;
                Projectile newProjectile = Instantiate(projectile, spawn.position, spawn.rotation) as Projectile;
                newProjectile.SetSpeed(MuzzleVelocity);
            }

            // Eject shell
            Instantiate(shell, shellEjection.position, shellEjection.rotation);
            // Muzzleflash
            muzzleFlash.Activate();

            // Initiate Recoil
            transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y);
            recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
            recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);

            source.PlayOneShot(shootAudio, 1);
        }
    }

    public void Reload()
    {
        if (!isReloading && projectilesRemainingInMag != ProjectilesPerMag)
        {
            StartCoroutine(AnimateReload());
        }
    }

    IEnumerator AnimateReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(.2f);

        source.PlayOneShot(reloadAudio, 1);

        float reloadSpeed = 1 / reloadTime;
        float percent = 0;

        Vector3 initialRot = transform.localEulerAngles;
        float maxReloadAngle = 30.0f;

        while (percent < 1)
        {
            percent += Time.deltaTime * reloadSpeed;

            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            transform.localEulerAngles = initialRot + Vector3.left * reloadAngle;

            yield return null;
        }

        isReloading = false;
        projectilesRemainingInMag = ProjectilesPerMag;
    }

    public void OnTriggerHold()
    {
        Shoot();
        triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerReleased()
    {
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = burstCount;
    }
}
