using System.Collections;
using UnityEngine;

public class SpawnBullet_Player : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab of gameobject
    GameObject bullet;
    Light bulletLight;

    public GameObject player;

    public AudioSource source;
    public AudioClip bulletCharge;
    public AudioClip bulletShot;

    public GameObject particle1;
    public GameObject particle2;
    GameObject particle3; 

    private float time;
    private int counter; // Seconds that have passed
    public int Counter
    {
        get
        {
            return counter;
        }
        set
        {
            counter = value;
        }
    }
    private float cooldown = 0.5f; // Cooldown between shots
    private float timeStamp = 0.0f;

    Coroutine bulletGrowth;

    // Member for coroutin "DoubleClickDetection"
    private float firstClickTime, timeBetweenClicks;
    private bool coroutineAllowed;
    private int clickCounter;
    private float aoeTimeStamp;
    private int aoeCooldown = 10;
    public int AoeCooldown { get { return aoeCooldown; } }
    private bool aoe;

    public delegate void aoeActivated();
    public event aoeActivated AoeActivated;

    public BuffManager bulletBuff;

    // Member for coroutine "BulletBarrage"
    Coroutine barrage, replenish;
    public int barrageAmount = 10;
    private int shotBullets;
    private bool reachedZero;
    private bool waiting;

    public delegate void BarrageBulletShot();
    public event BarrageBulletShot BulletShot;
    public event BarrageBulletShot BulletReplenished;

    private void Awake()
    {
        particle3 = particle1;
    }

    private void Start()
    {
        firstClickTime = 0f;
        timeBetweenClicks = 0.2f;
        clickCounter = 0;
        coroutineAllowed = true;
        aoeTimeStamp = -10;
    }

    private void Update()
    {
        // Checks if there's still an existing bullet and shoots it if yes
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (bullet != null)
            {
                bullet.GetComponent<Bullet_Player>().ShootBullet();
                counter = 0;
            }
        }

        // Check if spacebutton was double tapped
        if (Input.GetKeyDown(KeyCode.Space))
        {
            clickCounter += 1;
        }
        if (clickCounter == 1 && coroutineAllowed)
        {
            firstClickTime = Time.time;
            StartCoroutine(DoubleClickDetection());
        }
        
        // Only shoots bullet, if the value of the time that has passed is greater than the value of "timestamp" + "cooldown"
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= (timeStamp + cooldown))
        {
            Invoke("ChargeSound", .1f);
            // Resets the counter
            counter = 0;
            // Spawns the bullet            
            bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            // Reference for the "Light" in "bullet"
            bulletLight = bullet.GetComponentInChildren<Light>();
            if (bulletBuff.BulletBuffActive == false)
            {
                // Sets the light range to its initial value, when a new bullet is spawned
                bulletLight.range = 1;
            }
            // Set "time" x-amount smaller then the value in "if (time > x) {}" to get a delay for x seconds when key is pressed
            // Set them both to the same value to get no delay
            time = 0;
        }
        // While key is held down, "counter++" for every second
        if (Input.GetKey(KeyCode.Space))
        {
            time += Time.deltaTime;
            // Set value x-amount bigger then the value of "time" in "if (Input.GetKeyDown(KeyCode.Space) && Time.time >= (timeStamp + cooldown)) {}" to get a delay for x seconds when key is pressed
            // Set them both to the same value to get no delay
            if (time > 0)
            {
                time -= 1;
                // While key is held down, "counter++" for every second
                counter++;
                // Debug.Log("Counter: " + counter);
                // Only enters the if-statement when the space key is still held down after at least one cycle
                if (bullet == null)
                {
                    ChargeSound();
                    // Spawns a new "bullet" (For when the space button is pressed continously)
                    bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    bulletLight = bullet.GetComponentInChildren<Light>();
                    bulletLight.range = 1;
                }
                if (bulletBuff.BulletBuffActive == false)
                {
                    // Increases the scale of the "bullet" and "Light" each second the space key is pressed
                    bulletGrowth = StartCoroutine(BulletGrowth());
                }
                else
                {
                    bullet.transform.localScale = new Vector3(1.75f, 1.75f, 0);
                    bulletLight.range = 2.25f;
                    counter = 5;
                }
            }
        }
        // After 5 seconds, start counting from 1 again
        if (counter >= 5)
        {
            Instantiate(particle2, transform.position, Quaternion.identity);
            ShootSound();
            // Calls the "ShootBullet();" method from the "BulletPlayer" script
            bullet.GetComponent<Bullet_Player>().ShootBullet();
            if (bulletGrowth != null)
            {
                StopCoroutine(bulletGrowth);
            }
            // Saves the time when the last bullet was shot into "timestamp"
            timeStamp = Time.time;
            // Resets the counter
            counter = 0;
            // Resets "bullet" back to "null", so antoher "bullet" can be spawned
            bullet = null;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (bullet != null)
            {
                source.Stop();
                ShootSound();
                // Calls the "ShootBullet();" method from the "BulletPlayer" script
                bullet.GetComponent<Bullet_Player>().ShootBullet();
                if (bulletGrowth != null)
                {
                    StopCoroutine(bulletGrowth);
                }
                // Saves the time when the last bullet was shot into "timestamp"
                timeStamp = Time.time;
                counter = 0;
            }
            // Resets "bullet" back to "null", so antoher "bullet" can be spawned
            bullet = null;
        }

        // Excecutes the coroutine "BulletBarrage", while the shift key is pressed
        if (Input.GetKeyDown(KeyCode.S))
        {
            reachedZero = false;
            // Stops the "ReplenishBarrage" coroutin, if one is running
            if (replenish != null)
            {
                StopCoroutine(replenish);
                waiting = false;
            }
            barrage = StartCoroutine(BulletBarrage());
        }
        // When "barrageAmount" == 0 start the coroutine "ReplenishBarrage", even when the shift key is still pressed
        if (barrageAmount - shotBullets == 0)
        {
            if (replenish != null)
            {
                StopCoroutine(replenish);
            }
            reachedZero = true;
            barrageAmount -= shotBullets;
            shotBullets = 0;

            //Debug.Log("Test");

            replenish = StartCoroutine(ReplenishBarrage());
        }
        // Stops the coroutine "BulletBarrage" when the S key is released
        if (Input.GetKeyUp(KeyCode.S))
        {
            if (barrage != null)
            {
                StopCoroutine(barrage);
            }
            // Only substracts, if the "barrageAmount" hasn't reached 0 while the shift key was still pressed
            if (reachedZero == false)
            {
                barrageAmount -= shotBullets;
            }
        }
        // Starts the "ReplenishBarrage" coroutine, when the shift key is released
        if (!Input.GetKey(KeyCode.S) && barrageAmount != 10 && waiting == false)
        {
            replenish = StartCoroutine(ReplenishBarrage());
        }
    }

    /// <summary>
    /// Makes the bulletgrowth look more smooth
    /// </summary>
    /// <returns></returns>
    IEnumerator BulletGrowth()
    {
        for (int i = 0; i < 100; i++)
        {
            if (bullet != null)
            {
                bullet.transform.localScale += new Vector3(.0025f, .0025f, 0);
                bulletLight.range += .0025f;
                yield return new WaitForSeconds(.01f);
            }
        }
    }

    /// <summary>
    /// If spacebutton was double tapped, shoot additional bullets
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoubleClickDetection()
    {
        coroutineAllowed = false;
        while (Time.time < firstClickTime + timeBetweenClicks && Time.time >= aoeTimeStamp + aoeCooldown)
        {
            if (clickCounter == 2)
            {
                if (bullet != null)
                {
                    bullet.GetComponent<Bullet_Player>().ShootBullet();
                    counter = 0;
                }
                aoe = true;
                particle3.transform.localScale = new Vector3(5f, 5f, 5f);
                Instantiate(particle3, player.transform.position, Quaternion.identity);
                AoeActivated?.Invoke();
                aoeTimeStamp = Time.time;

                bullet = Instantiate(bulletPrefab, new Vector2(transform.position.x - 0.725f, transform.position.y - 0.25f), transform.rotation * Quaternion.Euler(0, 0, 33.75f));
                bulletLight = bullet.GetComponentInChildren<Light>();
                bulletLight.range = 1;
                ShootSound();
                bullet.GetComponent<Bullet_Player>().ShootBullet();
                bullet = Instantiate(bulletPrefab, new Vector2(transform.position.x + 0.725f, transform.position.y - 0.25f), transform.rotation * Quaternion.Euler(0, 0, -33.75f));
                bulletLight = bullet.GetComponentInChildren<Light>();
                bulletLight.range = 1;
                ShootSound();
                bullet.GetComponent<Bullet_Player>().ShootBullet();
                bullet = Instantiate(bulletPrefab, new Vector2(transform.position.x - 1, transform.position.y - 0.625f), transform.rotation * Quaternion.Euler(0, 0, 45));
                bulletLight = bullet.GetComponentInChildren<Light>();
                bulletLight.range = 1;
                ShootSound();
                bullet.GetComponent<Bullet_Player>().ShootBullet();
                bullet = Instantiate(bulletPrefab, new Vector2(transform.position.x + 1, transform.position.y - 0.625f), transform.rotation * Quaternion.Euler(0, 0, -45));
                bulletLight = bullet.GetComponentInChildren<Light>();
                bulletLight.range = 1;
                ShootSound();
                bullet.GetComponent<Bullet_Player>().ShootBullet();
                bullet = Instantiate(bulletPrefab, new Vector2(transform.position.x - 1, transform.position.y - 1), transform.rotation * Quaternion.Euler(0, 0, 67.5f));
                bulletLight = bullet.GetComponentInChildren<Light>();
                bulletLight.range = 1;
                ShootSound();
                bullet.GetComponent<Bullet_Player>().ShootBullet();
                bullet = Instantiate(bulletPrefab, new Vector2(transform.position.x + 1, transform.position.y - 1), transform.rotation * Quaternion.Euler(0, 0, -67.5f));
                bulletLight = bullet.GetComponentInChildren<Light>();
                bulletLight.range = 1;
                ShootSound();
                bullet.GetComponent<Bullet_Player>().ShootBullet();
                bullet = Instantiate(bulletPrefab, new Vector2(transform.position.x - 1.125f, transform.position.y - 1.375f), transform.rotation * Quaternion.Euler(0, 0, 73.125f));
                bulletLight = bullet.GetComponentInChildren<Light>();
                bulletLight.range = 1;
                ShootSound();
                bullet.GetComponent<Bullet_Player>().ShootBullet();
                bullet = Instantiate(bulletPrefab, new Vector2(transform.position.x + 1.125f, transform.position.y - 1.375f), transform.rotation * Quaternion.Euler(0, 0, -73.125f));
                bulletLight = bullet.GetComponentInChildren<Light>();
                bulletLight.range = 1;
                ShootSound();
                bullet.GetComponent<Bullet_Player>().ShootBullet();
                bullet = Instantiate(bulletPrefab, new Vector2(transform.position.x - 1, transform.position.y - 1.75f), transform.rotation * Quaternion.Euler(0, 0, 78.75f));
                bulletLight = bullet.GetComponentInChildren<Light>();
                bulletLight.range = 1;
                ShootSound();
                bullet.GetComponent<Bullet_Player>().ShootBullet();
                bullet = Instantiate(bulletPrefab, new Vector2(transform.position.x + 1, transform.position.y - 1.75f), transform.rotation * Quaternion.Euler(0, 0, -78.75f));
                bulletLight = bullet.GetComponentInChildren<Light>();
                bulletLight.range = 1;
                ShootSound();
                bullet.GetComponent<Bullet_Player>().ShootBullet();
                counter = 0;
                aoe = false;
                bullet = null;
                bulletLight = null;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        clickCounter = 0;
        firstClickTime = 0f;
        coroutineAllowed = true;
    }

    /// <summary>
    /// Shoots a bullet barrage of up to 10 buullets
    /// </summary>
    /// <returns></returns>
    IEnumerator BulletBarrage()
    {
        shotBullets = 0;
        if (bullet != null)
        {
            bullet.GetComponent<Bullet_Player>().ShootBullet();
            counter = 0;
        }
        for (int i = 0; i < barrageAmount; i++)
        {
            if (barrageAmount > i)
            {
                bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bulletLight = bullet.GetComponentInChildren<Light>();
                bulletLight.range = 1;
                counter = 1;
                ShootSound();
                bullet.GetComponent<Bullet_Player>().ShootBullet();
                counter = 0;
                shotBullets++;
                BulletShot?.Invoke();
                bullet = null;
                yield return new WaitForSeconds(.1f);
            }
        }
    }
    /// <summary>
    /// Replenishes one bullet of "BulletBarrage" each second
    /// </summary>
    /// <returns></returns>
    IEnumerator ReplenishBarrage()
    {
        waiting = true;

        for (int i = 0; i <= barrageAmount; i++)
        {
            if (barrageAmount >= 10)
            {
                break;
            }
            barrageAmount++;
            BulletReplenished?.Invoke();
            yield return new WaitForSeconds(1);
        }
        waiting = false;
    }

    /// <summary>
    /// Soundeffect while the bullet is charging
    /// </summary>
    void ChargeSound()
    {
        if (source.clip != bulletCharge)
        {
            source.clip = bulletCharge;
        }
        source.Play();
    }
    /// <summary>
    /// Soundeffect when the bullet is shot
    /// </summary>
    void ShootSound()
    {
        CancelInvoke("ChargeSound");
        if (source.clip != bulletShot)
        {
            source.clip = bulletShot;
        }
        source.Play();
    }

    /// <summary>
    /// Particle effect for bulletshots
    /// </summary>
    public void ShootParticle()
    {
        float x = .25f;
        float y = .25f;
        float z = .25f;

        x *= counter;
        y *= counter;
        z *= counter;

        // Increases the particel effect for each second the bullet has charged
        // Sets the particle size to 1 when the bullet is shot through an aoe
        // Won't play this effect when the counter has reached 5
        if (aoe == false || counter != 5)
        {
            particle1.transform.localScale = new Vector3(1 + x, 1 + y, 1 + z);
            Instantiate(particle1, transform.position, Quaternion.identity);
        }
    }
}
