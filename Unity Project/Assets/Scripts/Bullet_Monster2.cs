using UnityEngine;

public class Bullet_Monster2 : MonoBehaviour
{
    GameObject player; // Gameobject the bullet will chase after
    float speed = 1.0f; // Speed of the bullet

    public GameController gameController;

    public AudioClip hit;
    public GameObject particle;

    void Start()
    {
        // Looks for a gamebject with the tag "Player"
        player = GameObject.FindWithTag("Player");
        
        // if the gameobject exists in the scene
        if (player != null)
        {
            // Substract bullet coordinates with target coordinates to get the direction vector
            Vector3 v3 = (player.transform.position - transform.position).normalized;
            // Saves x and y coordinates of target vector into "v2"
            Vector2 v2 = new Vector2(v3.x, v3.y);
            // Bullet will only chase "player" if its y-position is >= that of the "player"
            if (transform.position.y >= player.transform.position.y)
            {
                // Shoots the bullet to "v2" coordinates with specified speed
                GetComponent<Rigidbody2D>().velocity = v2 * speed;
            }
            else
                // Shoots the bullet straight down if the monster is beneath the "player"
                GetComponent<Rigidbody2D>().velocity = Vector2.down * speed;
        }
        else
            GetComponent<Rigidbody2D>().velocity = Vector2.down * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletPlayer" || collision.gameObject.tag == "Shield")
        {
            Instantiate(particle, transform.position, Quaternion.identity);
            gameController.PlaySound(hit);
            UI.globalScore += 2;
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(particle, transform.position, Quaternion.identity);
            UI.globalScore += 2;
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
