using UnityEngine;

public class Bullet_Monster1 : MonoBehaviour
{
    Rigidbody2D bulletRB;
    Vector2 speed = new Vector2(0, -1.0f); // Moves the monster downwards

    public GameController gameController;

    public AudioClip hit;
    public GameObject particle;

    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        bulletRB.velocity = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletPlayer" || collision.gameObject.tag == "Shield")
        {
            Instantiate(particle, transform.position, Quaternion.identity);
            gameController.PlaySound(hit);
            UI.globalScore += 1;
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(particle, transform.position, Quaternion.identity);
            UI.globalScore += 1;
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
