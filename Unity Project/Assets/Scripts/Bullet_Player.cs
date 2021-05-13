using UnityEngine;

public class Bullet_Player : MonoBehaviour
{
    Rigidbody2D bulletRB;
    float speed = 5f; // Speed the bullet moves with
    private float health = 0; // Health of bullet
    int collision;

    // Reference for x-coordinates in "Player" script
    GameObject player;
    Player bulletXPosition;

    // Reference for "counter" in "SpawnBullet_Player" script
    GameObject bullet;
    public SpawnBullet_Player SBPlayer;

    Light bulletLight;

    bool isMoving = false;
    public bool IsMoving
    {
        get
        {
            return isMoving;
        }
        set
        {
            isMoving = value;
        }
    }

    private void Awake()
    {
        bulletRB = GetComponent<Rigidbody2D>();

        // Get the reference of "Player" (for player x poisition)
        player = GameObject.Find("Player");
        bulletXPosition = player.GetComponent<Player>();

        // Get the reference of "SpawnPoint_Bullet" (for counter)
        bullet = GameObject.Find("SpawnPoint_Bullet");
        SBPlayer = bullet.GetComponent<SpawnBullet_Player>();

        bulletLight = GetComponentInChildren<Light>();
    }

    private void Update()
    {
        // When the player is dead, destroy the bullet if one was spawned
        if (player == null)
        {
            Destroy(gameObject);
        }
        // While the bullet is not moving, set its x - position to that of the player
        if (isMoving == false && player != null)
        {
            // Saves players x-position in "x"
            float x = bulletXPosition.transform.position.x;
            // Sets the bullets new x-position
            transform.position = new Vector3(x, transform.position.y, 0);
        }
        else
            // Moves the bullet on the respective axis
            transform.Translate(0, Time.deltaTime * speed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletMonster" || collision.gameObject.tag == "Monster1" || collision.gameObject.tag == "Monster2")
        {
            // When the bullet is not shot and the "health" value is not set
            if (bulletRB.velocity.y == 0 && health == 0)
            {
                health = SBPlayer.Counter * 1.125f;
            }
            // Saves the amount of collisions the bullet has during the charging state
            this.collision++;
            health--;
            if (gameObject.transform.localScale.x > 0.25f)
            {
                gameObject.transform.localScale -= new Vector3(0.25f, 0.25f, 0);
                bulletLight.range -= 0.25f;
                
            }
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Gives the bullet its velocity and sets its "health" value
    /// </summary>
    public void ShootBullet()
    {
        isMoving = true;
        SBPlayer.ShootParticle();
        if (SBPlayer.Counter == 1)
        {
            health = SBPlayer.Counter - collision;
        }
        else
            // Sets the "health" value of the bullet depending on how many seconds it was charged, -collision amount, if it had any collisions during the charging state
            health = SBPlayer.Counter * 1.125f - collision;
    }
}
