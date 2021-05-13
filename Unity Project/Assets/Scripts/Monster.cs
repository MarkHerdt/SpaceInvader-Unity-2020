using UnityEngine;

public class Monster : MonoBehaviour
{
    Rigidbody2D monsterRB;
    Vector2 speed = new Vector2(0, -0.25f);

    Player player;
    public GameController gamecontroller;

    public AudioClip hit;
    public GameObject particle;

    private void Awake()
    {
        monsterRB = GetComponent<Rigidbody2D>();
        gamecontroller = GameObject.FindObjectOfType<GameController>();
    }

    private void Update()
    {
        monsterRB.velocity = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BulletPlayer" || collision.gameObject.tag == "Player")
        {
            Instantiate(particle, transform.position, Quaternion.identity);
            gamecontroller.PlaySound(hit);
            if (gameObject.tag == "Monster1")
            {
                UI.globalScore += 5;
                Destroy(gameObject);
            }
            else if (gameObject.tag == "Monster2")
            {
                UI.globalScore  += 10;
                Destroy(gameObject);
            }
            else
            {

            }
        }
        // When a monster hits the wall, destroy it and do 1 dmg to the player
        else if (collision.gameObject.tag == "Wall")
        {
            if (gamecontroller.Playing == true)
            {
                player = GameObject.FindObjectOfType<Player>();
                if (player != null)
                {
                    player.dmgTaken();
                }
            }
            Destroy(gameObject);
        }
    }
}
