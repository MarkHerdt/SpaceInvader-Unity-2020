using UnityEngine;

public class PowerUps : MonoBehaviour
{
    Rigidbody2D rb;
    SpawnPowerUp spawnPowerUp;
    public GameController gameController;

    public GameObject healthPrefab;
    public GameObject shieldPrefab;
    public GameObject bulletPrefab;

    public delegate void Buffs();
    public static event Buffs HealthBuff;
    public static event Buffs ShieldBuff;
    public static event Buffs BulletBuff;

    public AudioClip powerUp;
    public GameObject particle1;
    public GameObject particle2;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPowerUp = GameObject.FindObjectOfType<SpawnPowerUp>();
    }

    private void Start()
    {
        if (spawnPowerUp != null)
        {
            rb.velocity = new Vector2(spawnPowerUp.transform.position.x + Random.Range(5f, 7.5f), 0);
        }
    }

    private void Update()
    {
        transform.Translate(0, .0025f, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(particle1, transform.position, Quaternion.identity);
            Instantiate(particle2, collision.transform.position, Quaternion.identity);
            gameController.PlaySound(powerUp);
            if (gameObject.tag == "PUHealth")
            {
                HealthBuff();
            }
            if (gameObject.tag == "PUShield")
            {
                ShieldBuff();
            }
            if (gameObject.tag == "PUBullet")
            {
                BulletBuff();
            }
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
